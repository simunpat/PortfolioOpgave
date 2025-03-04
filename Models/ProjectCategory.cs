using System.ComponentModel.DataAnnotations;

namespace PortfolioOpgave.Models
{
    public class ProjectCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}