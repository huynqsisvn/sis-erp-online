using Microsoft.AspNetCore.Hosting;
using SisData.Data;
using SisData.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace SisPo.Pages.Input.POImexInv
{
    internal class Func
    {
        private static string DateFormat = "yyyy-MM-dd";
        public static readonly CultureInfo DateCulture = new CultureInfo("vi-VN");
        public static readonly CultureInfo NumCulture = new CultureInfo("vi-VN");

        public static async Task UploadInv(string token, DataSet ds,IDbManager myDb, IOptionsManager myOptions, IWebHostEnvironment env, Statemanagerment myStateMN, ISysVarManager mySysvar)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;
            SqlConnection connection = myDb.GetConnection();
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                foreach (DataTable table in (InternalDataCollectionBase)ds.Tables)
                    Func.UploadTempData(table, connection);

                using (SqlCommand cmd = new SqlCommand("EXEC POImexInv_UploadInv", connection))
                {
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                }

                DataSet dataSet = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_hd,ttxly,mst_ban, kh_mau_hd0, ngay_ct0, so_seri0, so_ct0, tinh_trang_hd,ten_dv_ban, dia_chi_ban, noi_dung_loi FROM #ph92", connection))
                {
                    adapter.Fill(dataSet);
                }

                int count1 = ds.Tables[0].Rows.Count;
                int count2 = dataSet.Tables[0].Rows.Count;
                int sl_da = count1 - count2;
                
                myStateMN?.SetNewThongbao($"Đang tải {count2} hóa đơn...");
                
                List<DataRow> dataRowList = await Func.UploadFilesAsync(token, dataSet.Tables[0], connection, myDb, env);
                
                myStateMN?.SetNewThongbao("Đang kiểm tra mã số thuế...");
                Func.Check_Mst_ban(dataSet.Tables[0],myDb, myOptions, mySysvar);
                
                myStateMN?.SetNewThongbao("Đang kiểm tra doanh nghiệp rủi ro...");
                Func.CheckDoanhNghiepRuiRoThue(dataSet, myDb);
                
                int num = count2 - dataRowList.Count;

                myStateMN?.SetNewThongbao("Đang xử lý chi tiết hóa đơn...");
                int processed = 0;
                foreach(DataRow row in dataSet.Tables[0].Rows)
                {
                    processed++;
                    try
                    {
                        myStateMN?.SetNewThongbao($"Đang xử lý hóa đơn {processed}/{dataSet.Tables[0].Rows.Count}...");
                        Func.CheckCt(row["id_hd"].ToString(), myDb, env);
                        Func.CreateBkhddvtct(row["id_hd"].ToString(), myDb);
                    }
                    catch (Exception ex)
                    {
                        myStateMN?.SetNewThongbao($"Lỗi xử lý hóa đơn {row["so_ct0"]}: {ex.Message}");
                    }
                }

                myStateMN?.SetNewThongbao(string.Format("Hoàn thành! Tổng: {0:#,0}, đã tồn tại: {1:#,0}, đã xử lý: {2:#,0}, lỗi: {3:#,0}", count1, sl_da, count2, num));
            }
            catch (Exception ex)
            {
                myStateMN?.SetNewThongbao($"Lỗi xử lý: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public static void Check_Mst_ban(DataTable tbl, IDbManager myDb, IOptionsManager myOptions,ISysVarManager mySysvar)
        {
            try
            {
                string[] array1 = tbl.Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>)(x => x["mst_ban"].ToString().Trim())).Distinct<string>().ToArray<string>();
                DataView dataView = new DataView(tbl);
                dataView.Sort = "mst_ban";
                foreach (string mst in array1)
                {
                    DataRowView[] rows = dataView.FindRows((object)mst);
                    if (rows.Length != 0)
                    {
                        DataTable mstInfo = Util.GetInfoMST(mst, myDb, mySysvar);
                        if (mstInfo == null || mstInfo.Rows.Count == 0)
                        {
                            foreach (DataRowView dataRowView in rows)
                            {
                                dataRowView["tinh_trang_hd"] = "2";
                                dataRowView["noi_dung_loi"] = "Mã số thuế không tồn tại";
                            }
                        }
                        else
                        {
                            string text1 = "";
                            string text2 = "";
                            string str1 = "";
                            string str2 = "";
                            foreach (DataRow row in (InternalDataCollectionBase)mstInfo.Rows)
                            {
                                string str3 = row["key"].ToString().Trim();
                                if (!(str3 == "ten_cong_ty"))
                                {
                                    if (!(str3 == "dia_chi"))
                                    {
                                        if (!(str3 == "status"))
                                        {
                                            if (str3 == "tinh_trang")
                                                str2 = row["value"].ToString().Trim();
                                        }
                                        else
                                            str1 = row["value"].ToString().Trim();
                                    }
                                    else
                                        text2 = row["value"].ToString().Trim();
                                }
                                else
                                    text1 = row["value"].ToString().Trim();
                            }
                            string str4 = "việt nam";
                            string str5 = Func.CleanName(text1);
                            string str6 = Func.CleanAddress(text2);
                            foreach (DataRowView dataRowView in rows)
                            {
                                string str3 = Func.CleanName(dataRowView["ten_dv_mua"].ToString());
                                List<string> stringList = new List<string>();
                                if (!str5.Equals(str3, StringComparison.OrdinalIgnoreCase))
                                    stringList.Add("Sai tên đơn vị người mua");
                                bool IsChkAdrr = myOptions.GetOptionsValue("M_HDDV_CHK_ADRR").ToString().Trim() == "1";
                                if (IsChkAdrr)
                                {
                                    string str7 = Func.CleanAddress(dataRowView["dia_chi_mua"].ToString());
                                    if (str7.EndsWith(str4))
                                        str7 = str7.Substring(0, str7.Length - str4.Length).TrimEnd();
                                    if (!str6.Equals(str7, StringComparison.OrdinalIgnoreCase))
                                        stringList.Add("Sai địa chỉ người mua");
                                }
                                if (str1 != "00")
                                    stringList.Add(str2);
                                if (stringList.Count != 0)
                                {
                                    if (dataRowView["noi_dung_loi"].ToString().Trim() != "")
                                        stringList.Add(dataRowView["noi_dung_loi"].ToString().Trim());
                                    for (int index = 1; index < stringList.Count; ++index)
                                        stringList[index] = stringList[index].ToString().Trim().ToLower();
                                    dataRowView["tinh_trang_hd"] = "2";
                                    dataRowView["noi_dung_loi"] = string.Join(", ", stringList.ToArray());
                                }
                            }
                        }
                    }
                }
                DataRow[] array2 = tbl.Rows.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>)(x => x["tinh_trang_hd"].ToString() == "2")).ToArray<DataRow>();
                if (array2.Length == 0)
                    return;
                string strsql = string.Join("\n", ((IEnumerable<DataRow>)array2).Select<DataRow, string>((System.Func<DataRow, string>)(x => string.Format("UPDATE ph92 SET tinh_trang_hd = '{1}', noi_dung_loi = N'{2}' WHERE id_hd = '{0}';", x["id_hd"], x["tinh_trang_hd"], (object)x["noi_dung_loi"].ToString().Replace("'", "''")))).ToArray<string>());
                myDb.ExecuteSqlQueryAsync(strsql);
            }
            catch (Exception ex)
            {
            }
        }
            
        internal static void CheckDoanhNghiepRuiRoThue(DataSet ds, IDbManager myDb)
        {
            try
            {
                string errorText = "Doanh nghiệp rủi ro theo công văn số 1798/TCT-TTKT ngày 16/5/2023.";
                HashSet<string> stringSet = new HashSet<string>();
                DataTable dataTable = ds.Tables[0].Copy();               
                
                DataTable table = myDb.GetTable("select rtrim(ltrim(ma_so_thue)) as ma_so_thue from dmdnruiro where status = '1'");
                if (table.Rows.Count == 0)
                    return;
                string CommandText = "";
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    string mstNguoiBan = row["mst_ban"].ToString().Trim();
                    string id_hd = row["id_hd"].ToString().Trim();
                    if (stringSet.Contains(mstNguoiBan))
                        CommandText += Func.GetUpdateQuerynoi_dung_loi(errorText, id_hd);
                    else if (table.Rows.Cast<DataRow>().Any<DataRow>((System.Func<DataRow, bool>)(x => x["ma_so_thue"].ToString() == mstNguoiBan)))
                    {
                        stringSet.Add(mstNguoiBan);
                        CommandText += Func.GetUpdateQuerynoi_dung_loi(errorText, id_hd);
                    }
                }
                if (CommandText == "")
                    return;
                myDb.ExecuteSqlQueryAsync(CommandText);
            }
            catch (Exception ex)
            {
            }
        }

        private static string GetUpdateQuerynoi_dung_loi(string errorText, string id_hd)
        {
            return "UPDATE ph92" + " SET noi_dung_loi = case when isnull(noi_dung_loi, '') = '' then N'" + errorText + "' else noi_dung_loi + N'; " + errorText + "' end" + " WHERE id_hd = '" + id_hd + "';\n";
        }

        private static string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            text = text.Trim().Replace("  ", " ");
            text = text.Trim().Replace(" ", "");
            text = text.Replace(".", "").Replace(",", "").Replace("-", "").Replace("_", "");
            text = text.Trim().Replace("/", "");
            return text.ToLower();
        }

        private static string CleanName(string text)
        {
            return Func.CleanText(text);
        }

        private static string CleanAddress(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            text = text.ToLower().Trim().Replace("thành phố hồ chí minh", "tphcm");
            text = text.Replace("phường ", "p");
            text = text.Replace("quận ", "q");
            text = text.Replace("thị xã ", "tx");
            text = text.Replace("thị trấn ", "tt");
            text = text.ToLower().Trim().Replace("thành phố", "tp");
            text = text.ToLower().Trim().Replace("hồ chí minh", "hcm");
            text = text.ToLower().Trim().Replace("việt nam", "");
            return Func.CleanText(text);
        }

        public static void UploadTempData(DataTable tbl, SqlConnection connection)
        {
            string tableName = tbl.TableName;
            using (SqlCommand sqlCommand = new SqlCommand(string.Format("Select {0} Into [#{1}] From [{1}] Where 1=0", Func.GetMergeFields(tbl, true, connection), tableName), connection))
            {
                if (sqlCommand.Connection.State != ConnectionState.Open) { sqlCommand.Connection.Open(); }
                sqlCommand.CommandTimeout = 900000;
                sqlCommand.ExecuteNonQuery();
            }
            if (tbl.Rows.Count == 0)
                return;
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
            {
                sqlBulkCopy.BatchSize = 100;
                sqlBulkCopy.BulkCopyTimeout = 9000000;
                string str = string.Format("[#{0}]", tableName);
                sqlBulkCopy.DestinationTableName = str;
                sqlBulkCopy.WriteToServer(tbl);
            }
        }

        public static void UploadData(DataTable tbl, SqlConnection connection)
        {
            if (tbl == null)
                return;
            string tableName = tbl.TableName;
            if (tbl.Rows.Count == 0)
                return;
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
            {
                sqlBulkCopy.BatchSize = 100;
                sqlBulkCopy.BulkCopyTimeout = 9000000;
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.WriteToServer(tbl);
            }
        }

        public static async Task<List<DataRow>> UploadFilesAsync(string token,DataTable tbl,SqlConnection connection, IDbManager myDb, IWebHostEnvironment myWebhost)
        {
            DataTable tbl1 = JsonData.InitDataDetail();
            List<DataRow> dataRowList = new List<DataRow>();
            List<string> errorLogs = new List<string>();
            List<string> tempFolders = new List<string>();
            int i = 0;
            foreach (DataRow row in (InternalDataCollectionBase)tbl.Rows)
            {
                ++i;
                DateTime dateTime = (DateTime)row["ngay_ct0"];
                string ngay_ct0 = dateTime.ToString("yyyyMMdd");
                string str1 = row["id_hd"].ToString().Trim();
                string str2 = Path.Combine(myWebhost.WebRootPath, "upload_tmp", str1);
                if (!Directory.Exists(str2))
                    Directory.CreateDirectory(str2);
                string str3 = row["mst_ban"].ToString().Trim();
                string str4 = row["ten_dv_ban"].ToString().Trim();
                string dataForm = row["kh_mau_hd0"].ToString().Trim();
                string str5 = row["so_seri0"].ToString().Trim();
                string str6 = row["so_ct0"].ToString().Trim();
                string ttxly = row["ttxly"].ToString().Trim();
                string str7 = Path.Combine(str2, string.Format("{0}.zip", str1));
                
                string str8 = await Hddv.DownloadFileTCTAsync(token, str3, dataForm, str5, str6, str7, ttxly).ConfigureAwait(false);
                if (str8 == "OK")
                {
                    str8 = Func.UploadFile(str1, str3, ngay_ct0, str5, str6, str7, connection, myWebhost);
                    if (str8 == "OK")
                        dataRowList.Add(row);
                }
                if (str8 != "OK")
                {
                    string invDetail = await Hddv.GetInvDetailAsync(token, str3, dataForm, str5, str6, ttxly);
                    if (!string.IsNullOrEmpty(invDetail) && invDetail.StartsWith("{") && invDetail.EndsWith("}"))
                    {
                        dataRowList.Add(row);
                        JsonData.ToDetailTable(invDetail, tbl1);
                    }
                    else
                    {
                        errorLogs.Add($"{str1}: {str8}\n{str7}");
                    }
                }
               
                if (Directory.Exists(str2))
                    tempFolders.Add(str2);
            }
            
            if (tempFolders.Count > 0)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    foreach (string folder in tempFolders)
                    {
                        try
                        {
                            int retries = 3;
                            while (retries > 0)
                            {
                                try
                                {
                                    if (Directory.Exists(folder))
                                        Directory.Delete(folder, true);
                                    break;
                                }
                                catch
                                {
                                    retries--;
                                    if (retries > 0)
                                        System.Threading.Thread.Sleep(500);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                });
            }
            
            if (errorLogs.Count > 0)
            {
                try
                {
                    string logPath = Path.Combine(myWebhost.ContentRootPath, "wwwroot", "logs");
                    if (!Directory.Exists(logPath))
                        Directory.CreateDirectory(logPath);
                    await File.WriteAllTextAsync(Path.Combine(logPath, "UploadFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log"), 
                        string.Join("\n---\n", errorLogs));
                }
                catch (Exception ex)
                {
                }
            }
            
            Func.UploadData(Func.LoadCtData(tbl1, myDb, myWebhost), connection);
            return dataRowList;
        }

        internal static string UploadFile(
          string id_hd,
          string mst_ban,
          string ngay_ct0,
          string so_seri0,
          string so_ct0,
          string file_zip,
          SqlConnection connection, IWebHostEnvironment myWebhost)
        {
            if (!File.Exists(file_zip))
                return string.Format("File '{0}' not found.", (object)file_zip);
            Func.Extract(file_zip, myWebhost);
            string directoryName = Path.GetDirectoryName(file_zip);
            string str1 = Path.Combine(directoryName, "invoice.xml");
            if (!File.Exists(str1))
                return string.Format("File '{0}' not found.\n", (object)str1);
            try
            {
                DataSet dataSet = Ult.ReadXml(str1);
                if (dataSet.Tables.Contains("TTChung"))
                {
                    DataTable table = dataSet.Tables["TTChung"];
                    if (table.Rows.Count > 0 && table.Columns.Contains("NLap"))
                        ngay_ct0 = table.Rows[0]["NLap"].ToString().Replace("-", "");
                }
                string str2 = string.Format("{0}_{1}_{2}_{3}.xml", mst_ban, ngay_ct0, so_seri0, so_ct0);
                string md5HashFromFile1 = SisConverts.GetMD5HashFromFile(str1);
                SqlCommand sqlCommand1 = new SqlCommand("EXEC POImexInv_UploadFile @Id_hd, @File_name, @File_type, @Hash_code, @File_contain", connection);
                sqlCommand1.Parameters.Add("@Id_hd", SqlDbType.VarChar).Value = id_hd;
                sqlCommand1.Parameters.Add("@File_name", SqlDbType.VarChar).Value = str2.Replace('/', '_');
                sqlCommand1.Parameters.Add("@File_type", SqlDbType.VarChar).Value = "1";
                sqlCommand1.Parameters.Add("@Hash_code", SqlDbType.VarChar).Value = md5HashFromFile1;
                sqlCommand1.Parameters.Add("@File_contain", SqlDbType.Image).Value = File.ReadAllBytes(str1);
                sqlCommand1.ExecuteNonQuery();
                string str3 = Path.Combine(directoryName, "invoice.html");
                if (!File.Exists(str3))
                    return string.Format("File '{0}' not found.", str3);
                string str4 = string.Format("{0}_{1}_{2}_{3}.html", mst_ban, ngay_ct0, so_seri0, so_ct0);
                string md5HashFromFile2 = SisConverts.GetMD5HashFromFile(str3);
                SqlCommand sqlCommand2 = new SqlCommand("EXEC POImexInv_UploadFile @Id_hd, @File_name, @File_type, @Hash_code, @File_contain", connection);
                sqlCommand2.Parameters.Add("@Id_hd", SqlDbType.VarChar).Value = id_hd;
                sqlCommand2.Parameters.Add("@File_name", SqlDbType.VarChar).Value = str4.Replace('/', '_');
                sqlCommand2.Parameters.Add("@File_type", SqlDbType.VarChar).Value = "2";
                sqlCommand2.Parameters.Add("@Hash_code", SqlDbType.VarChar).Value = md5HashFromFile2;
                sqlCommand2.Parameters.Add("@File_contain", SqlDbType.Image).Value = File.ReadAllBytes(str3);
                sqlCommand2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            return "OK";
        }

        public static void Extract(string inFile, IWebHostEnvironment myHost)
        {
            if (!File.Exists(inFile))
                return;
            string directoryName = Path.GetDirectoryName(inFile);
            string arguments = string.Format("x \"{0}\" -y -r -o\"{1}\"", inFile, directoryName);
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(Path.Combine(myHost.ContentRootPath, "wwwroot\\exe", "7Z.exe"), arguments)
            };
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            
            // Timeout sau 30 giây nếu giải nén quá lâu
            if (!process.WaitForExit(30000))
            {
                process.Kill();
            }
        }

        public static string GetMergeFields(DataTable tbl, bool isid, SqlConnection connection)
        {
            string[] tblstruct = Func.ReadStruct(tbl.TableName, isid, connection);
            string str = "";
            foreach (DataColumn column in tbl.Columns.Cast<DataColumn>().Where<DataColumn>((System.Func<DataColumn, bool>)(x => !((IEnumerable<string>)tblstruct).Contains<string>(x.ColumnName))).ToArray<DataColumn>())
                tbl.Columns.Remove(column);
            foreach (DataColumn column in (InternalDataCollectionBase)tbl.Columns)
                str = !string.IsNullOrEmpty(str.Trim()) ? str + "," + column.ColumnName : str + column.ColumnName;
            return str;
        }

        public static string[] ReadStruct(string name, bool isid, SqlConnection connection)
        {
            string cmdText = "SELECT column_name as name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + name + "'";
            if (!isid)
                cmdText = cmdText + "\nEXCEPT\nSELECT a.name FROM sys.identity_columns a JOIN sys.objects b ON a.object_id = b.object_id WHERE b.name = '" + name + "'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(cmdText, connection));
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            if (dataSet.Tables.Count == 0)
                return new string[0];
            return dataSet.Tables[0].Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>)(x => x[nameof(name)].ToString().Trim())).ToArray<string>();
        }

        internal static SqlConnection GetConnection(IDbManager myDb)
        {            
            return myDb.GetConnection();
        }

        private static DataTable CheckColumnSize(DataTable tbl, IDbManager myDb)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("col");
            dataTable.Columns.Add("size");
            dataTable.Columns.Add("data_len");
            dataTable.Columns.Add("value");
            if (tbl.Rows.Count == 0)
                return dataTable;
            string cmd = "SELECT column_name as column_name, \r\n                                            CHARACTER_MAXIMUM_LENGTH as max_length\r\n                                            from INFORMATION_SCHEMA.COLUMNS\r\n                                            where table_name=@table_name";
            DataTable table = myDb.LoadDataSet(cmd,new Dictionary<string, object> { { "@table_name", tbl.TableName } }).Tables[0];
            table.DefaultView.Sort = "column_name";
            foreach (DataColumn column in (InternalDataCollectionBase)tbl.Columns)
            {
                DataColumn col = column;
                if (!(col.DataType != typeof(string)))
                {
                    DataRowView[] rows = table.DefaultView.FindRows((object)col.ColumnName);
                    if (rows.Length != 0)
                    {
                        int num = (int)rows[0]["max_length"];
                        int data_len = tbl.Rows.Cast<DataRow>().Max<DataRow>((System.Func<DataRow, int>)(x => x[col.ColumnName].ToString().Length));
                        if (data_len > num)
                        {
                            string str = tbl.Rows.Cast<DataRow>().FirstOrDefault<DataRow>((System.Func<DataRow, bool>)(x => x[col.ColumnName].ToString().Length == data_len))[col.ColumnName].ToString();
                            dataTable.Rows.Add(col.ColumnName, num, data_len, str);
                        }
                    }
                }
            }
            return dataTable;
        }

        private static DataSet GetPhDataTemplate(IDbManager myDb)
        {
            DataSet dataSet = myDb.LoadDataSet("SELECT * FROM ph92 WHERE 1 =0;SELECT * FROM ct92gt WHERE 1 =0;");
            dataSet.Tables[0].TableName = "ph92";
            dataSet.Tables[1].TableName = "ct92gt";
            return dataSet;
        }

        private static DataSet GetCtDataTemplate(SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM ct92 WHERE 1 =0;SELECT * FROM ct92gt WHERE 1 =0", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);            
            dataSet.Tables[0].TableName = "ct92";
            dataSet.Tables[1].TableName = "ct92gt";
            return dataSet;
        }

        public static DataSet LoadPhData(string jsondata,string Ma_ct, IOptionsManager myOptions , IDbManager myDb, Statemanagerment myStateMN, IWebHostEnvironment host)
        {
            DataSet phDataTemplate = Func.GetPhDataTemplate(myDb);
            DataSet dataSet = JsonData.ToDataSet(jsondata);
            if (dataSet.Tables.Count < 1)
                return phDataTemplate;
            DataTable table1 = dataSet.Tables[0];
            DataTable table2 = phDataTemplate.Tables[0];
            string tableName1 = table2.TableName;
            if (tableName1 != "ph92")
                return phDataTemplate;
            Dictionary<string, string> mapping1 = Func.GetMapping("FromJson", string.Format("{0}.MAP", tableName1), host);
            if (mapping1.Count == 0)
                return phDataTemplate;
            DateTime date = DateTime.Now.Date;
            string time = DateTime.Now.ToString("HH:mm:sss");
            int mUserId = myStateMN.GetCurUser().User_id;
            string str = myStateMN.GetDvcs().Ma_dvcs.Trim();
            foreach (DataRow row1 in (InternalDataCollectionBase)table1.Rows)
            {
                DataRow row2 = table2.NewRow();
                row2["ma_dvcs"] = str;
                row2["tinh_trang_hd"] = "1";
                row2["date"] = row2["date0"] = date;
                row2["time"] = row2["time0"] = time;
                row2["user_id"] = row2["user_id0"] = mUserId;
                row2["ma_ct"] = Ma_ct;
                row2["ws_id"] = myOptions.GetOptionsValue("M_WS_ID").Trim();
                foreach (KeyValuePair<string, string> keyValuePair in mapping1)
                {
                    if (table2.Columns.Contains(keyValuePair.Key) && table1.Columns.Contains(keyValuePair.Value))
                        row2[keyValuePair.Key] = Func.Convert(keyValuePair.Key, table2.Columns[keyValuePair.Key].DataType, row1[keyValuePair.Value], myDb);
                }
                table2.Rows.Add(row2);
            }
            if (dataSet.Tables.Count < 2)
                return phDataTemplate;
            DataTable table3 = dataSet.Tables[1];
            DataTable table4 = phDataTemplate.Tables[1];
            string tableName2 = table4.TableName;
            if (tableName2 != "ct92gt")
                return phDataTemplate;
            Dictionary<string, string> mapping2 = Func.GetMapping("FromJson", string.Format("{0}.MAP", (object)tableName2), host);
            if (mapping2.Count == 0)
                return phDataTemplate;
            foreach (DataRow row1 in (InternalDataCollectionBase)table3.Rows)
            {
                DataRow row2 = table4.NewRow();
                foreach (KeyValuePair<string, string> keyValuePair in mapping2)
                {
                    if (table4.Columns.Contains(keyValuePair.Key) && table3.Columns.Contains(keyValuePair.Value))
                        row2[keyValuePair.Key] = Func.Convert(keyValuePair.Key, table4.Columns[keyValuePair.Key].DataType, row1[keyValuePair.Value], myDb);
                }
                table4.Rows.Add(row2);
            }
            return phDataTemplate;
        }

        public static DataTable LoadCtData(DataTable tbl, IDbManager myDb, IWebHostEnvironment host)
        {
            DataSet ctDataTemplate = Func.GetCtDataTemplate(myDb.GetConnection());
            if (ctDataTemplate.Tables.Count == 0)
                return (DataTable)null;
            DataTable table = ctDataTemplate.Tables[0];
            string tableName = table.TableName;
            if (tableName != "ct92")
                return (DataTable)null;
            Dictionary<string, string> mapping = Func.GetMapping("FromJson", string.Format("{0}.MAP", tableName), host);
            if (mapping.Count == 0)
                return (DataTable)null;
            foreach (DataRow row1 in (InternalDataCollectionBase)tbl.Rows)
            {
                DataRow row2 = table.NewRow();
                foreach (KeyValuePair<string, string> keyValuePair in mapping)
                {
                    if (table.Columns.Contains(keyValuePair.Key) && tbl.Columns.Contains(keyValuePair.Value))
                    {
                        row2[keyValuePair.Key] = Func.ConvertFromDetail(keyValuePair.Key, table.Columns[keyValuePair.Key].DataType, row1[keyValuePair.Value]);
                        string key = keyValuePair.Key;
                        if (!(key == "thue_suat") && !(key == "thue_suati") && !(key == "tl_ck"))
                        {
                            if (key == "ma_thue_i")
                            {
                                string str = row2[keyValuePair.Key].ToString().Trim().Trim('%');
                                if (str.Length < 2)
                                    str = str.PadLeft(2, '0');
                                row2[keyValuePair.Key] = str;
                            }
                        }
                        else
                            row2[keyValuePair.Key] = (object)(SisConverts.ToDec(row2[keyValuePair.Key]) * new Decimal(100));
                    }
                }
                table.Rows.Add(row2);
            }
            return table;
        }

        public static DataSet LoadCtData(string id_hd, DataSet invds, SqlConnection connection, IDbManager myDb, IWebHostEnvironment host)
        {
            DataSet ctDataTemplate = Func.GetCtDataTemplate(connection);
            if (invds.Tables.Count < 2)
                return ctDataTemplate;
            DataTable dataTable = (DataTable)null;
            foreach (DataTable table in (InternalDataCollectionBase)ctDataTemplate.Tables)
            {
                string tableName = table.TableName;
                string str = tableName;
                if (str.ToLower() == "ct92")
                {
                    dataTable = invds.Tables["HHDVu"];
                }
                Dictionary<string, string> mapping = Func.GetMapping("FromXML", string.Format("{0}.MAP", tableName), host);
                if (mapping.Count != 0)
                {
                    foreach (DataRow row1 in (InternalDataCollectionBase)dataTable.Rows)
                    {
                        DataRow row2 = table.NewRow();
                        row2[nameof(id_hd)] = (object)id_hd;
                        foreach (KeyValuePair<string, string> keyValuePair in mapping)
                        {
                            if (table.Columns.Contains(keyValuePair.Key) && dataTable.Columns.Contains(keyValuePair.Value))
                                row2[keyValuePair.Key] = Func.Convert(keyValuePair.Key, table.Columns[keyValuePair.Key].DataType, row1[keyValuePair.Value], myDb);
                        }
                        table.Rows.Add(row2);
                    }
                }
            }
            if (!invds.Tables.Contains("LPhi"))
                return ctDataTemplate;
            DataTable table1 = invds.Tables["LPhi"];
            if (!table1.Columns.Contains("TLPhi") || !table1.Columns.Contains("TPhi"))
                return ctDataTemplate;
            DataTable table2 = ctDataTemplate.Tables["ct92"];
            int num = SisConverts.TryToInt(table2.Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>)(x => x["stt"].ToString())).Max<string>()) + 1;
            foreach (DataRow row1 in (InternalDataCollectionBase)table1.Rows)
            {
                object obj = row1["TPhi"];
                if (obj != null && obj != DBNull.Value && !((string)obj == string.Empty))
                {
                    DataRow row2 = table2.NewRow();
                    row2[nameof(id_hd)] = id_hd;
                    row2["stt"] = num++;
                    row2["ten_vt_inv"] = row1["TLPhi"];
                    row2["tien0"] = Func.ConvertFromDetail((string)null, typeof(Decimal), (object)obj.ToString());
                    table2.Rows.Add(row2);
                }
            }
            return ctDataTemplate;
        }

        internal static Dictionary<string, string> GetMapping(string dir, string filename, IWebHostEnvironment host)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string path = Path.Combine(host.ContentRootPath, "wwwroot\\Template\\Imex\\hddv\\" , dir, filename);
            if (!File.Exists(path))
                return dictionary;
            try
            {
                foreach (string readAllLine in File.ReadAllLines(path))
                {
                    if (!readAllLine.StartsWith("#"))
                    {
                        string[] strArray = readAllLine.Split('=');
                        if (strArray.Length >= 2)
                        {
                            string key = strArray[0];
                            string str = strArray[1];
                            dictionary.Add(key, str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dictionary;
        }

        static Func()
        {
            Func.NumCulture.NumberFormat.NumberDecimalSeparator = ".";
            Func.NumCulture.NumberFormat.NumberGroupSeparator = ",";
            Func.DateCulture.DateTimeFormat.DateSeparator = "/";
        }
        internal static string GetMa_thue(string value, IDbManager myDb)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            value = value.Trim().Replace(" ", "").ToUpper();
            return Dmthue.GetMa_thueMap(value.Trim('%'), myDb);
        }
        private static object Convert(string col, Type type, object val, IDbManager myDb)
        {
            if (val == null)
                return DBNull.Value;

            if (col.StartsWith("ma_thue", StringComparison.OrdinalIgnoreCase))
            {
                string[] strArray = val.ToString().Split(':');
                string str = strArray[strArray.Length - 1];
                str = str.Trim().Replace(" ", "").ToUpper().Trim('%');
                switch (str)
                {
                    case "00.00":
                        str = "0";
                        break;
                    case "05.00":
                        str = "5";
                        break;
                    case "08.00":
                        str = "8";
                        break;
                    case "10.00":
                        str = "10";
                        break;
                }
                return GetMa_thue(str, myDb);
            }
            if (col.StartsWith("thue_suat"))
            {
                string[] strArray = val.ToString().Split(':');
                string str = strArray[strArray.Length - 1];
                if (!str.EndsWith("%"))
                    return Hddv.GetThue_suat(val.ToString(), myDb);
                val = str.Trim('%');
            }
            TypeCode typeCode = Type.GetTypeCode(type);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            try
            {
                string text1 = val.ToString();
                switch (typeCode)
                {
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        if (string.IsNullOrEmpty(text1))
                            return 0;
                        string text2 = text1.Replace(",", ".");
                        if (!text2.EndsWith("%"))
                            return converter.ConvertFromString((ITypeDescriptorContext)null, Func.NumCulture, text2);
                        return (Decimal)converter.ConvertFromString((ITypeDescriptorContext)null, Func.NumCulture, text2.TrimEnd('%'));
                    case TypeCode.DateTime:
                        if (string.IsNullOrEmpty(text1))
                            return DBNull.Value;
                        return ((DateTime)converter.ConvertFromString((ITypeDescriptorContext)null, Func.DateCulture, text1)).Date;
                    case TypeCode.String:
                        return text1;
                    default:
                        return converter.ConvertFromString(text1);
                }
            }
            catch (Exception ex)
            {
            }
            return DBNull.Value;
        }

        internal static object ConvertFromDetail(string col, Type type, object val)
        {
            if (val == null)
                return (object)DBNull.Value;
            TypeCode typeCode = Type.GetTypeCode(type);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            try
            {
                string text = val.ToString();
                switch (typeCode)
                {
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        if (string.IsNullOrEmpty(text))
                            return (object)DBNull.Value;
                        return converter.ConvertFromString((ITypeDescriptorContext)null, Func.NumCulture, text);
                    case TypeCode.DateTime:
                        if (string.IsNullOrEmpty(text))
                            return (object)DBNull.Value;
                        return (object)((DateTime)converter.ConvertFromString((ITypeDescriptorContext)null, Func.DateCulture, text)).Date;
                    case TypeCode.String:
                        return (object)text;
                    default:
                        return converter.ConvertFromString(text);
                }
            }
            catch (Exception ex)
            {
            }
            return (object)DBNull.Value;
        }

        internal static void CheckCt(string id_hd, IDbManager myDb, IWebHostEnvironment host)
        {
            string filename = Hddv.DownloadFile(id_hd, "1", host, myDb);
            if (string.IsNullOrEmpty(filename))
                return;
            try
            {
                SqlConnection connection = myDb.GetConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                    
                DataSet invds = Ult.ReadXml(filename);
                DataSet ds = Func.LoadCtData(id_hd, invds, connection, myDb, host);
                
                foreach (DataTable table in ds.Tables)
                    Func.UploadData(table, connection);
                    
                Func.UpdateCt2Ph(id_hd, ds, myDb);
                
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            catch (Exception ex)
            {
            }
        }
        internal static void UpdateCt2Ph(string id_hd, DataSet ds, IDbManager myDb)
        {
            SqlCommand cmd = new SqlCommand("EXEC dbo.POImexInv_Ct2Ph @Id_hd");
            cmd.Parameters.Add("@Id_hd", SqlDbType.VarChar).Value = id_hd;
            myDb.ExecuteSqlCommandAsync(cmd);
        }
        internal static void CreateBkhddvtct(string id_hd, IDbManager myDb)
        {
            SqlCommand cmd = new SqlCommand("EXEC dbo.POImexInv_Create_Bkhddvtct @Id_hd;");
            cmd.Parameters.Add("@Id_hd", SqlDbType.VarChar).Value = id_hd;
            myDb.ExecuteSqlCommandAsync(cmd);
        }
        public static string ReplaceVariable(string value)
        {
            value = value.Replace("'", "").Replace(";", "");
            return value;
        }
    }
}
