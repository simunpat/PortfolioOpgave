using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class SkillCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 100)]
        public int ProficiencyLevel { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}