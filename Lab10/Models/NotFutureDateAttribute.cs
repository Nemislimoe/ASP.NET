using System;
using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is DateTime dt)
            {
                return dt <= DateTime.Now;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Дата не може бути у майбутньому";
        }
    }

    public class Appointment
    {
        [Required(ErrorMessage = "Заголовок обов'язковий")]
        public string Title { get; set; }

        [NotFutureDate(ErrorMessage = "Дата не може бути у майбутньому")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
