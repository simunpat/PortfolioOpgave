using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Navigation properties
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();
        public ICollection<Education> Education { get; set; } = new List<Education>();
    }
}