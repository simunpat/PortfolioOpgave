using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? LiveDemoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int ProjectCategoryId { get; set; }
    }

    public class CreateProjectDto
    {
        [Required]
        public string Title { get; set; }
        public string? LiveDemoUrl { get; set; }
        [Required]
        public int ProjectCategoryId { get; set; }
    }

    public class UpdateProjectDto
    {
        [Required]
        public string Title { get; set; }
        public string? LiveDemoUrl { get; set; }
        [Required]
        public int ProjectCategoryId { get; set; }
    }
}