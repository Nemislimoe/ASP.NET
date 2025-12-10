using System.ComponentModel.DataAnnotations;

namespace Lab13.Models
{
    public class CalculatorModel
    {
        [Required]
        public int A { get; set; }

        [Required]
        public int B { get; set; }

        public int Sum { get; set; }
        public bool DivisionSuccess { get; set; }
        public string? DivisionMessage { get; set; }

        public double? DivisionResult { get; set; }
    }
}
