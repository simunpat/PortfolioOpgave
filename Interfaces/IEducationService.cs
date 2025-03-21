using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using System.Collections.Generic;

namespace PortfolioOpgave.Interfaces
{
    public interface IEducationService : IService<Education>
    {
        IEnumerable<EducationDto> GetAllWithDetails();
        EducationDto GetByIdWithDetails(int id);
        EducationDto Create(CreateEducationDto createEducationDto);
        void Update(int id, CreateEducationDto updateEducationDto);
        IEnumerable<EducationDto> GetUserEducations(int userId);
    }
}