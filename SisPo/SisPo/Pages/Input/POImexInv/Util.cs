using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SisData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace SisPo.Pages.Input.POImexInv
{    
    public interface ITaxInfoService
    {
        Task<DataTable> GetInfoMSTAsync(string mst);
        Task<DataTable> GetMstInfoAsync(string mst);
    }   
    public class TaxInfoService
    {
        private const string ApiBaseUrl = "https://api.fast.com.vn";
        private const string ServiceName = "GetCompanyInformation";
        
        private readonly IDbManager _dbManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TaxInfoService> _logger;
        
        private static readonly Dictionary<string, string> _mstCache = new();
        private static readonly Dictionary<string, string> _fieldMapping = new()
        {
            { "Name", "ten_cong_ty" },
            { "Address", "dia_chi" },
            { "Status", "tinh_trang" }
        };

        public TaxInfoService(IDbManager dbManager, IHttpClientFactory httpClientFactory, ILogger<TaxInfoService> logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DataTable> GetInfoMSTAsync(string mst)
        {
            try
            {
                const string tableStartTag = "<table class=\"table-taxinfo\">";
                const string tableEndTag = "</table>";

                var url = $"https://masothue.vn/Search/?q={mst}&type=auto";
                var webCode = await GetWebCodeAsync(url);

                if (string.IsNullOrEmpty(webCode))
                    return null;

                DataTable result;
                if (!webCode.Contains("table-taxinfo"))
                {
                    result = webCode.Contains("text-center") 
                        ? await ConvertToDataTableAsync(webCode, mst, tableStartTag, tableEndTag) 
                        : null;
                }
                else
                {
                    var startIndex = webCode.ToLower().IndexOf(tableStartTag, StringComparison.Ordinal);
                    webCode = webCode.Substring(startIndex);
                    var endIndex = webCode.ToLower().IndexOf(tableEndTag, StringComparison.Ordinal);
                    webCode = webCode.Substring(0, endIndex + tableEndTag.Length);
                    result = ConvertHtmlToDataTable(webCode);
                }

                // Merge with API data
                var apiData = await GetMstInfoAsync(mst);
                if (apiData != null && apiData.Rows.Count > 0)
                {
                    result ??= CreateEmptyTaxInfoTable();
                    
                    foreach (DataRow apiRow in apiData.Rows)
                    {
                        var key = apiRow["key"]?.ToString()?.Trim();
                        var existingRow = result.Rows.Cast<DataRow>()
                            .FirstOrDefault(x => x["key"]?.ToString()?.Trim()
                            .Equals(key, StringComparison.OrdinalIgnoreCase) == true);

                        if (existingRow == null)
                        {
                            result.Rows.Add(apiRow["key"], apiRow["value"]);
                        }
                        else
                        {
                            existingRow["value"] = apiRow["value"];
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting MST info for {MST}", mst);
                return null;
            }
        }

        public async Task<DataTable> GetMstInfoAsync(string mst)
        {
            if (string.IsNullOrWhiteSpace(mst))
                return null;

            try
            {
                mst = mst.Trim();

                // Check cache first
                if (_mstCache.TryGetValue(mst, out var cachedData))
                {
                    return ParseJsonToDataTable(cachedData);
                }

                // Call API - This needs to be implemented based on your actual API service
                // For now, returning null as placeholder
                // var apiResponse = await CallApiServiceAsync(...);
                
                _logger.LogInformation("Getting MST info for {MST} from API", mst);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting MST info from API for {MST}", mst);
                return null;
            }
        }

        private async Task<string> GetWebCodeAsync(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TaxInfo");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", GenerateRandomUserAgent());
                
                var response = await client.GetAsync(url);
                
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Access forbidden to {Url}", url);
                    return string.Empty;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading web content from {Url}", url);
                return string.Empty;
            }
        }

        private async Task<DataTable> ConvertToDataTableAsync(string html, string mst, string startKey, string endKey)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var linkNode = htmlDoc.DocumentNode.Descendants("a")
                .FirstOrDefault(x => x.InnerText.Equals(mst, StringComparison.OrdinalIgnoreCase));

            if (linkNode == null)
                return null;

            var detailUrl = "https://masothue.vn" + linkNode.Attributes["href"].Value;
            var detailHtml = await GetWebCodeAsync(detailUrl);

            if (string.IsNullOrEmpty(detailHtml))
                return null;

            if (!detailHtml.Contains("table-taxinfo"))
                return await ConvertToDataTableAsync(detailHtml, mst, startKey, endKey);

            var startIndex = detailHtml.ToLower().IndexOf(startKey, StringComparison.Ordinal);
            if (startIndex < 0) return null;
            
            detailHtml = detailHtml.Substring(startIndex);
            var endIndex = detailHtml.ToLower().IndexOf(endKey, StringComparison.Ordinal);
            if (endIndex < 0) return null;
            
            detailHtml = detailHtml.Substring(0, endIndex + endKey.Length);

            return ConvertHtmlToDataTable(detailHtml);
        }

        private DataTable ConvertHtmlToDataTable(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var headerRows = htmlDoc.DocumentNode.SelectNodes("//table/thead/tr");
            var bodyRows = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");

            var dataTable = CreateEmptyTaxInfoTable();

            // Process header
            if (headerRows != null && headerRows.Count > 0)
            {
                var headers = headerRows[0].Elements("th").Select(th => th.InnerText.Trim());
                foreach (var header in headers.Where(h => !string.IsNullOrWhiteSpace(h)))
                {
                    dataTable.Rows.Add("ten_cong_ty", header);
                }
            }

            // Process body
            if (bodyRows != null)
            {
                foreach (var tr in bodyRows)
                {
                    var cells = tr.Elements("td").Select(td => td.InnerText.Trim()).ToArray();
                    
                    if (cells.Length == 0)
                        continue;

                    var key = cells[0].Trim().ToLower().Replace(" ", "_");
                    var value = cells.Length > 1 ? cells[1] : string.Empty;

                    // Handle special case for representative
                    if (key.Equals("nguoi_dai_dien", StringComparison.OrdinalIgnoreCase) && cells.Length > 1)
                    {
                        var tdNodes = tr.Elements("td").ToArray();
                        if (tdNodes.Length > 1)
                        {
                            var innerHtml = tdNodes[1].InnerHtml;
                            var tempDoc = new HtmlDocument();
                            tempDoc.LoadHtml(innerHtml);
                            var linkNode = tempDoc.DocumentNode.SelectSingleNode("//a");
                            value = linkNode?.InnerText.Trim() ?? value;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(key) || !string.IsNullOrWhiteSpace(value))
                    {
                        dataTable.Rows.Add(key, value);
                    }
                }
            }

            return dataTable;
        }

        private async Task SaveTaxInfoAsync(params TaxInfo[] taxInfos)
        {
            if (taxInfos == null || taxInfos.Length == 0)
                return;

            try
            {
                var batch = taxInfos.Take(50).ToArray();
                var queries = batch.Select(BuildUpdateQuery).Where(q => !string.IsNullOrEmpty(q));
                var sqlQuery = string.Join("\n", queries);

                if (!string.IsNullOrWhiteSpace(sqlQuery))
                {
                    await _dbManager.ExecuteSqlQueryAsync(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving tax info to database");
            }
        }

        private static string BuildUpdateQuery(TaxInfo taxInfo)
        {
            if (taxInfo == null || string.IsNullOrWhiteSpace(taxInfo.TaxCode))
                return string.Empty;
                
            var name = taxInfo.Name?.Replace("'", "''") ?? string.Empty;
            var status = TranslateStatus(taxInfo.Status);
            return $"EXEC dbo.UpdateOrInsertMst '{taxInfo.TaxCode}', N'{name}', N'{status}'";
        }

        private DataTable ParseJsonToDataTable(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return null;

            var dataTable = CreateEmptyTaxInfoTable();

            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                if (jObject == null)
                    return null;

                foreach (var property in jObject.Properties())
                {
                    var value = property.Value?.ToString();
                    if (string.IsNullOrEmpty(value) || 
                        "taxcode".Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if ("status".Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        dataTable.Rows.Add("status", value);
                        dataTable.Rows.Add(MapFieldName(property.Name), TranslateStatus(value));
                    }
                    else
                    {
                        dataTable.Rows.Add(MapFieldName(property.Name), value);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing JSON to DataTable");
                return null;
            }
        }

        private static DataTable CreateEmptyTaxInfoTable()
        {
            var table = new DataTable("info");
            table.Columns.Add("key", typeof(string));
            table.Columns.Add("value", typeof(string));
            return table;
        }

        private static string MapFieldName(string fieldName)
        {
            return _fieldMapping.TryGetValue(fieldName, out var mapped) ? mapped : fieldName;
        }

        private static string TranslateStatus(string statusCode)
        {
            return statusCode switch
            {
                "00" => "?ang ho?t ??ng (?ã ???c c?p GCN ?KT)",
                "01" => "Ng?ng ho?t ??ng và ?ã ?óng MST",
                "02" => "?ã chuy?n c? quan thu? qu?n lý",
                "03" => "Ng?ng ho?t ??ng nh?ng ch?a hoàn thành th? t?c ?óng MST",
                "04" => "?ang ho?t ??ng (???c c?p thông báo MST)",
                "04-01" => "Vi ph?m pháp lu?t ?ã chuy?n c? quan công an",
                "05" => "T?m ngh? kinh doanh có th?i h?n",
                "06" => "Không ho?t ??ng t?i ??a ch? ?ã ??ng ký",
                "00-01" => "Vi ph?m ?ã chuy?n c? quan công an",
                _ => statusCode ?? string.Empty
            };
        }

        private static string GenerateRandomUserAgent()
        {
            var random = new Random();
            var prefix = GenerateRandomString(10, false);
            var suffix = random.Next(1, 999999999);
            return $"Mozilla/5.0 {prefix}{suffix}";
        }

        private static string GenerateRandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder(size);
            var random = new Random();
            
            for (int i = 0; i < size; i++)
            {
                char c = (char)random.Next('A', 'Z' + 1);
                builder.Append(c);
            }
            
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }

    /// <summary>
    /// Tax Information model
    /// </summary>
    public class TaxInfo
    {
        public string TaxCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
   
    public static class Util
    {
        public static DataTable GetInfoMST(string mst, IDbManager dbManager, ISysVarManager mySysvar)
        {
            try
            {
                const string tableStartTag = "<table class=\"table-taxinfo\">";
                const string tableEndTag = "</table>";

                var url = $"https://masothue.vn/Search/?q={mst}&type=auto";
                var webCode = getWebCode(url, mySysvar);

                if (string.IsNullOrEmpty(webCode))
                    return null;

                DataTable result;
                if (!webCode.Contains("table-taxinfo"))
                {
                    result = webCode.Contains("text-center")? convToDataTable(webCode, mst, tableStartTag, tableEndTag, mySysvar) : null;
                }
                else
                {
                    var startIndex = webCode.ToLower().IndexOf(tableStartTag, StringComparison.Ordinal);
                    webCode = webCode.Substring(startIndex);
                    var endIndex = webCode.ToLower().IndexOf(tableEndTag, StringComparison.Ordinal);
                    webCode = webCode.Substring(0, endIndex + tableEndTag.Length);
                    result = ConvertHtmlToDataTable(webCode);
                }

                //// Merge with API data
                //var apiData = await GetMstInfoAsync(mst, dbManager, mySysvar);
                //if (apiData != null && apiData.Rows.Count > 0)
                //{
                //    result ??= CreateEmptyTaxInfoTable();
                    
                //    foreach (DataRow apiRow in apiData.Rows)
                //    {
                //        var key = apiRow["key"]?.ToString()?.Trim();
                //        var existingRow = result.Rows.Cast<DataRow>()
                //            .FirstOrDefault(x => x["key"]?.ToString()?.Trim()
                //            .Equals(key, StringComparison.OrdinalIgnoreCase) == true);

                //        if (existingRow == null)
                //        {
                //            result.Rows.Add(apiRow["key"], apiRow["value"]);
                //        }
                //        else
                //        {
                //            existingRow["value"] = apiRow["value"];
                //        }
                //    }
                //}

                return result;
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        private static DataTable convToDataTable(string html, string mst, string sKey, string eKey, ISysVarManager mySysvar)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode htmlNode = (from x in htmlDocument.DocumentNode.Descendants("a")
                                 where x.InnerText.Equals(mst)
                                 select x).FirstOrDefault();
            DataTable result = null;
            if (htmlNode != null)
            {
                string text = "https://masothue.vn";
                string url = text + htmlNode.Attributes["href"].Value;
                string webCode = getWebCode(url, mySysvar);
                if (webCode.Contains("table-taxinfo"))
                {
                    webCode = webCode.Substring(webCode.ToLower().IndexOf(sKey));
                    webCode = webCode.Substring(0, webCode.ToLower().IndexOf(eKey) + eKey.Length);
                    result = convert_HTML_to_DataTable(webCode);
                }
                else
                {
                    convToDataTable(webCode, mst, sKey, eKey, mySysvar);
                }
            }
            return result;
        }
        private static string getWebCode(string url, ISysVarManager mySysvar)
        {
            try
            {
                using var httpClient = new HttpClient(new HttpClientHandler
                {
                    // Enable TLS 1.2 and TLS 1.3 (equivalent to SecurityProtocolType 4080 in old code)
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                    MaxConnectionsPerServer = 9999
                });

                string randomPrefix = RandomString(10, false);
                string randomUserAgent = $"{randomPrefix}{RandomNumber(1, 999999999)}";
                
                string userAgent = mySysvar.GetSysVar("M_USER_AGENT")?.ToString()?.Trim() ?? "Mozilla/5.0";
                
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                var response = httpClient.GetStringAsync(url).GetAwaiter().GetResult();
                return response;
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ProtocolError && 
                    webEx.Response is HttpWebResponse response && 
                    response.StatusCode == HttpStatusCode.Forbidden)
                {                    
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                // Log error if needed
                return string.Empty;
            }
        }
        private static DataTable convert_HTML_to_DataTable(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//table/thead/tr");
            HtmlNodeCollection htmlNodeCollection2 = htmlDocument.DocumentNode.SelectNodes("//table/tbody/tr");
            DataTable dataTable = new DataTable("taxInfo");
            dataTable.Columns.Add("key");
            dataTable.Columns.Add("value");
            if (htmlNodeCollection != null)
            {
                IEnumerable<string> enumerable = from th in htmlNodeCollection[0].Elements("th")
                                                 select th.InnerText.Trim();
                foreach (string item in enumerable)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["key"] = "ten_cong_ty";
                    dataRow["value"] = item;
                    if (!string.IsNullOrEmpty(dataRow["key"].ToString().Trim()) || !string.IsNullOrEmpty(dataRow["value"].ToString().Trim()))
                    {
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            if (htmlNodeCollection2 != null)
            {
                IEnumerable<string[]> enumerable2 = htmlNodeCollection2.Select((HtmlNode tr) => (from td in tr.Elements("td")
                                                                                                 select td.InnerText.Trim()).ToArray());
                IEnumerable<HtmlNode>[] array = htmlNodeCollection2.Select((HtmlNode tr) => tr.Elements("td")).ToArray();
                int num = 0;
                foreach (string[] item2 in enumerable2)
                {
                    DataRow dataRow2 = dataTable.NewRow();
                    dataRow2["key"] =item2[0].Trim().ToLower().Replace(" ", "_");
                    if (item2.Length > 1)
                    {
                        if (dataRow2["key"].ToString().Trim().Equals("nguoi_dai_dien"))
                        {
                            string[] array2 = array[num].Select((HtmlNode td) => td.InnerHtml).ToArray();
                            HtmlDocument htmlDocument2 = new HtmlDocument();
                            htmlDocument2.LoadHtml(array2[1]);
                            HtmlNodeCollection htmlNodeCollection3 = htmlDocument.DocumentNode.SelectNodes("//a");
                            dataRow2["value"] = htmlNodeCollection3[0].InnerText.Trim();
                        }
                        else
                        {
                            dataRow2["value"] = item2[1];
                        }
                    }
                    if (!string.IsNullOrEmpty(dataRow2["key"].ToString().Trim()) || !string.IsNullOrEmpty(dataRow2["value"].ToString().Trim()))
                    {
                        dataTable.Rows.Add(dataRow2);
                    }
                    num++;
                }
            }
            return dataTable;
        }
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                char value = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0)));
                stringBuilder.Append(value);
            }
            if (lowerCase)
            {
                return stringBuilder.ToString().ToLower();
            }
            return stringBuilder.ToString();
        }
        private static DataTable ConvertHtmlToDataTable(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var headerRows = htmlDoc.DocumentNode.SelectNodes("//table/thead/tr");
            var bodyRows = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");

            var dataTable = CreateEmptyTaxInfoTable();

            // Process header
            if (headerRows != null && headerRows.Count > 0)
            {
                var headers = headerRows[0].Elements("th").Select(th => th.InnerText.Trim());
                foreach (var header in headers.Where(h => !string.IsNullOrWhiteSpace(h)))
                {
                    dataTable.Rows.Add("ten_cong_ty", header);
                }
            }

            // Process body
            if (bodyRows != null)
            {
                foreach (var tr in bodyRows)
                {
                    var cells = tr.Elements("td").Select(td => td.InnerText.Trim()).ToArray();
                    
                    if (cells.Length == 0)
                        continue;

                    var key = RemoveDiacritics(cells[0].Trim()).ToLower().Replace(" ", "_");
                    var value = cells.Length > 1 ? cells[1] : string.Empty;

                    // Handle special case for representative
                    if (key.Equals("nguoi_dai_dien", StringComparison.OrdinalIgnoreCase) && cells.Length > 1)
                    {
                        var tdNodes = tr.Elements("td").ToArray();
                        if (tdNodes.Length > 1)
                        {
                            var innerHtml = tdNodes[1].InnerHtml;
                            var tempDoc = new HtmlDocument();
                            tempDoc.LoadHtml(innerHtml);
                            var linkNode = tempDoc.DocumentNode.SelectSingleNode("//a");
                            value = linkNode?.InnerText.Trim() ?? value;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(key) || !string.IsNullOrWhiteSpace(value))
                    {
                        dataTable.Rows.Add(key, value);
                    }
                }
            }

            return dataTable;
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static async Task<DataTable> GetMstInfoAsync(string mst, IDbManager dbManager)
        {
            if (string.IsNullOrWhiteSpace(mst))
                return null;

            try
            {
                mst = mst.Trim();

                // Call stored procedure to get MST info
                string sql = $"EXEC dbo.GetMstInfo '{mst}'";
                var ds = await dbManager.LoadDataset(sql);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ParseJsonToDataTable(ds.Tables[0].Rows[0]["json_data"]?.ToString());
                }
                
                return null;
            }
            catch (Exception ex)            {
               
                return null;
            }
        }

        private static DataTable ParseJsonToDataTable(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return null;

            var dataTable = CreateEmptyTaxInfoTable();

            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                if (jObject == null)
                    return null;

                var fieldMapping = new Dictionary<string, string>
                {
                    { "Name", "ten_cong_ty" },
                    { "Address", "dia_chi" },
                    { "Status", "tinh_trang" }
                };

                foreach (var property in jObject.Properties())
                {
                    var value = property.Value?.ToString();
                    if (string.IsNullOrEmpty(value) || 
                        "taxcode".Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var fieldName = fieldMapping.ContainsKey(property.Name) 
                        ? fieldMapping[property.Name] 
                        : property.Name;

                    if ("status".Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        dataTable.Rows.Add("status", value);
                        dataTable.Rows.Add(fieldName, TranslateStatus(value));
                    }
                    else
                    {
                        dataTable.Rows.Add(fieldName, value);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        public static async Task SaveTaxInfoAsync(IDbManager dbManager, params TaxInfo[] taxInfos)
        {
            if (taxInfos == null || taxInfos.Length == 0)
                return;

            try
            {
                var batch = taxInfos.Take(50).ToArray();
                var queries = batch.Select(BuildUpdateQuery).Where(q => !string.IsNullOrEmpty(q));
                var sqlQuery = string.Join("\n", queries);

                if (!string.IsNullOrWhiteSpace(sqlQuery))
                {
                    await dbManager.ExecuteSqlQueryAsync(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                // Log error
            }
        }

        private static string BuildUpdateQuery(TaxInfo taxInfo)
        {
            if (taxInfo == null || string.IsNullOrWhiteSpace(taxInfo.TaxCode))
                return string.Empty;
                
            var name = taxInfo.Name?.Replace("'", "''") ?? string.Empty;
            var status = TranslateStatus(taxInfo.Status);
            return $"EXEC dbo.UpdateOrInsertMst '{taxInfo.TaxCode}', N'{name}', N'{status}'";
        }

        private static DataTable CreateEmptyTaxInfoTable()
        {
            var table = new DataTable("taxInfo");
            table.Columns.Add("key", typeof(string));
            table.Columns.Add("value", typeof(string));
            return table;
        }

        private static string TranslateStatus(string statusCode)
        {
            return statusCode switch
            {
                "00" => "Đang hoạt động (đã được cấp GCN ĐKT)",
                "01" => "Ngừng hoạt động và đã đóng MST",
                "02" => "Đã chuyển cơ quan thuế quản lý",
                "03" => "Ngừng hoạt động nhưng chưa hoàn thành thủ tục đóng MST",
                "04" => "Đang hoạt động (được cấp thông báo MST)",
                "04-01" => "Vi phạm pháp luật đã chuyển cơ quan công an",
                "05" => "Tạm nghỉ kinh doanh có thời hạn",
                "06" => "Không hoạt động tại địa chỉ đã đăng ký",
                "00-01" => "Vi phạm đã chuyển cơ quan công an",
                _ => statusCode ?? string.Empty
            };
        }

        private static string GenerateRandomUserAgent()
        {
            var random = new Random();
            var prefix = GenerateRandomString(10, false);
            var suffix = random.Next(1, 999999999);
            return $"Mozilla/5.0 {prefix}{suffix}";
        }

        private static string GenerateRandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder(size);
            var random = new Random();
            
            for (int i = 0; i < size; i++)
            {
                char c = (char)random.Next('A', 'Z' + 1);
                builder.Append(c);
            }
            
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
