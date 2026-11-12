using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.POImexInv
{
    public class SisConverts
    {
        public static int ToInt(object value)
        {
            return ToInt(value, 0);
        }

        public static int ToInt(object value, int defval)
        {
            if (value == null || value == DBNull.Value)
                return defval;
            return Convert.ToInt32(value);
        }

        public static int TryToInt(object value)
        {
            return TryToInt(value, 0);
        }

        public static int TryToInt(object value, int defval)
        {
            if (value == null || value == DBNull.Value)
                return defval;
            string s = value.ToString().Trim();
            if (s == "")
                return defval;
            int result = 0;
            if (int.TryParse(s, out result))
                return result;
            return defval;
        }

        public static Decimal TryToDec(object value)
        {
            return TryToDec(value, Decimal.Zero);
        }

        public static Decimal TryToDec(object value, Decimal defval)
        {
            if (value == null || value == DBNull.Value)
                return defval;
            if (value.ToString().Trim() == "")
                return defval;
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
            }
            return defval;
        }

        public static Decimal ToDec(object value, Decimal defval)
        {
            if (value == null || value == DBNull.Value)
                return defval;
            return Convert.ToDecimal(value);
        }

        public static Decimal ToDec(object value)
        {
            return ToDec(value, Decimal.Zero);
        }

        public static DateTime ToDate(object value)
        {
            if (value == null || value is DBNull)
                return DateTime.Today;
            return (DateTime)value;
        }

        public static DateTime TryToDate(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new DateTime();
            return TryToDate(str, "yyyyMMdd");
        }

        public static DateTime TryToDate(string str, string format)
        {
            if (string.IsNullOrEmpty(str))
                return new DateTime();
            DateTime result;
            DateTime.TryParseExact(str, format, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return result;
        }

        public static string ConvertDataToSql(object value, Type ValueType)
        {
            string str = ValueType.ToString();
            return str == "System.String" ? string.Format("N'{0}'", (value as string).Replace("'", "'")) : (str == "System.DateTime" ? string.Format("'{0}'", ((DateTime)value).ToString("yyyyMMdd")) : string.Format("'{0}'", value));
        }
        public static string GetMD5HashFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
            fileStream.Close();
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return stringBuilder.ToString();
        }
    }
}
