using System;
using System.ComponentModel.DataAnnotations;

namespace Lab9.Models
{
    public class TicketOrder
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int TicketsCount { get; set; }
    }
}
