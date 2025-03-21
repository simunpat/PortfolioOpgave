using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProficiencyLevel { get; set; }
        public int UserId { get; set; }
    }

    public class CreateSkillDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int ProficiencyLevel { get; set; }
    }

    public class UpdateSkillDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int ProficiencyLevel { get; set; }
    }
}