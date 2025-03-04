using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IEducationService : IService<Education>
    {
        IEnumerable<EducationDto> GetAllWithDetails();
        EducationDto GetByIdWithDetails(int id);
        EducationDto Create(EducationCreateDto createEducationDto);
        void Update(int id, EducationCreateDto updateEducationDto);
    }
}