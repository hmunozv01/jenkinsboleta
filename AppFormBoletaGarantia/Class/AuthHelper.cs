using AppFormBoletaGarantia.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFormBoletaGarantia.Class
{
    public class AuthHelper
    {
        private static Dictionary<string, int[]> _permissions = new Dictionary<string, int[]>
        {
            { "Usuarios", new [] { Grupo.AdminSistema } },
        };

        public static string Identity
        {
            get
            {
                if (HttpContext.Current.Session["CustomUser"] != null)
                {
                    return @"SOCOVESA\" + HttpContext.Current.Session["CustomUser"];
                }
                else if (ConfigHelper.CustomUserEnabled)
                {
                    return @"SOCOVESA\" + ConfigHelper.CustomUserName;
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        private static Usuario _usuario = null;
        public static Usuario Usuario
        {
            set
            {

                _usuario = value;
            }
            get
            {
                return _usuario;
            }
        }

        public static bool Administrador
        {
            get
            {
                return EsGrupo(1);
            }
        }

        public static bool Editor
        {
            get
            {
                return EsGrupo(2);
            }
        }

        public static bool Comun
        {
            get
            {
                return EsGrupo(3);
            }
        }

        private static bool EsGrupo(int grupoId)
        {
            return _usuario != null && _usuario.GrupoId == grupoId;
        }

        public static bool Has(params string[] permissions)
        {
            return _permissions.Where(p => permissions.Contains(p.Key) && Is(p.Value)).Any();
        }

        public static bool Is(params int[] groups)
        {
            return Usuario.GrupoId != null && groups.Contains(Usuario.GrupoId.Value);
        }
    }
}