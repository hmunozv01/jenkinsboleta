using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AppFormBoletaGarantia.Models
{
    public class DateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value as string != string.Empty)
            {
                string message = null;
                string sValue = value.ToString();
                
                string format = "dd/MM/yyyy";
                if (!Regex.Match(sValue, @"^\d{2}\/\d{2}\/\d{4} - \d{2}\/\d{2}\/\d{4}$").Success)
                {
                    message = "Formato fecha no válido.";
                }
                else
                {
                    try
                    {
                        DateTime date1;
                        DateTime date2;
                        string[] dates = sValue.Split(new[] { " - " }, StringSplitOptions.None);

                        if (!DateTime.TryParseExact(dates[0], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date1) || !DateTime.TryParseExact(dates[1], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date2))
                        {
                            message = "Formato fecha no válido.";
                        }

                        else if (date2.Date < date1.Date)
                        {
                            message = "La fecha de inicio debe ser menor a la fecha de termino.";
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "Formato fecha no válido.";
                    }
                }

                if (message != null)
                {
                    return new ValidationResult(message);
                }
            }

            return null;
        }
    }
}