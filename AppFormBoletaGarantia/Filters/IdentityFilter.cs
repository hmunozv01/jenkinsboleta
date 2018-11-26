using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppFormBoletaGarantia.Class;
using System.Data.Entity.Infrastructure;
using System.IO;
using Newtonsoft.Json;
using AppFormBoletaGarantia.Database;

namespace AppFormBoletaGarantia.Filters
{
    public class IdentityFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                try
                {
                    string name = AuthHelper.Identity;

                    if (name.Trim().Length == 0)
                    {
                        throw new Exception("Debe estar autenticado.");
                    }

                    string[] parts = name.Split('\\');
                    string dominio = parts[0];
                    string user = parts[1];

                    Usuario usuario = ctx.Usuario.FirstOrDefault(u => u.Dominio == dominio && u.Usuario1 == user && u.Eliminado == false);
                    if (usuario == null || usuario.GrupoId == null)
                    {
                        throw new Exception("Usuario no registrado.");
                    }

                    ((IObjectContextAdapter)ctx).ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, usuario);

                    AuthHelper.Usuario = usuario;
                    base.OnActionExecuting(filterContext);

                }
                catch (Exception ex)
                {
                    throw new HttpException(503, ex.Message);
                }
            }
        }

    }
}