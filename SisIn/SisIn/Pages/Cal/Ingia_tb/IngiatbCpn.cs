using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SisIn.Pages.Cal.Ingia_tb
{
    public partial class IngiatbCpn
    {        
        private static class GiaiHPT
        {
            public static void BacThangDuoi(double[,] arrHPT, List<GroupItem> omaKhos)
            {
                for (int index1 = 0; index1 < omaKhos.Count; ++index1)
                {
                    if (arrHPT[index1, index1] == 0.0)
                    {
                        for (int index2 = index1 + 1; index2 < omaKhos.Count; ++index2)
                        {
                            if (arrHPT[index2, index1] != 0.0)
                            {
                                for (int index3 = index1; index3 <= omaKhos.Count; ++index3)
                                {
                                    double num = arrHPT[index1, index3];
                                    arrHPT[index1, index3] = arrHPT[index2, index3];
                                    arrHPT[index2, index3] = num;
                                }
                                break;
                            }
                        }
                    }
                    double num1 = arrHPT[index1, index1];
                    for (int index2 = omaKhos.Count - 1; index2 > index1; --index2)
                    {
                        if (arrHPT[index2, index1] != 0.0)
                        {
                            double num2 = arrHPT[index2, index1];
                            for (int index3 = index1; index3 <= omaKhos.Count; ++index3)
                            {
                                arrHPT[index2, index3] = arrHPT[index2, index3] / num2;
                                arrHPT[index2, index3] = arrHPT[index2, index3] * num1;
                                arrHPT[index2, index3] = arrHPT[index2, index3] - arrHPT[index1, index3];
                            }
                        }
                    }
                }
            }

            public static void BacThangTren(double[,] arrHPT, List<GroupItem> omaKhos)
            {
                for (int index1 = omaKhos.Count - 1; index1 >= 0; --index1)
                {
                    if (arrHPT[index1, index1] != 0.0)
                    {
                        arrHPT[index1, omaKhos.Count] = arrHPT[index1, omaKhos.Count] / arrHPT[index1, index1];
                        arrHPT[index1, index1] = 1.0;
                    }
                    else
                        arrHPT[index1, omaKhos.Count] = 0.0;

                    for (int index2 = 0; index2 < index1; ++index2)
                    {
                        arrHPT[index2, omaKhos.Count] -= arrHPT[index2, index1] * arrHPT[index1, omaKhos.Count];
                        arrHPT[index2, index1] = 0.0;
                    }
                }
            }

            public static string MatrixText(double[,] arrHPT, List<GroupItem> items)
            {
                var stringList = new List<string>();
                string headerStr = "";
                int numDistinctMaVt = items.Select(x => x.ma_vt).Distinct().Count();

                for (int index = 0; index < items.Count; ++index)
                    headerStr += numDistinctMaVt <= 1
                        ? $"\t{items[index].ma_kho}"
                        : $"\t[{items[index].ma_vt}, {items[index].ma_kho}]";

                stringList.Add(headerStr);

                for (int index1 = 0; index1 < items.Count; ++index1)
                {
                    string rowStr = numDistinctMaVt <= 1
                        ? $"{items[index1].ma_kho}"
                        : $"[{items[index1].ma_vt}, {items[index1].ma_kho}]";

                    for (int index2 = 0; index2 < items.Count + 1; ++index2)
                        rowStr += $"\t{arrHPT[index1, index2]}";

                    stringList.Add(rowStr);
                }

                return string.Join("\n", stringList);
            }
        }

        // Model class
        private class GroupItem
        {
            public string ma_vv { get; set; }
            public string ma_vt { get; set; }
            public string ma_kho { get; set; }
        }

        // Calculate average price using linear equations
        private async Task ProcessBatchDataWithLinearEquations(List<Dictionary<string, object>> batchData,
            int batchIndex, int roundGia, int roundGiaNT, DataTable fullDataTable)
        {
            try
            {
                if (batchData == null || batchData.Count == 0)
                    return;

                // Extract unique combinations of ma_vv, ma_vt, ma_kho (match WPF ordering: ma_vt then ma_vv)
                var groupItems = batchData
                    .Where(x => (x["ma_kho"]?.ToString() ?? "").Trim() != "")
                    .GroupBy(x => new {
                        ma_vv = x["ma_vv"]?.ToString() ?? "",
                        ma_kho = x["ma_kho"]?.ToString() ?? "",
                        ma_vt = x["ma_vt"]?.ToString() ?? ""
                    })
                    .Select(g => new GroupItem
                    {
                        ma_vv = g.Key.ma_vv.Trim(),
                        ma_kho = g.Key.ma_kho.Trim(),
                        ma_vt = g.Key.ma_vt.Trim()
                    })
                    .OrderBy(x => x.ma_vt)
                    .ThenBy(x => x.ma_vv)
                    .ToList();

                if (groupItems.Count == 0)
                    return;

                // Build matrix for linear equations
                double[,] matrix = new double[groupItems.Count, groupItems.Count + 1];
                var tempDataTable = batchData.ToDataTable();

                for (int i = 0; i < groupItems.Count; i++)
                {
                    string filter = $"ma_vt = '{groupItems[i].ma_vt}' AND ma_kho = '{groupItems[i].ma_kho}' AND ma_vv = '{groupItems[i].ma_vv}'";

                    for (int j = 0; j < groupItems.Count + 1; j++)
                    {
                        if (i == j)
                        {
                            // Công thức WPF: ton_ck + sl_xuat_gia0 - sl_nhap_gia0_dc0
                            double tonCk = GetDataTableSum(fullDataTable, filter, "ton_ck");
                            double slXuatGia0 = GetDataTableSum(fullDataTable, filter, "sl_xuat_gia0");
                            double slNhapGia0Dc0 = GetDataTableSum(fullDataTable, filter, "sl_nhap_gia0_dc0");
                            matrix[i, j] = tonCk + slXuatGia0 - slNhapGia0Dc0;
                        }
                        else if (j == groupItems.Count)
                        {
                            // Công thức WPF: du_dk + tien_nhap_gia - tien_xuat_gia
                            double duDk = GetDataTableSum(fullDataTable, filter, "du_dk");
                            double tienNhapGia = GetDataTableSum(fullDataTable, filter, "tien_nhap_gia");
                            double tienXuatGia = GetDataTableSum(fullDataTable, filter, "tien_xuat_gia");
                            matrix[i, j] = duDk + tienNhapGia - tienXuatGia;
                        }
                        else
                        {
                            // Cross-filter: thêm ma_vt của j để match WPF
                            string crossFilter = $"ma_vt = '{groupItems[i].ma_vt}' AND ma_kho = '{groupItems[i].ma_kho}' AND ma_vv = '{groupItems[i].ma_vv}' " +
                                              $"AND ma_vt = '{groupItems[j].ma_vt}' AND ma_khon = '{groupItems[j].ma_kho}' AND ma_vv_n = '{groupItems[j].ma_vv}'";
                            double crossQuantity = GetDataTableSum(fullDataTable, crossFilter, "sl_nhap_gia0_dc");
                            matrix[i, j] = -crossQuantity;
                        }
                    }
                }

                // Solve linear equations
                GiaiHPT.BacThangDuoi(matrix, groupItems);
                GiaiHPT.BacThangTren(matrix, groupItems);

                // Store results
                var results = new List<Dictionary<string, object>>();
                for (int i = 0; i < groupItems.Count; i++)
                {
                    double calculatedPrice = Math.Round(matrix[i, groupItems.Count], roundGia);

                    results.Add(new Dictionary<string, object>
                    {
                        { "ma_vv", groupItems[i].ma_vv },
                        { "ma_kho", groupItems[i].ma_kho },
                        { "ma_vt", groupItems[i].ma_vt },
                        { "gia", calculatedPrice },
                        { "gia_nt", 0 },
                        { "processid", ProcessID },
                        { "nam", Year1 },
                        { "ky", Period1 }
                    });
                }

                // Save results to database
                await SaveCalculatedPricesToDatabase(results);

                progressValue = batchIndex + 1;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xử lý batch {batchIndex}: {ex.Message}", ex);
            }
        }

        private double GetDataTableSum(DataTable dt, string filter, string columnName)
        {
            try
            {
                if (dt == null || !dt.Columns.Contains(columnName))
                    return 0;

                object result = dt.Compute($"SUM({columnName})", filter);
                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToDouble(result);
            }
            catch
            {
                return 0;
            }
        }

        private async Task SaveCalculatedPricesToDatabase(List<Dictionary<string, object>> results)
        {
            try
            {
                List<DataRow> rows = tbKhongGia.AsEnumerable().ToList();
                myDb.SaveDataTable<DataRow>(rows, "dmgiaupload", "processid",ProcessID);
                string strsql = "INSERT INTO[dbo].[dmgiaupload]([nam], [ky], [ma_kho], [ma_vt], [gia], [gia_nt], [computername], [processid], [ma_vv], [ma_dvcs]) VALUES(@nam, @ky, @ma_kho, @ma_vt, @gia, @gia_nt, @computername, @processid, @ma_vv, @ma_dvcs)";
                foreach (var item in results)
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "@Ma_vv", item["ma_vv"]?.ToString() ?? "" },
                        { "@Ma_kho", item["ma_kho"]?.ToString() ?? "" },
                        { "@Ma_vt", item["ma_vt"]?.ToString() ?? "" },
                        { "@Gia", item["gia"] },
                        { "@Gia_nt", item["gia_nt"] },
                        { "processid", ProcessID },
                        { "nam", Year1 },
                        { "ky", Period1 },
                        { "computername", "" },
                        { "ma_dvcs", Ma_dvcs }
                    };                    
                    myDb.LoadDataSet(strsql, parameters);                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu kết quả: {ex.Message}", ex);
            }
        }
    }
  
    public static class ListExtensions
    {
        public static DataTable ToDataTable(this List<Dictionary<string, object>> list)
        {
            var dt = new DataTable();

            if (list.Count == 0)
                return dt;

            foreach (var key in list[0].Keys)
                dt.Columns.Add(key);

            foreach (var dict in list)
            {
                var row = dt.NewRow();
                foreach (var key in dict.Keys)
                    row[key] = dict[key] ?? DBNull.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}