using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppFormBoletaGarantia.Class
{
    public class ConfigHelper
    {
        public static string ConnectionStringEntities
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["FormularioBoletaGarantiaEntities"].ConnectionString;
            }
        }

        public static bool CustomUserEnabled
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["App.CustomUser.Enabled"]);
            }
        }

        public static string CustomUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["App.CustomUser.Name"];
            }
        }

        public static string AppAttachmentDir
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Attachment.Dir"];
            }
        }

        public static string AppAttachmentMime
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Attachment.Mime"];
            }
        }

        public static bool AppEmailEnabled
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["App.Email.Enabled"]);
            }
        }

        public static string AppEmailBoleta
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Email.Boleta"];
            }
        }

        public static string AppEmailGerente
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Email.Gerente"];
            }
        }

        public static string AppEmailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Email.From"];
            }
        }

        public static string AppEmailHost
        {
            get
            {
                return ConfigurationManager.AppSettings["App.Email.Host"];
            }
        }

        public static int AppEmailPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["App.Email.Port"]);
            }
        }
    }
}