using Microsoft.AspNetCore.Components;
using SisData.Data;
using SisPo.Pages.List.Apthue;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SisPo.Pages.Input.POImexInv
{
    public sealed class Dmthue
	{
		private static DataTable tbldmthue30;
		private static DataTable tbldmthue90;
		public static DataTable Dmthue30;
		public static DataTable Dmthue90;
		public static string GetMa_thue(decimal thue_suat)
		{
			DataRow dataRow = Dmthue30.Rows.Cast<DataRow>().FirstOrDefault((DataRow x) => SisConverts.ToDec(x["thue_suat"]) == thue_suat);
			if (dataRow == null)
			{
				return "";
			}
			return dataRow["ma_thue"].ToString().Trim();
		}

		public static string GetMa_thueMap(string ma_thue, IDbManager IDb)
		{            
			return IDb.GetValue($"SELECT ma_thue FROM dmthue90 WHERE status = '1' AND nhom_thue = '{ma_thue.Trim()}'")?.ToString()??"".Trim(); ;
		}

		public static string GetTk_thue(string ma_thue, IDbManager IDb)
		{
            string tk_thue_no = IDb.GetValue($"SELECT tk_thue_no FROM dmthue30 WHERE status = '1' AND nhom_thue = '{ma_thue.Trim()}'")?.ToString()??"";
			return tk_thue_no.Trim();
		}
	}

}
