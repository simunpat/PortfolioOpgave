using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IProjectService : IService<Project>
    {
        IEnumerable<ProjectDto> GetAllWithDetails();
        ProjectDto GetByIdWithDetails(int id);
        ProjectDto Create(ProjectCreateDto createProjectDto);
        void Update(int id, ProjectCreateDto updateProjectDto);
    }
}