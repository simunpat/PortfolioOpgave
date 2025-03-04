using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class Education
    {
        public int Id { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string Institution { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}