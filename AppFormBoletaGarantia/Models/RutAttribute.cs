using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AppFormBoletaGarantia.Models
{
    public class RutAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value as string != string.Empty && !ValidarRut(value.ToString()))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        private bool ValidarRut(string strRut)
        {
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            string RutDigito;

            strRut = ClearRut(strRut);
            string digitoVerificador = strRut.Substring(strRut.Length - 1).ToUpper();
            int rut = int.Parse(strRut.Substring(0, strRut.Length - 1));

            Contador = 2;
            Acumulador = 0;

            while (rut != 0)
            {
                Multiplo = (rut % 10) * Contador;
                Acumulador = Acumulador + Multiplo;
                rut = rut / 10;
                Contador = Contador + 1;
                if (Contador == 8)
                {
                    Contador = 2;
                }
            }

            Digito = 11 - (Acumulador % 11);
            RutDigito = Digito.ToString().Trim();

            if (Digito == 10)
            {
                RutDigito = "K";
            }
            else if (Digito == 11)
            {
                RutDigito = "0";
            }

            return RutDigito == digitoVerificador;
        }

        private string ClearRut(string strRut)
        {
            return Regex.Replace(strRut, @"\s+", "").Replace(".", "").Replace("-", "");
        }

    }
}