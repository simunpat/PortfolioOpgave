using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using System.Collections.Generic;

namespace PortfolioOpgave.Interfaces
{
    public interface IWorkExperienceService : IService<WorkExperience>
    {
        IEnumerable<WorkExperienceDto> GetAllWithDetails();
        WorkExperienceDto GetByIdWithDetails(int id);
        WorkExperienceDto Create(CreateWorkExperienceDto createWorkExperienceDto);
        void Update(int id, CreateWorkExperienceDto updateWorkExperienceDto);
        IEnumerable<WorkExperienceDto> GetUserWorkExperiences(int userId);
    }
}