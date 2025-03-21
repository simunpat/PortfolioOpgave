using System.Collections.Generic;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IProjectService
    {
        ProjectDto GetById(int id);
        IEnumerable<ProjectDto> GetAll();
        IEnumerable<ProjectDto> GetAllByUserId(int userId);
        ProjectDto Create(CreateProjectDto createProjectDto, int userId);
        ProjectDto Update(int id, UpdateProjectDto updateProjectDto, int userId);
        void Delete(int id, int userId);
    }
}