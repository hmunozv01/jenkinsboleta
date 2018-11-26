using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AppFormBoletaGarantia.Models
{
    public class BoletaGarantiaForm
    {
        public int? Id { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        public int? EstadoBoletaGarantiaId { get; set; }

        [Display(Name = "Tipo de Garantía")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        public int? TipoGarantiaId { get; set; }

        [Display(Name = "Solicitado por")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        public int? InmobiliariaId { get; set; }

        [Display(Name = "Beneficiario")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(255, ErrorMessage = "No puede exceder el máximo de 255 caracteres.")]
        public string Beneficiario { get; set; }

        [Display(Name = "RUT Beneficiario")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [RegularExpression(@"^[0-9]{1,8}\-[K|k|0-9]$", ErrorMessage = "Debe tener un formato válido.")]
        [Rut(ErrorMessage = "Debe ingresar un RUT válido")]
        public string RutBeneficiario { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        //[Range(0, Int32.MaxValue, ErrorMessage = "Debe ingresar un número entero válido.")]
        [RegularExpression(@"\d+(\.\d{1,3})?", ErrorMessage = "Debe ingresar un número válido (máx. 3 decimales).")]
        //[DataType(DataType.Currency)]
        public string Monto { get; set; }

        [Display(Name = "Tipo de Moneda")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        public int? TipoMonedaId { get; set; }

        [Display(Name = "Glosa")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(4000, ErrorMessage = "No puede exceder el máximo de 4000 caracteres.")]
        public string Glosa { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [CustomDate(Format = "dd-MM-yyyy", ErrorMessage = "Debe ser una fecha válida.")]
        public string FechaVencimiento { get; set; }

        [Display(Name = "Responsable")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder el máximo de 255 caracteres.")]
        public string Responsable { get; set; }

        [Display(Name = "Centro de Costo")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder el máximo de 255 caracteres.")]
        public string CentroCosto { get; set; }

        [Display(Name = "Observaciones")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        public string Observaciones { get; set; }

        [Display(Name = "Entrega en Santiago")]
        public bool EntregaSantiago { get; set; }

        [Display(Name = "Lugar de retiro")]
        [RequiredIfOtherFieldIsFalse("EntregaSantiago", ErrorMessage = "Campo obligatorio.")]
        public string LugarRetiro { get; set; }

        [Display(Name = "Nombre persona que retira")]
        [RequiredIfOtherFieldIsFalse("EntregaSantiago", ErrorMessage = "Campo obligatorio.")]
        public string NombrePersonaRetiro { get; set; }

        [Display(Name = "RUT persona que retira")]
        [RequiredIfOtherFieldIsFalse("EntregaSantiago", ErrorMessage = "Campo obligatorio.")]
        [RegularExpression(@"^[0-9]{1,8}\-[K|k|0-9]$", ErrorMessage = "Debe tener un formato válido.")]
        [Rut(ErrorMessage = "Debe ingresar un RUT válido")]
        public string RutPersonaRetiro { get; set; }

        [Display(Name = "Email Notificación")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
        public string EmailNotificacion { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo")]
        //[Required(ErrorMessage = "Debe seleccionar un archivo XML.")]
        [JsonIgnore]
        public HttpPostedFileBase Archivo { get; set; }

        public bool EsNuevo()
        {
            return Id == null || Id <= 0;
        }
    }
}