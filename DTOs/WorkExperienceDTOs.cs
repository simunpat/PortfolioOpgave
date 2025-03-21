using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class WorkExperienceDto
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
    }

    public class CreateWorkExperienceDto
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }
    }

    public class UpdateWorkExperienceDto
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}