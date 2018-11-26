using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppFormBoletaGarantia.Models
{
    public class EditarUsuarioForm
    {        
        public int? Id { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Debe un rol para el usuario.")]
        public int? GrupoId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar un nombre.")]
        public string Nombre { get; set; }

        [Display(Name = "Cuenta")]
        [Required(ErrorMessage = "Debe ingresar una cuenta.")]
        public string Cuenta { get; set; }

        public IList<InmobiliariaItem> Inmobiliarias { get; set; }

        public bool IsNew {
            get
            {
                return Id == null || Id <= 0;
            }
        }

    }
}