using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace AppFormBoletaGarantia.Class
{
    public class UtilsHelper
    {  
        public static string UniqId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string UniqName(string ext)
        {
            return string.Format(@"{0}{1}", DateTime.Now.Ticks, ext).ToLower();
        }

        public static DateTime StartOfWeek(DateTime date)
        {
            int delta = DayOfWeek.Monday - date.DayOfWeek;
            return date.AddDays(delta);
        }

        public static DateTime EndOfWeek(DateTime date)
        {
            return StartOfWeek(date).AddDays(7).AddSeconds(-1);
        }

        public static List<DateTime> WeekDays(DateTime date)
        {
            int delta = DayOfWeek.Monday - date.DayOfWeek;
            DateTime monday = date.AddDays(delta);

            List<DateTime> week = new List<DateTime> { monday };

            for (int i = 1; i < 5; i++)
            {
                week.Add(monday.AddDays(i));
            }

            return week;
        }

        public static string[] Split(string value, string separator)
        {
            return value.Split(new[] { separator }, StringSplitOptions.None);
        }

        public static bool DateParse(string date, string format, out DateTime output)
        {
            return DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
        }

        public static bool DateRangeParse(string date, out DateTime date1, out DateTime date2)
        {
            string[] dates = date.Split(new[] { " - " }, StringSplitOptions.None);
            DateTime.TryParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1);
            DateTime.TryParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2);

            return true;
        }

        public static List<string> ListDaysTask(DateTime from, DateTime to)
        {
            List<string> days = new List<string>();

            for(DateTime date = from; date.Date <= to.Date; date = date.AddDays(1))
            {
                days.Add(date.ToString("yyyyMMdd"));
            }

            return days;
        }

        public static int DiffDaysGantt(DateTime a, DateTime b)
        {
            return ((int)(b - a).TotalDays) + 1;
        }

        public static string GenerateSyncId()
        {
            return DateTime.Now.ToString("yyyyddMMHHmmssffffff") + SHA1(Guid.NewGuid().ToString("N")).ToUpper();
        }

        public static string SHA1(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public static string GetNodeValue(XmlNodeList nodes, string fieldTag, string valueTag, string search)
        {
            foreach (XmlNode node in nodes)
            {
                if (node[fieldTag] != null && node[fieldTag].InnerText == search && node[valueTag] != null)
                {
                    return node[valueTag].InnerText;
                }
            }

            return null;
        }

        public static string formatCLP(decimal value)
        {
            NumberFormatInfo nfi = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";

            return string.Format(nfi, "{0:0,0}", value);
        }

        public static string formatUF(decimal value)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";

            return string.Format(nfi, "{0:0,0.00}", value);
        }

        public static string getUserEmail(string domain, string username)
        {
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, username);

            if (user != null)
            {
                return user.EmailAddress;
            }

            return null;
        }

    }
}