using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using System.Collections.Generic;

namespace PortfolioOpgave.Interfaces
{
    public interface IProjectCategoryService
    {
        IEnumerable<ProjectCategoryDto> GetAllWithDetails();
        ProjectCategoryDto GetByIdWithDetails(int id);
        ProjectCategoryDto GetByProjectId(int projectId);
        ProjectCategoryDto Create(CreateProjectCategoryDto createProjectCategoryDto);
        void Update(int id, CreateProjectCategoryDto updateProjectCategoryDto);
        void Delete(int id);
    }
}