using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Position { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}