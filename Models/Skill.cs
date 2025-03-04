using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ProficiencyLevel { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}