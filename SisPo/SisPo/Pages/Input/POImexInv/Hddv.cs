using Microsoft.AspNetCore.Hosting;
using SisData.Data;
using SisData.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.POImexInv
{
    public interface IHddvService
    {
        Task<bool> CanGetInvoiceAsync(DateTime ngay1, DateTime ngay2);
        Task<string> DownloadFileAsync(string authorizationToken, string dataTaxCode, string dataForm,
            string dataSeri, string dataNumber, string filename, string ttxly = "6");
        Task<string> GetInvDetailAsync(string authorizationToken, string dataTaxCode, string dataForm,
            string dataSeri, string dataNumber, string ttxly);
        Task<string> GetTaxInvoiceListOutAsync(string authorizationToken, DateTime dateFrom, DateTime dateTo,
            string dataTaxCode, string dataForm, string dataSeri, string dataNumber,
            string dataInvoiceStatus, string dataProcessStatus, string pageState);
        string Base64Decode(string base64EncodedData);
    }

    public class HddvService : IHddvService
    {
        private const string ApiBaseUrl = "https://hoadondientu.gdt.gov.vn:30000";
        private readonly IDbManager _dbManager;

        public HddvService(IDbManager dbManager)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }
        public async Task<bool> CanGetInvoiceAsync(DateTime ngay1, DateTime ngay2)
        {
            try
            {
                var query = "SELECT DISTINCT CONVERT(varchar(7), ngay_ct0, 120) as namthang FROM ph92";
                var table = _dbManager.GetTable(query);

                if (table == null || table.Rows.Count == 0)
                    return false;

                table.DefaultView.Sort = "namthang";

                string month1 = ngay1.ToString("yyyy-MM");
                string month2 = ngay2.ToString("yyyy-MM");

                if (table.DefaultView.FindRows(month1).Length == 0)
                    table.Rows.Add(month1);

                if (table.DefaultView.FindRows(month2).Length == 0)
                    table.Rows.Add(month2);

                return table.Rows.Count <= 3;
            }
            catch
            {
                return false;
            }
        }        

        public async Task<string> GetInvDetailAsync(
            string authorizationToken,
            string dataTaxCode,
            string dataForm,
            string dataSeri,
            string dataNumber,
            string ttxly)
        {
            string url = string.Empty;
            try
            {
                var taxCode = SanitizeInput(dataTaxCode);
                var form = SanitizeInput(dataForm);
                var number = SanitizeInput(dataNumber);
                var seri = SanitizeInput(dataSeri);

                var endpoint = ttxly == "8" ? "/sco-query/invoices/detail" : "/query/invoices/detail";
                url = $"{ApiBaseUrl.TrimEnd('/')}{endpoint}?nbmst={taxCode}&khhdon={seri}&shdon={number}&khmshdon={form}";

                // TODO: Implement async Web.GetStringAsync
                return await Task.Run(() => Web.GetString(url, authorizationToken));
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{url}";
            }
        }

        public string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return string.Empty;

            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string SanitizeInput(string value)
        {
            return value?.Replace("'", "").Replace(";", "") ?? string.Empty;
        }

        public async Task<string> GetTaxInvoiceListOutAsync(
            string authorizationToken,
            DateTime dateFrom,
            DateTime dateTo,
            string dataTaxCode,
            string dataForm,
            string dataSeri,
            string dataNumber,
            string dataInvoiceStatus,
            string dataProcessStatus,
            string pageState)
        {
            try
            {
                var searchParams = new List<string>();
                var invoiceType = "sold";
                var queryType = "query";

                if (!string.IsNullOrEmpty(dataTaxCode))
                    searchParams.Add($"nmmst=={SanitizeInput(dataTaxCode)}");

                if (!string.IsNullOrEmpty(dataInvoiceStatus))
                    searchParams.Add($"tthai=={SanitizeInput(dataInvoiceStatus)}");

                if (!string.IsNullOrEmpty(dataProcessStatus))
                {
                    searchParams.Add($"ttxly=={SanitizeInput(dataProcessStatus)}");
                    if (dataProcessStatus == "8")
                        queryType = "sco-query";
                }

                if (!string.IsNullOrEmpty(dataForm))
                    searchParams.Add($"khmshdon=={SanitizeInput(dataForm)}");

                if (!string.IsNullOrEmpty(dataNumber))
                    searchParams.Add($"shdon=={SanitizeInput(dataNumber)}");

                if (!string.IsNullOrEmpty(dataSeri))
                    searchParams.Add($"khhdon=={SanitizeInput(dataSeri)}");

                var searchQuery = searchParams.Count > 0 ? ";" + string.Join(";", searchParams) : string.Empty;
                var stateParam = !string.IsNullOrEmpty(pageState) ? $"&state={pageState}" : string.Empty;

                var dateFromStr = dateFrom.ToString("dd/MM/yyyy");
                var dateToStr = dateTo.ToString("dd/MM/yyyy");

                var url = $"{ApiBaseUrl.TrimEnd('/')}/{queryType}/invoices/{invoiceType}?" +
                         $"sort=tdlap:desc&size=50{stateParam}&" +
                         $"search=tdlap=ge={dateFromStr}T00:00:00;tdlap=le={dateToStr}T23:59:59{searchQuery}";

                // TODO: Implement async Web.GetStringAsync
                return await Task.Run(() => Web.GetString(url, authorizationToken));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> DownloadFileAsync(
            string authorizationToken,
            string dataTaxCode,
            string dataForm,
            string dataSeri,
            string dataNumber,
            string filename,
            string ttxly = "6")
        {
            string url = string.Empty;
            try
            {
                var taxCode = SanitizeInput(dataTaxCode);
                var form = SanitizeInput(dataForm);
                var number = SanitizeInput(dataNumber);
                var seri = SanitizeInput(dataSeri);

                var endpoint = ttxly == "8" ? "/sco-query/invoices/export-xml" : "/query/invoices/export-xml";
                url = $"{ApiBaseUrl.TrimEnd('/')}{endpoint}?nbmst={taxCode}&khhdon={seri}&shdon={number}&khmshdon={form}";                
                await Task.Run(() => Web.DownloadFile(url, authorizationToken, filename));
                return "OK";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{url}";
            }
        }
        private static async Task DownloadFileAsync(string url, string token, string filename)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36  Edg/91.0.864.59");

                if (!string.IsNullOrEmpty(token))
                {
                    var cleanToken = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cleanToken);
                }

                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                var directory = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                await File.WriteAllBytesAsync(filename, bytes).ConfigureAwait(false);
            }
        }       
    }

    // Legacy static class for backward compatibility
    [Obsolete("Use IHddvService instead")]
    public static class Hddv
    {
        private static IHddvService _service;
        private const string ApiBaseUrl = "https://hoadondientu.gdt.gov.vn:30000";
        public static void Initialize(IHddvService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public static bool CanGetInvoice(DateTime ngay1, DateTime ngay2, DbManager myDb)
            => _service?.CanGetInvoiceAsync(ngay1, ngay2).GetAwaiter().GetResult() ?? false;
        public static string DownloadFile(string authorizationToken, string dataTaxCode, string dataForm,
            string dataSeri, string dataNumber, string filename, string ttxly)
            => _service?.DownloadFileAsync(authorizationToken, dataTaxCode, dataForm, dataSeri, dataNumber, filename, ttxly).GetAwaiter().GetResult() ?? "Service not initialized";

        public static string GetInvDetail(string authorizationToken, string dataTaxCode, string dataForm,
            string dataSeri, string dataNumber, string ttxly)
            => _service?.GetInvDetailAsync(authorizationToken, dataTaxCode, dataForm, dataSeri, dataNumber, ttxly).GetAwaiter().GetResult() ?? "Service not initialized";
       
        private static async Task<string> GetInvDetailHttpAsync(string url, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/91.0.864.59");
                
                if (!string.IsNullOrEmpty(token))
                {
                    var cleanToken = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cleanToken);
                }

                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public static string Base64Decode(string base64EncodedData)
            => _service?.Base64Decode(base64EncodedData) ?? string.Empty;

        public static string GetTaxInvoiceListOut(string authorizationToken, DateTime dateFrom, DateTime dateTo,
            string dataTaxCode, string dataForm, string dataSeri, string dataNumber,
            string dataInvoiceStatus, string dataProcessStatus, string pageState)
            => _service?.GetTaxInvoiceListOutAsync(authorizationToken, dateFrom, dateTo, dataTaxCode, dataForm,
                dataSeri, dataNumber, dataInvoiceStatus, dataProcessStatus, pageState).GetAwaiter().GetResult() ?? "Service not initialized";

        public static async Task<string> DownloadFileTCTAsync(
          string authorizationToken,
          string dataTaxCode,
          string dataForm,
          string dataSeri,
          string dataNumber,
          string filename,
          string ttxly)
        {
            string url = "";
            try
            {
                string str1 = Func.ReplaceVariable(dataTaxCode);
                string str2 = Func.ReplaceVariable(dataForm);
                string str3 = Func.ReplaceVariable(dataNumber);
                string str4 = Func.ReplaceVariable(dataSeri);
                if (ttxly == "8")
                    url = "https://hoadondientu.gdt.gov.vn:30000".TrimEnd('/') + string.Format("/sco-query/invoices/export-xml?nbmst={0}&khhdon={1}&shdon={2}&khmshdon={3}", str1, str4, str3, str2);
                else
                    url = "https://hoadondientu.gdt.gov.vn:30000".TrimEnd('/') + string.Format("/query/invoices/export-xml?nbmst={0}&khhdon={1}&shdon={2}&khmshdon={3}", str1, str4, str3, str2);
                await DownloadFileAsync(url, authorizationToken, filename).ConfigureAwait(false);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message + "\n" + url;
            }
        }
        public static decimal GetThue_suat(string val, IDbManager myDb)
        {
            string ma_thue = Mapping.GetMa_thue(val, myDb);
            object thue_suat = myDb.GetValue($"SELECT thue_suat FROM dmthue30 WHERE status = '1' AND ma_thue = '{ma_thue.Trim()}'");
            return SisConverts.ToDec(thue_suat);
        }
        public static async Task<string> DownloadFileAsync(
            string authorizationToken,
            string dataTaxCode,
            string dataForm,
            string dataSeri,
            string dataNumber,
            string filename,
            string ttxly = "6")
        {
            string url = string.Empty;
            try
            {
                var taxCode = SanitizeInput(dataTaxCode);
                var form = SanitizeInput(dataForm);
                var number = SanitizeInput(dataNumber);
                var seri = SanitizeInput(dataSeri);

                var endpoint = ttxly == "8" ? "/sco-query/invoices/export-xml" : "/query/invoices/export-xml";
                url = $"{ApiBaseUrl.TrimEnd('/')}{endpoint}?nbmst={taxCode}&khhdon={seri}&shdon={number}&khmshdon={form}";
                await Task.Run(() => Web.DownloadFile(url, authorizationToken, filename));
                return "OK";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{url}";
            }
        }
        private static async Task DownloadFileAsync(string url, string token, string filename)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36  Edg/91.0.864.59");

                if (!string.IsNullOrEmpty(token))
                {
                    var cleanToken = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cleanToken);
                }

                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                var directory = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                await File.WriteAllBytesAsync(filename, bytes).ConfigureAwait(false);
            }
        }
        private static string SanitizeInput(string value)
        {
            return value?.Replace("'", "").Replace(";", "") ?? string.Empty;
        }
        public static string DownloadFile(string id_hd, string type, IWebHostEnvironment host, IDbManager myDb)
        {
            string filename = "";
            try
            {
                DataSet dataSet = myDb.LoadDataSet("EXEC dbo.Hddv_GetFile @Id_hd, @Type", new Dictionary<string, object>
                {
                    ["@Id_hd"] = (object)id_hd,
                    ["@Type"] = (object)type
                });
                if (dataSet.Tables[0].Rows.Count == 0)
                    return filename;
                DataRow row = dataSet.Tables[0].Rows[0];
                filename = Path.Combine(host.ContentRootPath, "wwwroot\\Hddv", row["file_name"].ToString());
                string hash = row["hash_code"].ToString();
                Hddv.SynchroFile((int)row["id"], filename, hash, myDb);
            }
            catch (Exception ex)
            {
            }
            return filename;
        }
        private static void SynchroFile(int id, string filename, string hash, IDbManager myDb)
        {
            try
            {
                if (File.Exists(filename) && hash == Ult.GetInvHash(filename))
                    return;
                string directoryName = Path.GetDirectoryName(filename);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                object obj = myDb.GetValue($"SELECT file_contain FROM ct92files WHERE id = {id}");
                if (obj == null || obj == DBNull.Value)
                    return;
                File.WriteAllBytes(filename, (byte[])obj);
            }
            catch (Exception ex)
            {
            }
        }
        [Obsolete("Use async version instead")]
        public static string DownloadFileTCT(
          string authorizationToken,
          string dataTaxCode,
          string dataForm,
          string dataSeri,
          string dataNumber,
          string filename,
          string ttxly) => DownloadFileTCTAsync(authorizationToken, dataTaxCode, dataForm, dataSeri, dataNumber, filename, ttxly).GetAwaiter().GetResult();

        public static async Task<string> GetInvDetailAsync(string authorizationToken, string dataTaxCode, string dataForm,
            string dataSeri, string dataNumber, string ttxly)
        {
            if (_service != null)
                return await _service.GetInvDetailAsync(authorizationToken, dataTaxCode, dataForm, dataSeri, dataNumber, ttxly);
            return await GetInvDetailHttpAsync(BuildInvDetailUrl(dataTaxCode, dataForm, dataSeri, dataNumber, ttxly), authorizationToken);
        }

        private static string BuildInvDetailUrl(string dataTaxCode, string dataForm, string dataSeri, string dataNumber, string ttxly)
        {
            string url = "https://hoadondientu.gdt.gov.vn:30000";
            string str1 = Func.ReplaceVariable(dataTaxCode);
            string str2 = Func.ReplaceVariable(dataForm);
            string str3 = Func.ReplaceVariable(dataNumber);
            string str4 = Func.ReplaceVariable(dataSeri);
            string endpoint = ttxly == "8" ? "/sco-query/invoices/detail" : "/query/invoices/detail";
            return url.TrimEnd('/') + string.Format("{0}?nbmst={1}&khhdon={2}&shdon={3}&khmshdon={4}", endpoint, str1, str4, str3, str2);
        }
    }
}




