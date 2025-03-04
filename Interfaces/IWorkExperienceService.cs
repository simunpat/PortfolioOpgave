using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IWorkExperienceService : IService<WorkExperience>
    {
        IEnumerable<WorkExperienceDto> GetAllWithDetails();
        WorkExperienceDto GetByIdWithDetails(int id);
        WorkExperienceDto Create(WorkExperienceCreateDto createWorkExperienceDto);
        void Update(int id, WorkExperienceCreateDto updateWorkExperienceDto);
    }
}