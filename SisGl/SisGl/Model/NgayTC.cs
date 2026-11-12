using SisGl;
using SisGl.Model;
using System;
using System.Diagnostics;

namespace SisGl.Model
{
    public class NgayTC
    {              
        public static void SetStartDate(DateTime _date)
        {
            StartDate = _date;
        }
        private static DateTime _ngay_ct0;

        public static DateTime StartDate { get => _ngay_ct0; set => _ngay_ct0 = value; }

        public static DateTime StartOfPeriod(int year, int period)
        {
            DateTime dateTime = StartDate;
            if (dateTime.Month > 1)
            {
                int year1 = year;
                int month = period;
                dateTime = StartDate;
                int day = dateTime.Day;
                dateTime = new DateTime(year1, month, day);
                return dateTime.AddMonths(StartDate.Month - 1);
            }
            int year2 = year;
            int month1 = period;
            dateTime = StartDate;
            int day1 = dateTime.Day;
            return new DateTime(year2, month1, day1);
        }

        public static DateTime EndOfPeriod(int year, int period)
        {
            DateTime dateTime = StartDate;
            if (dateTime.Month > 1)
            {
                int year1 = year;
                int month = period;
                dateTime = StartDate;
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

        public static DateTime GetStartDate(DateTime date)
        {
            if (date.ToString("MMdd").CompareTo(StartDate.ToString("MMdd")) >= 0)
                return new DateTime(date.Year, StartDate.Month, StartDate.Day);
            int year = date.Year - 1;
            DateTime startDate = StartDate;
            int month = startDate.Month;
            startDate = StartDate;
            int day = startDate.Day;
            return new DateTime(year, month, day);
        }

        public static int GetYearOfDate(DateTime date)
        {
            DateTime startDate = GetStartDate(date);
            return date.Day < startDate.Day ? date.AddMonths(-startDate.Month).Year : date.AddMonths(-startDate.Month + 1).Year;
        }

        public static int GetPeriodOfDate(DateTime date)
        {
            DateTime startDate = GetStartDate(date);
            return date.Day < startDate.Day ? date.AddMonths(-startDate.Month).Month : date.AddMonths(-startDate.Month + 1).Month;
        }
    }
}
