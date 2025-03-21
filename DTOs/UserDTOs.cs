using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PortfolioOpgave.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<ProjectDto> Projects { get; set; }
        public ICollection<SkillDto> Skills { get; set; }
        public ICollection<WorkExperienceDto> WorkExperience { get; set; }
        public ICollection<EducationDto> Education { get; set; }
    }
}