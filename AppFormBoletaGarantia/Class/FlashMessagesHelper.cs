using System.Web;
using System.Web.Mvc;

namespace AppFormBoletaGarantia.Class
{
    public class FlashMessagesHelper
    {
        public static void Ok(TempDataDictionary temp, string message)
        {
            temp[BuildType("ok")] = message;
        }

        public static void Bad(TempDataDictionary temp, string message)
        {
            temp[BuildType("bad")] = message;
        }

        public static void Warn(TempDataDictionary temp, string message)
        {
            temp[BuildType("warn")] = message;
        }

        public static void Info(TempDataDictionary temp, string message)
        {
            temp[BuildType("info")] = message;
        }

        public static string Ok(TempDataDictionary temp)
        {
            return temp[BuildType("ok")].ToString();
        }

        public static string Bad(TempDataDictionary temp)
        {
            return temp[BuildType("bad")].ToString();
        }

        public static string Warn(TempDataDictionary temp)
        {
            return temp[BuildType("warn")].ToString();
        }

        public static string Info(TempDataDictionary temp)
        {
            return temp[BuildType("info")].ToString();
        }

        public static bool HasOk(TempDataDictionary temp)
        {
            return temp[BuildType("ok")] != null;
        }

        public static bool HasBad(TempDataDictionary temp)
        {
            return temp[BuildType("bad")] != null;
        }

        public static bool HasWarn(TempDataDictionary temp)
        {
            return temp[BuildType("warn")] != null;
        }

        public static bool HasInfo(TempDataDictionary temp)
        {
            return temp[BuildType("info")] != null;
        }

        private static string BuildType(string type)
        {
            return "_flash_message_" + type;
        }
    }
}
