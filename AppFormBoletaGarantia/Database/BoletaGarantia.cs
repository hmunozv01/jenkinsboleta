//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppFormBoletaGarantia.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class BoletaGarantia
    {
        public int Id { get; set; }
        public Nullable<int> InmobiliariaId { get; set; }
        public Nullable<int> TipoMonedaId { get; set; }
        public Nullable<int> TipoGarantiaId { get; set; }
        public Nullable<int> UsuarioSolicitanteId { get; set; }
        public Nullable<int> EstadoBoletaGarantiaId { get; set; }
        public string RutSolicitante { get; set; }
        public string Beneficiario { get; set; }
        public string RutBeneficiario { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public string Glosa { get; set; }
        public Nullable<System.DateTime> FechaVencimiento { get; set; }
        public string Responsable { get; set; }
        public string CentroCosto { get; set; }
        public string Observaciones { get; set; }
        public Nullable<byte> EntregaSantiago { get; set; }
        public string LugarRetiro { get; set; }
        public string NombrePersonaRetiro { get; set; }
        public string RutPersonaRetiro { get; set; }
        public Nullable<System.DateTime> FechaSolicitud { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string Archivo { get; set; }
        public string EmailNotificacion { get; set; }
    
        public virtual EstadoBoletaGarantia EstadoBoletaGarantia { get; set; }
        public virtual Inmobiliaria Inmobiliaria { get; set; }
        public virtual TipoGarantia TipoGarantia { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
