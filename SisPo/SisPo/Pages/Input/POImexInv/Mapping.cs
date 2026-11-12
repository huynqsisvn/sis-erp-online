using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SisData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SisPo.Pages.Input.POImexInv
{
	public sealed class Mapping
	{
		private static string Mapping_Path;

		private static string DateFormat;

		private static CultureInfo culture;

		private static string[] ds_km;

		private DataSet dsInv = null;

		private DataSet dsData = null;

		private static int Stt;

		private static Dictionary<string, string> morecolumns;

		private string Stt_rec { get; set; }

		private string Ma_ct { get; set; }

		static Mapping()
		{
			Mapping_Path = "Template\\Imex\\Inv\\Mapping";
			DateFormat = "yyyy-MM-dd";
			culture = CultureInfo.CreateSpecificCulture("es-US");
			ds_km = new string[5] { "0", "1", "2", "3", "4" };
			Stt = 1;
			morecolumns = new Dictionary<string, string>();
			morecolumns.Add("tien_truoc_ck", "TienTruocCK");
		}

		public Mapping(string stt_rec, string ma_ct, DataSet dsinv, DataSet dsdata)
		{
			Stt_rec = stt_rec;
			Ma_ct = ma_ct;
			dsInv = dsinv;
			dsData = dsdata;
			culture.NumberFormat.NumberDecimalSeparator = ".";
		}

		internal void Map(DbManager myDb, OptionsManager myOptions, SysVarManager mySys, IWebHostEnvironment myWebhost)
		{
			if (dsData.Tables.Count < 2)
			{
				return;
			}			
			DataSet dataSet = myDb.LoadDataSet($"SELECT m_phdbf, m_ctdbf, m_ctgtdbf FROM dmct WHERE ma_ct = '{Ma_ct}'");
			if (dataSet.Tables[0].Rows.Count != 0)
			{
				DataRow dataRow = dataSet.Tables[0].Rows[0];
				string name = dataRow["m_phdbf"].ToString().Trim();
				string name2 = dataRow["m_ctdbf"].ToString().Trim();
				string name3 = dataRow["m_ctgtdbf"].ToString().Trim();
				MapPh(name, dsData.Tables[0],myDb, myOptions, myWebhost, mySys);
				MapCt(name2, dsData.Tables[1], mySys, myWebhost, myDb);
				if (dsData.Tables.Count > 2)
				{
					MapCt(name3, dsData.Tables[2], mySys, myWebhost, myDb);
				}
			}
		}

		private void MapPh(string name, DataTable tbl, DbManager myDb, OptionsManager myOptions, IWebHostEnvironment myWebhost, SysVarManager mySysvar)
		{
			string mappingFilename = GetMappingFilename(name, myWebhost);
			if (!File.Exists(mappingFilename) || !tbl.Columns.Contains("stt_rec") || !tbl.Columns.Contains("ma_ct"))
			{
				return;
			}
			DataRow dataRow = tbl.NewRow();
			dataRow["stt_rec"] = Stt_rec;
			dataRow["ma_ct"] = Ma_ct;
			tbl.Rows.Add(dataRow);
			DataTable dataTable = null;
			string tablename = "TTChung";
			dataTable = dsInv.Tables.Cast<DataTable>().FirstOrDefault((DataTable x) => x.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase));
			if (dataTable != null && dataTable.Rows.Count > 1)
			{
				DataRow dataRow2 = dataTable.Rows[0];
				for (int i = 1; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow3 = dataTable.Rows[i];
					foreach (DataColumn column in dataTable.Columns)
					{
						if (dataRow2[column] == DBNull.Value)
						{
							dataRow2[column] = dataRow3[column];
						}
					}
				}
			}
			string[] array = File.ReadAllLines(mappingFilename);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array3 = (from x in text.Split('=')
								   select x.Trim()).ToArray();
				if (array3.Length < 2)
				{
					if (array3[0].StartsWith("["))
					{
						tablename = array3[0].Trim('[').Trim(']');
						dataTable = dsInv.Tables.Cast<DataTable>().FirstOrDefault((DataTable x) => x.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase));
					}
				}
				else if (dataTable != null)
				{
					string text2 = array3[0];
					string text3 = array3[1];
					if (tbl.Columns.Contains(text2) && dataTable.Columns.Contains(text3))
					{
						dataRow[text2] = GetValue(text2, tbl.Columns[text2].DataType, dataTable.Rows[0][text3], mySysvar, myDb);
					}
				}
			}
			dataRow["so_ct0"] = dataRow["so_ct0"].ToString().Trim().TrimStart('0');
			if (dataRow["ma_nt"].ToString().Trim() == "")
			{
				dataRow["ma_nt"] = myOptions.GetOptionsValue("M_MA_NT0").Trim();
			}
		}

		private void MapCt(string name, DataTable tbl, SysVarManager mySysvar, IWebHostEnvironment myWebhost, IDbManager myDb)
		{
			string mappingFilename = GetMappingFilename(name, myWebhost);
			if (!File.Exists(mappingFilename) || !tbl.Columns.Contains("stt_rec") || !tbl.Columns.Contains("ma_ct"))
			{
				return;
			}
			string[] array = File.ReadAllLines(mappingFilename);
			DataTable dataTable = null;
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array3 = (from x in text.Split('=')
								   select x.Trim()).ToArray();
				if (array3.Length < 2)
				{
					if (!array3[0].StartsWith("["))
					{
						continue;
					}
					string tablename = array3[0].Trim('[').Trim(']');
					dataTable = dsInv.Tables.Cast<DataTable>().FirstOrDefault((DataTable x) => x.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase));
					if (dataTable != null)
					{
						for (int j = tbl.Rows.Count; j < dataTable.Rows.Count; j++)
						{
							DataRow dataRow = tbl.NewRow();
							dataRow["stt_rec"] = Stt_rec;
							dataRow["ma_ct"] = Ma_ct;
							tbl.Rows.Add(dataRow);
						}
					}
				}
				else
				{
					if (dataTable == null)
					{
						continue;
					}
					string text2 = array3[0];
					string text3 = array3[1];
					if (tbl.Columns.Contains(text2) && dataTable.Columns.Contains(text3))
					{
						for (int k = 0; k < dataTable.Rows.Count; k++)
						{
							tbl.Rows[k][text2] = GetValue(text2, tbl.Columns[text2].DataType, dataTable.Rows[k][text3], mySysvar, myDb);
						}
					}
					foreach (KeyValuePair<string, string> morecolumn in morecolumns)
					{
						text2 = morecolumn.Key;
						text3 = morecolumn.Value;
						if (tbl.Columns.Contains(text2) && dataTable.Columns.Contains(text3))
						{
							for (int l = 0; l < dataTable.Rows.Count; l++)
							{
								tbl.Rows[l][text2] = GetValue(text2, tbl.Columns[text2].DataType, dataTable.Rows[l][text3], mySysvar, myDb);
							}
						}
					}
				}
			}
			if (tbl.Columns.Contains("stt_rec0"))
			{
				for (int m = 0; m < tbl.Rows.Count; m++)
				{
					tbl.Rows[m]["stt_rec0"] = string.Format(mySysvar.GetSysVar("M_FORMAT_stt_rec0").ToString(), m + 1);
				}
			}
		}

		private static string GetMappingFilename(string name, IWebHostEnvironment myWebhost)
		{
			string text = Path.Combine(myWebhost.WebRootPath + "\\"+ Mapping_Path, $"Cust\\{name}.MAP");
			if (!File.Exists(text))
			{
				text = Path.Combine(myWebhost.WebRootPath + "\\" + Mapping_Path, $"{name}.MAP");
			}
			return text;
		}

		public static object GetValue(string colname, Type datatype, object value, SysVarManager mySysvar, IDbManager myDb)
		{
			if (colname.StartsWith("ma_thue", StringComparison.OrdinalIgnoreCase))
			{
				return GetMa_thue(value.ToString(), myDb);
			}
			if (colname.StartsWith("thue_suat", StringComparison.OrdinalIgnoreCase))
			{
				return GetThue_suat(value.ToString());
			}
			if (colname.Equals("km_ck", StringComparison.OrdinalIgnoreCase))
			{
				return GetKm(value.ToString());
			}
			if (colname.Equals("stt_rec0", StringComparison.OrdinalIgnoreCase))
			{
				return GetStt_rec0(value.ToString(), mySysvar);
			}
			if (datatype == typeof(DateTime))
			{
				return ToDate(value.ToString());
			}
			if (datatype == typeof(decimal))
			{
				return ToDec(value.ToString());
			}
			if (datatype == typeof(double))
			{
				return (double)ToDec(value.ToString());
			}
			return value;
		}

		internal static DateTime ToDate(string str)
		{
			DateTime result;
			bool flag = DateTime.TryParseExact(str, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
			return result;
		}

		internal static decimal ToDec(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0m;
			}
			str = str.Replace(",", ".");
			decimal result;
			bool flag = decimal.TryParse(str, NumberStyles.Number, culture, out result);
			return result;
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

		private static decimal GetThue_suat(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0m;
			}
			string[] array = value.Split(':');
			string text = array[array.Length - 1].Trim().ToUpper();
			text = text.Trim().Trim('%');
			return ToDec(text);
		}

		private static string GetKm(string value)
		{
			if (ds_km.Any((string x) => x == value))
			{
				return "0";
			}
			return value;
		}

		private static string GetStt_rec0(string stt, SysVarManager mySysvar)
		{
			return string.Format(mySysvar.GetSysVar("M_FORMAT_stt_rec0").ToString(), Stt++);
		}
	}
}
