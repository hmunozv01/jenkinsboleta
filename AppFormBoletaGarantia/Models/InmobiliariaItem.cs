using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFormBoletaGarantia.Models
{
    public class InmobiliariaItem
    {

        public int? UsuarioInmobiliariaId { get; set; }
        public int? InmobiliariaId { get; set; }
        public bool? Checked { get; set; }
        public string Nombre { get; set; }

        public bool EsNuevo
        {
            get
            {
                return UsuarioInmobiliariaId == null || UsuarioInmobiliariaId <= 0;
            }
        }

    }
}