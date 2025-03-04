using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.DTOs
{
    public class EducationDto
    {
        public int Id { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
    }

    public class EducationCreateDto
    {
        [Required]
        public string Institution { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}