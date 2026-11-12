using DevExpress.Blazor;
using SisData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisLib.Ctrl
{
    public class NgayTC
    {
        public static DataTable GetFiltered(IGrid grdBrowse)
        {
            DataTable dat = grdBrowse.Data as DataTable;
            DataTable tblmoi = dat.Clone();
            for (int i = 0; i < grdBrowse.GetVisibleRowCount(); i++)
            {
                DataRow a = (grdBrowse.GetDataItem(i) as DataRowView).Row;
                tblmoi.Rows.Add(a.ItemArray);
            }
            return tblmoi;
        }

        public static string GetFilteredByCol(IGrid grdBrowse, string colName)
        {
            string strres = "";
            for (int i = 0; i < grdBrowse.GetVisibleRowCount(); i++)
            {
                DataRowView ta = grdBrowse.GetDataItem(i) as DataRowView;
                strres += String.IsNullOrEmpty(strres) ? String.Format("'{0}'", ta[colName].ToString()) : String.Format(",'{0}'", ta[colName].ToString());
            }
            return "(" + strres + ")";
        }
        public static DateTime ParseSysVarDate(string value)
        {
            string normalized = value.Replace("-", "/").Trim();
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "M/d/yyyy h:mm:ss tt", "M/d/yyyy H:mm:ss", "M/d/yyyy" };
            if (DateTime.TryParseExact(normalized, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;
            return DateTime.Parse(normalized, CultureInfo.InvariantCulture);
        }

        public static bool CheckValidNgayKs(ISysVarManager sasObj, DateTime? ngay_ct)
        {
            if (!ngay_ct.HasValue)
                return false;
            DateTime dateTime = ParseSysVarDate(sasObj.GetSysVar("M_NGAY_KS"));
            return ngay_ct.Value.Date > dateTime.Date;
        }

        public static bool CheckValidNgayKs(ISysVarManager sasObj, IDbManager myDb, DateTime? ngay_ct, string ma_ct)
        {
            if (!ngay_ct.HasValue)
                return false;
            DateTime dateTime = ParseSysVarDate(sasObj.GetSysVar("M_NGAY_KS"));
            bool flag = ngay_ct.Value.Date > dateTime.Date;
            if (flag)
            {
                string Strsql = String.Format("SELECT count(1) FROM dmct WHERE ma_ct like '{0}' AND ngay_ks >= '{1}'", ma_ct, ngay_ct.Value.ToString("yyyyMMdd"));
                if (Convert.ToInt16(myDb.GetValue(Strsql)) > (short)0)
                    flag = false;
            }
            return flag;
        }

        public static bool CheckValidNgayMs(ISysVarManager sasObj, DateTime? ngay_ct)
        {
            if (!ngay_ct.HasValue)
                return false;
            DateTime dateTime = ParseSysVarDate(sasObj.GetSysVar("M_NGAY_KY1"));
            return ngay_ct.Value.Date >= dateTime.Date;
        }

        public static bool CheckValidNgayKy1(ISysVarManager sasObj, DateTime ngay_ct)
        {
            if (ngay_ct == new DateTime())
                return false;
            DateTime dateTime = ParseSysVarDate(sasObj.GetSysVar("M_NGAY_KY1"));
            return ngay_ct.Date >= dateTime.Date;
        }
        public static DateTime GetStartDate(ISysVarManager sasObj)
        {
            DateTime dateTime = ParseSysVarDate(sasObj.GetSysVar("M_NGAY_DN"));
            return dateTime;
        }
        public static DateTime GetStartDate(ISysVarManager sasObj, DateTime date)
        {
            DateTime dateTime = StartOfPeriod(sasObj, date.Year, date.Month);
            return dateTime;
        }
        public static DateTime StartOfYear(ISysVarManager sasObj, DateTime date)
        {
            DateTime dateTime = StartOfPeriod(sasObj, GetYearOfDate(sasObj,date), 1);
            return dateTime;
        }
        public static DateTime StartOfPeriod(ISysVarManager sasObj, int year, int period)
        {
            DateTime StartDate = GetStartDate(sasObj);
            if (StartDate.Month > 1)
            {
                return new DateTime(year, period, StartDate.Day).AddMonths(StartDate.Month - 1);
            }
            return new DateTime(year, period, StartDate.Day);
        }
        public static DateTime EndOfPeriod(ISysVarManager sasObj,int year, int period)
        {
            DateTime dateTime = GetStartDate(sasObj);
            DateTime StartDate = dateTime;
            if (dateTime.Month > 1)
            {
                int year1 = year;
                int month = period;                
                int day = dateTime.Day;
                dateTime = new DateTime(year1, month, day);
                dateTime = dateTime.AddMonths(StartDate.Month);
                return dateTime.AddDays(-1.0);
            }
            int year2 = year;
            int month1 = period;
            dateTime = StartDate;
            int day1 = dateTime.Day;
            dateTime = new DateTime(year2, month1, day1);
            dateTime = dateTime.AddMonths(1);
            return dateTime.AddDays(-1.0);
        }
        public static int GetYearOfDate(ISysVarManager sasObj,DateTime date)
        {
            DateTime startDate = GetStartDate(sasObj,date);
            return date.Day < startDate.Day ? date.AddMonths(-startDate.Month).Year : date.AddMonths(-startDate.Month + 1).Year;
        }

        public static int GetPeriodOfDate(ISysVarManager sasObj,DateTime date)
        {
            DateTime startDate = GetStartDate(sasObj);
            DateTime start = new DateTime(date.Year, startDate.Month, startDate.Day);
            
            if (date < start)
                start = start.AddYears(-1);            
            int financialMonth = ((date.Year - start.Year) * 12) + date.Month - start.Month + 1;
            return financialMonth;
        }
        public DateTime GetBeginPeriod(ISysVarManager sasObj,int nMonthForward, DateTime Ngay_ct)
        {
            DateTime _ngaytc = GetStartDate(sasObj);           
            DateTime dateTime1 = GetStartDate(sasObj,Ngay_ct);
            for (DateTime dateTime2 = dateTime1.AddMonths(nMonthForward).AddDays(-1.0); Ngay_ct.CompareTo(dateTime1) < 0 || Ngay_ct.CompareTo(dateTime2) > 0; dateTime2 = dateTime1.AddMonths(nMonthForward).AddDays(-1.0))
                dateTime1 = dateTime1.AddMonths(nMonthForward);
            return dateTime1;
        }
    }
}
