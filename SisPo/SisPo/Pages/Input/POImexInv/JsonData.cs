using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SisPo.Pages.Input.POImexInv
{
    internal class JsonData
    {
        public static DataSet ToDataSet(string data)
        {
            DataSet dataSet = JsonData.InitDataSet();
            DataTable table1 = dataSet.Tables[0];
            DataTable table2 = dataSet.Tables[1];
            try
            {
                FieldInfo[] fields1 = typeof(Invoice).GetFields();
                FieldInfo[] fields2 = typeof(TaxRate).GetFields();
                foreach (Invoice data1 in JsonConvert.DeserializeObject<InvoiceCollection>(data).datas)
                {
                    DataRow row1 = table1.NewRow();
                    foreach (FieldInfo fieldInfo1 in fields1)
                    {
                        string name1 = fieldInfo1.Name;
                        object obj1 = fieldInfo1.GetValue((object)data1);
                        if (obj1 != null)
                        {
                            if (table1.Columns.Contains(name1))
                            {
                                row1[name1] = obj1;
                            }
                            else
                            {
                                string str1 = name1;
                                if (!(str1 == "thttltsuat"))
                                {
                                    if (str1 == "nbcks")
                                    {
                                        try
                                        {
                                            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
                                            {
                                                if (table1.Columns.Contains("cks_nguoi_ban"))
                                                {
                                                    Signing signing = JsonConvert.DeserializeObject<Signing>(obj1.ToString());
                                                    string str2 = signing.Subject.ToString();
                                                    int num1 = str2.IndexOf("CN=");
                                                    int num2 = str2.IndexOf(", S=");
                                                    if (num1 != -1 && num2 != -1 && num2 >= num1)
                                                    {
                                                        string str3 = str2.Substring(num1 + 3, num2 - num1 - 3);
                                                        row1["cks_nguoi_ban"] = (object)str3;
                                                        if (table1.Columns.Contains("ngay_ks_ban"))
                                                            row1["ngay_ks_ban"] = (object)signing.SigningTime;
                                                    }
                                                    else
                                                        continue;
                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                                continue;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    int num = 1;
                                    foreach (TaxRate taxRate in (TaxRate[])obj1)
                                    {
                                        DataRow row2 = table2.NewRow();
                                        row2["id"] = (object)data1.id;
                                        row2["stt"] = (object)num;
                                        foreach (FieldInfo fieldInfo2 in fields2)
                                        {
                                            if (!(fieldInfo2 == (FieldInfo)null))
                                            {
                                                string name2 = fieldInfo2.Name;
                                                object obj2 = fieldInfo2.GetValue((object)taxRate);
                                                if (table2.Columns.Contains(name2))
                                                    row2[name2] = obj2;
                                            }
                                        }
                                        table2.Rows.Add(row2);
                                    }
                                }
                                fieldInfo1.ToString();
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(data1.thtttoan) && data1.ttkhac != null && (uint)data1.ttkhac.Length > 0U)
                    {
                        Other other = ((IEnumerable<Other>)data1.ttkhac).FirstOrDefault<Other>((System.Func<Other, bool>)(o => "HTTToan".Equals(o.ttruong, StringComparison.OrdinalIgnoreCase) || "HT thanh toán".Equals(o.ttruong, StringComparison.OrdinalIgnoreCase)));
                        if (other != null)
                            row1["thtttoan"] = (object)other.dlieu;
                    }
                    table1.Rows.Add(row1);
                }
            }
            catch (Exception ex)
            {
            }
            return dataSet;
        }

        private static DataSet InitDataSet()
        {
            DataSet dataSet = new DataSet();
            string[] strArray1 = "nbmst;khmshdon;khhdon;shdon;mhdon;dvtte;hdon;hsgoc;thtttoan;id;nbdchi;nbten;nky;nmdchi;nmmst;nmten;ntao;ntnhan;ncnhat;tchat;tdlap;tgia;tgtcthue;tgtthue;tgtttbchu;tgtttbso;thdon;tthai;ttxly;mkhang;cks_nguoi_ban;ngay_ks_ban".Split(';');
            DataTable table1 = new DataTable();
            table1.Columns.AddRange(((IEnumerable<string>)strArray1).Select<string, DataColumn>((System.Func<string, DataColumn>)(x => new DataColumn(x))).ToArray<DataColumn>());
            dataSet.Tables.Add(table1);
            string[] strArray2 = "id;stt;tsuat;thtien;tthue;gttsuat".Split(';');
            DataTable table2 = new DataTable();
            table2.Columns.AddRange(((IEnumerable<string>)strArray2).Select<string, DataColumn>((System.Func<string, DataColumn>)(x => new DataColumn(x))).ToArray<DataColumn>());
            dataSet.Tables.Add(table2);
            return dataSet;
        }

        internal static DataTable InitDataDetail()
        {
            DataSet dataSet = new DataSet();
            string[] strArray = "idhdon;stt;dgia;dvtinh;ltsuat;sluong;stbchu;stckhau;tchat;ten;thtcthue;thtien;nbten;tlckhau;tsuat;tthue".Split(';');
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(((IEnumerable<string>)strArray).Select<string, DataColumn>((System.Func<string, DataColumn>)(x => new DataColumn(x))).ToArray<DataColumn>());
            return dataTable;
        }

        internal static string SelectToken(string jsondata, string key)
        {
            try
            {
                return JObject.Parse(jsondata).SelectToken(key).ToString();
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        public static void ToDetailTable(string data, DataTable tbl)
        {
            try
            {
                PhiDetail[] phiDetailArray = (PhiDetail[])null;
                FieldInfo[] fields1 = typeof(InvDetail).GetFields();
                FieldInfo[] fields2 = typeof(InvCt).GetFields();
                InvDetail invDetail = JsonConvert.DeserializeObject<InvDetail>(data);
                foreach (FieldInfo fieldInfo1 in fields1)
                {
                    string name1 = fieldInfo1.Name;
                    object obj1 = fieldInfo1.GetValue((object)invDetail);
                    if (obj1 != null)
                    {
                        if ("thttlphi" == name1)
                            phiDetailArray = (PhiDetail[])obj1;
                        else if (!("hdhhdvu" != name1))
                        {
                            foreach (InvCt invCt in (InvCt[])obj1)
                            {
                                DataRow row = tbl.NewRow();
                                foreach (FieldInfo fieldInfo2 in fields2)
                                {
                                    if (!(fieldInfo2 == (FieldInfo)null))
                                    {
                                        string name2 = fieldInfo2.Name;
                                        object obj2 = fieldInfo2.GetValue((object)invCt);
                                        row[name2] = obj2;
                                    }
                                }
                                tbl.Rows.Add(row);
                            }
                        }
                    }
                }
                if (phiDetailArray == null)
                    return;
                int num = SisConverts.TryToInt((object)tbl.Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>)(x => x["stt"].ToString())).Max<string>()) + 1;
                foreach (PhiDetail phiDetail in phiDetailArray)
                {
                    if (phiDetail.tphi != null && !(phiDetail.tphi == string.Empty))
                    {
                        DataRow row = tbl.NewRow();
                        row["idhdon"] = (object)invDetail.id;
                        row["stt"] = (object)num++;
                        row["ten"] = (object)phiDetail.tlphi;
                        row["thtien"] = Func.ConvertFromDetail((string)null, typeof(Decimal), (object)phiDetail.tphi);
                        tbl.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
