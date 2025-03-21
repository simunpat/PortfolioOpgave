using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class ProjectCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateProjectCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }

    public class UpdateProjectCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}