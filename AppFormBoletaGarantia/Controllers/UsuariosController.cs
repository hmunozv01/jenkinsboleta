using AppFormBoletaGarantia.Class;
using AppFormBoletaGarantia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppFormBoletaGarantia.Database;
using AppFormBoletaGarantia.Filters;

namespace AppFormBoletaGarantia.Controllers
{
    public class UsuariosController : Controller
    {
        [Auth("Usuarios")]
        public ActionResult Index()
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                int[] ids = new int[] { 51, 52 };
                List<SP_GetUsuarios_Result> usuarios = ctx.SP_GetUsuarios().Where(u => !ids.Contains(u.Id)).ToList();
                ViewBag.Usuarios = usuarios;

                ViewBag.UsuariosJson = JsonConvert.SerializeObject(usuarios);
                return View();
            }
        }

        [HttpGet]
        [Auth("Usuarios")]
        public ActionResult Editar(EditarUsuarioForm model)
        {
            ModelState.Clear();

            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                Usuario usuario = null;

                if (model.Id > 0)
                {
                    usuario = ctx.Usuario.Where(t => t.Id == model.Id).FirstOrDefault();
                    if (usuario == null)
                    {
                        throw new HttpException(404, "Usuario no existe.");
                    }

                    model.Nombre = usuario.Nombre;
                    model.Cuenta = usuario.Usuario1;
                    model.GrupoId = usuario.GrupoId;
                }

                //int[] ids = new int[] { 1, 2, 8, 9 };
                //List<Grupo> grupos = ctx.Grupo.Where(g => ids.Contains(g.Id)).ToList();
                List<Grupo> grupos = ctx.Grupo.ToList();
                List<InmobiliariaItem> inmobiliarias = ctx.SP_UsuarioInmobiliarias(usuario == null ? 0 : usuario.Id).Select(ui => new InmobiliariaItem
                {
                    UsuarioInmobiliariaId = ui.UsuarioInmobiliariaId,
                    InmobiliariaId = ui.Id,
                    Nombre = ui.Nombre,
                    Checked = ui.Checked,
                }).ToList();
                model.Inmobiliarias = inmobiliarias;

                ViewBag.Usuario = usuario;
                ViewBag.Grupos = grupos;

                return View(model);
            }
        }

        [HttpPost]
        [Auth("Usuarios")]
        [ActionName("Editar")]
        public ActionResult EditarPost(EditarUsuarioForm model)
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                Usuario usuario = new Usuario();

                bool repetido = false;
                if (!model.IsNew)
                {
                    usuario = ctx.Usuario.Where(t => t.Id == model.Id).FirstOrDefault();
                    if (usuario == null)
                    {
                        throw new HttpException(404, "Usuario no existe.");
                    }

                    repetido = ctx.Usuario.Where(u => u.Id != usuario.Id && u.Usuario1 == model.Cuenta).Any();
                }
                else
                {
                    repetido = ctx.Usuario.Where(u => u.Usuario1 == model.Cuenta).Any();
                }

                if (repetido)
                {
                    ModelState.AddModelError("Cuenta", "Ya existe un usuario usando esta cuenta.");
                }

                if (ModelState.IsValid)
                {

                    usuario.Nombre = model.Nombre;
                    usuario.Dominio = "SOCOVESA";
                    usuario.Usuario1 = model.Cuenta;
                    usuario.GrupoId = model.GrupoId;
                    usuario.FechaModificacion = DateTime.Now;

                    if (model.IsNew)
                    {
                        usuario.Eliminado = false;
                        usuario.FechaCreacion = DateTime.Now;
                        ctx.Usuario.Add(usuario);
                    }

                    ctx.SaveChanges();

                    Dictionary<int?, UsuarioInmobiliaria> usuarioInmobiliarias = ctx.UsuarioInmobiliaria.Where(ui => ui.UsuarioId == usuario.Id).ToDictionary(ui => ui.InmobiliariaId, ui => ui);
                    foreach (InmobiliariaItem ui in model.Inmobiliarias)
                    {
                        if (usuarioInmobiliarias.ContainsKey(ui.InmobiliariaId))
                        {
                            UsuarioInmobiliaria _ui = usuarioInmobiliarias[ui.InmobiliariaId];
                            if (ui.Checked == null || ui.Checked == false)
                            {
                                ctx.UsuarioInmobiliaria.Remove(_ui);
                            }
                        }
                        else if (ui.Checked == true)
                        {
                            UsuarioInmobiliaria _ph = new UsuarioInmobiliaria();
                            _ph.UsuarioId = usuario.Id;
                            _ph.InmobiliariaId = ui.InmobiliariaId;
                            ctx.UsuarioInmobiliaria.Add(_ph);
                        }
                    }

                    ctx.SaveChanges();

                    FlashMessagesHelper.Ok(TempData, "Usuario guardado correctamente.");
                    return RedirectToAction("Index");

                }

                List<Grupo> grupos = ctx.Grupo.ToList();
                ViewBag.Usuario = usuario;
                ViewBag.Grupos = grupos;

                return View(model);
            }
        }

        [HttpGet]
        [Auth("Usuarios")]
        public ActionResult Eliminar(int id)
        {
            using (FormularioBoletaGarantiaEntities ctx = new FormularioBoletaGarantiaEntities())
            {
                Usuario usuario = ctx.Usuario.Where(t => t.Id == id).FirstOrDefault();
                if (usuario == null || usuario.Id == 1)
                {
                    throw new HttpException(404, "Usuario no existe.");
                }

                usuario.Eliminado = true;
                usuario.FechaModificacion = DateTime.Now;
                ctx.SaveChanges();

                FlashMessagesHelper.Info(TempData, "Usuario eliminado correctamente.");
                return RedirectToAction("Index");
            }
        }
    }
}