using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppFormBoletaGarantia.Database;
using Newtonsoft.Json;
using AppFormBoletaGarantia.Models;
using AppFormBoletaGarantia.Class;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Mail;
using AppFormBoletaGarantia.Extensions;
using System.IO;

namespace AppFormBoletaGarantia.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                List<int?> inmobiliarias = ctx.UsuarioInmobiliaria.Where(ui => ui.UsuarioId == AuthHelper.Usuario.Id).Select(ui => ui.InmobiliariaId).ToList();

                bool esAdmin = AuthHelper.Is(Grupo.AdminSistema);

                List<BoletaGarantia> boletas = ctx.BoletaGarantia
                    .Include("EstadoBoletaGarantia")
                    .Include("TipoGarantia")
                    .Include("Inmobiliaria")
                    .Include("TipoMoneda")
                    .Include("Usuario")
                    //.Where(b => inmobiliarias.Contains(b.InmobiliariaId))
                    .Where(b => esAdmin || b.UsuarioSolicitanteId == AuthHelper.Usuario.Id)
                    .OrderByDescending(b => b.FechaSolicitud.Value)
                    .ToList();

                ViewBag.Boletas = boletas;

                return View();
            }
        }

        [HttpPost]
        public ActionResult VerBoletaGarantia(int boletaGarantiaId)
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                BoletaGarantia boleta = ctx.BoletaGarantia
                    .Include("EstadoBoletaGarantia")
                    .Include("TipoGarantia")
                    .Include("Inmobiliaria")
                    .Include("TipoMoneda")
                    .Include("Usuario")
                    .Where(b => b.Id == boletaGarantiaId)
                    .FirstOrDefault();

                if (boleta == null)
                {
                    throw new HttpException(404, "No existe el elemento en base de datos.");
                }

                ViewBag.Boleta = boleta;

                return View();
            }
        }

        public ActionResult Formulario(BoletaGarantiaForm model)
        {
            ModelState.Clear();
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                if (model.EsNuevo())
                {
                    model.EstadoBoletaGarantiaId = 1;
                    model.EntregaSantiago = true;
                    model.FechaVencimiento = DateTime.Now.ToString("dd-MM-yyyy");
                }
                else
                {
                    BoletaGarantia boleta = ctx.BoletaGarantia.Where(b => b.Id == model.Id).FirstOrDefault();
                    if (boleta == null)
                    {
                        throw new HttpException(404, "Boleta no existe.");
                    }

                    model.EstadoBoletaGarantiaId = boleta.EstadoBoletaGarantiaId;
                    model.TipoGarantiaId = boleta.TipoGarantiaId;
                    model.InmobiliariaId = boleta.InmobiliariaId;
                    model.Beneficiario = boleta.Beneficiario;
                    model.RutBeneficiario = boleta.RutBeneficiario;
                    model.Monto = string.Format(CultureInfo.InvariantCulture, "{0:G29}", boleta.Monto.Value);
                    model.TipoMonedaId = boleta.TipoMonedaId;
                    model.Glosa = boleta.Glosa;
                    model.FechaVencimiento = boleta.FechaVencimiento.Value.ToString("dd-MM-yyyy");
                    model.Responsable = boleta.Responsable;
                    model.CentroCosto = boleta.CentroCosto;
                    model.Observaciones = boleta.Observaciones;
                    model.EntregaSantiago = (boleta.EntregaSantiago == 0) ? false : true;
                    model.LugarRetiro = boleta.LugarRetiro;
                    model.NombrePersonaRetiro = boleta.NombrePersonaRetiro;
                    model.RutPersonaRetiro = boleta.RutPersonaRetiro;

                    ViewBag.Boleta = boleta;
                }

                LoadForm();
                return View(model);
            }
        }

        [HttpPost]
        [ActionName("Formulario")]
        public async Task<ActionResult> FormularioPost(BoletaGarantiaForm model)
        {
            if (ModelState.IsValid)
            {
                using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
                {
                    BoletaGarantia boleta = new BoletaGarantia();

                    if (!model.EsNuevo())
                    {
                        boleta = ctx.BoletaGarantia.Where(b => b.Id == model.Id).FirstOrDefault();
                        if (boleta == null)
                        {
                            throw new HttpException(404, "Boleta no existe.");
                        }

                        boleta.EstadoBoletaGarantiaId = model.EstadoBoletaGarantiaId;
                    }
                    else
                    {
                        boleta.UsuarioSolicitanteId = AuthHelper.Usuario.Id;
                        boleta.FechaSolicitud = DateTime.Now;
                        boleta.FechaCreacion = DateTime.Now;
                        boleta.EstadoBoletaGarantiaId = 1;
                    }

                    boleta.InmobiliariaId = model.InmobiliariaId;
                    boleta.TipoMonedaId = model.TipoMonedaId;
                    boleta.TipoGarantiaId = model.TipoGarantiaId;
                    boleta.RutSolicitante = ctx.Inmobiliaria.Where(i => i.Id == model.InmobiliariaId).FirstOrDefault().Rut;
                    boleta.Beneficiario = model.Beneficiario;
                    boleta.RutBeneficiario = model.RutBeneficiario;
                    boleta.Monto = decimal.Parse(model.Monto, CultureInfo.InvariantCulture);
                    boleta.Glosa = model.Glosa;
                    boleta.FechaVencimiento = DateTime.ParseExact(model.FechaVencimiento, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    boleta.Responsable = model.Responsable;
                    boleta.CentroCosto = model.CentroCosto;
                    boleta.Observaciones = model.Observaciones;
                    boleta.EntregaSantiago = Convert.ToByte((model.EntregaSantiago == true) ? 1 : 0);
                    boleta.LugarRetiro = model.LugarRetiro;
                    boleta.NombrePersonaRetiro = model.NombrePersonaRetiro;
                    boleta.RutPersonaRetiro = model.RutPersonaRetiro;
                    boleta.FechaModificacion = DateTime.Now;
                    boleta.EmailNotificacion = model.EmailNotificacion;

                    if (model.EsNuevo())
                    {
                        ctx.BoletaGarantia.Add(boleta);
                    }
                    ctx.SaveChanges();

                    if (model.Archivo.ContentLength > 0 && ConfigHelper.AppAttachmentMime.Contains(model.Archivo.ContentType.ToLower()))
                    {
                        string ext = Path.GetExtension(model.Archivo.FileName);
                        string nombreArchivo = UtilsHelper.UniqName(ext);

                        model.Archivo.SaveAs(Path.Combine(ConfigHelper.AppAttachmentDir, nombreArchivo));

                        boleta.Archivo = nombreArchivo;
                        ctx.SaveChanges();
                    }

                    if (model.EsNuevo() && ConfigHelper.AppEmailEnabled)
                    {
                        BoletaGarantia boletaEmail = ctx.BoletaGarantia
                            .Include("EstadoBoletaGarantia")
                            .Include("TipoGarantia")
                            .Include("Inmobiliaria")
                            .Include("TipoMoneda")
                            .Include("Usuario")
                            .Where(b => b.Id == boleta.Id)
                            .FirstOrDefault();

                        ViewBag.Boleta = boletaEmail;

                        string body = this.RenderRazorViewToString("~/Views/Emails/_Boleta.cshtml", null);
                        bool result = await EmailHelper.Send(ConfigHelper.AppEmailFrom, ConfigHelper.AppEmailBoleta, "Notificación Nueva Solicitud", body);
                        if (!result)
                        {
                            FlashMessagesHelper.Bad(TempData, "No se pudo enviar el email de notificación.");
                        }

                        if (model.EmailNotificacion != null && model.EmailNotificacion.Trim().Length > 0)
                        {
                            result = await EmailHelper.Send(ConfigHelper.AppEmailFrom, model.EmailNotificacion, "Notificación Nueva Solicitud", body);
                            if (!result)
                            {
                                FlashMessagesHelper.Bad(TempData, "No se pudo enviar el email de notificación.");
                            }
                        }

                        if (ConfigHelper.AppEmailGerente != null && ConfigHelper.AppEmailGerente.Length > 0)
                        {
                            result = await EmailHelper.Send(ConfigHelper.AppEmailFrom, ConfigHelper.AppEmailGerente, "Notificación Nueva Solicitud", body);
                            if (!result)
                            {
                                FlashMessagesHelper.Bad(TempData, "No se pudo enviar el email de notificación.");
                            }
                        }

                        string email = UtilsHelper.getUserEmail("socovesa", AuthHelper.Usuario.Usuario1);
                        if (email != null)
                        {
                            result = await EmailHelper.Send(ConfigHelper.AppEmailFrom, email, "Notificación Nueva Solicitud", body);
                            if (!result)
                            {
                                FlashMessagesHelper.Bad(TempData, "No se pudo enviar el email de notificación.");
                            }
                        }
                        
                    }

                    FlashMessagesHelper.Ok(TempData, "Solicitud guardada correctamente.");
                    return RedirectToAction("Index");
                }

            }

            LoadForm();
            FlashMessagesHelper.Bad(TempData, "Información no válida. Revise los campos para mas detalle.");

            return View(model);
        }

        [NonAction]
        private void LoadForm()
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                var inmobiliarias = ctx.Inmobiliaria
                    .Join(ctx.UsuarioInmobiliaria, i => i.Id, ui => ui.InmobiliariaId, (i, ui) => new { Inmobiliaria = i, UsuarioInmobiliaria = ui })
                    .Where(z => z.UsuarioInmobiliaria.UsuarioId == AuthHelper.Usuario.Id)
                    .Select(z => new
                    {
                        Id = z.Inmobiliaria.Id,
                        Nombre = z.Inmobiliaria.Nombre,
                        Rut = z.Inmobiliaria.Rut
                    }).OrderBy(z => z.Nombre).ToList();

                ViewBag.EstadosBoletaGarantia = ctx.EstadoBoletaGarantia.ToList();
                ViewBag.TiposGarantia = ctx.TipoGarantia.ToList();
                ViewBag.Inmobiliarias = inmobiliarias;
                ViewBag.TiposMoneda = ctx.TipoMoneda.ToList();
                ViewBag.InmobiliariasJson = JsonConvert.SerializeObject(inmobiliarias);
            }
        }

        public async Task<ActionResult> Email()
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                BoletaGarantia boletaEmail = ctx.BoletaGarantia
                            .Include("EstadoBoletaGarantia")
                            .Include("TipoGarantia")
                            .Include("Inmobiliaria")
                            .Include("TipoMoneda")
                            .Include("Usuario")
                            .First();

                ViewBag.Boleta = boletaEmail;

                string body = this.RenderRazorViewToString("~/Views/Emails/_Boleta.cshtml", null);
                bool result = await EmailHelper.Send(ConfigHelper.AppEmailFrom, ConfigHelper.AppEmailBoleta, "Notificación Nueva Solicitud", body);

                return Content(result ? "Ok!" : "Bad!");
            }


        }

        public ActionResult Incompatible()
        {
            return View();
        }

        public ActionResult NoRegistrado()
        {
            return View();
        }
    }
}