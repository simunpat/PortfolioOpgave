using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        public string Title { get; set; }

        public string? LiveDemoUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProjectCategoryId { get; set; }
    }
}