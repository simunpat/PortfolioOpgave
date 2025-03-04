using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IProjectCategoryService : IService<ProjectCategory>
    {
        IEnumerable<ProjectCategoryDto> GetAllWithDetails();
        ProjectCategoryDto GetByIdWithDetails(int id);
        ProjectCategoryDto Create(ProjectCategoryCreateDto createProjectCategoryDto);
        void Update(int id, ProjectCategoryCreateDto updateProjectCategoryDto);
    }
}