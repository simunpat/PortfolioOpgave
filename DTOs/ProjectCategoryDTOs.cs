using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class ProjectCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProjectCategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}