using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AppFormBoletaGarantia.Models
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public string Format { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value as string != string.Empty)
            {
                string sValue = value.ToString();

                if (Format == null || Format == string.Empty)
                {
                    Format = "yyyy-MM-dd";
                }

                try
                {
                    DateTime date;
                    DateTime.TryParseExact(sValue, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                }
                catch (Exception ex)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return null;
        }
    }
}