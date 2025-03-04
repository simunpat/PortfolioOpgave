using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? LiveDemoUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProjectCategoryId { get; set; }
        public ProjectCategory ProjectCategory { get; set; } = null!;
    }
}