using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class ProjectService : Service<Project>, IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ProjectCategory> _categoryRepository;

        public ProjectService(
            IRepository<Project> projectRepository,
            IRepository<User> userRepository,
            IRepository<ProjectCategory> categoryRepository) : base(projectRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<ProjectDto> GetAllWithDetails()
        {
            var projects = _projectRepository.GetAll();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.LiveDemoUrl ?? string.Empty
            }).ToList();
        }

        public ProjectDto GetByIdWithDetails(int id)
        {
            var project = _projectRepository.GetById(id);
            if (project == null)
                return null;

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.LiveDemoUrl ?? string.Empty
            };
        }

        public ProjectDto Create(ProjectCreateDto createProjectDto)
        {
            var user = _userRepository.GetById(createProjectDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createProjectDto.UserId} not found");

            var category = _categoryRepository.GetById(createProjectDto.ProjectCategoryId);
            if (category == null)
                throw new KeyNotFoundException($"ProjectCategory with ID {createProjectDto.ProjectCategoryId} not found");

            var project = new Project
            {
                Title = createProjectDto.Title,
                LiveDemoUrl = createProjectDto.LiveDemoUrl,
                UserId = createProjectDto.UserId,
                ProjectCategoryId = createProjectDto.ProjectCategoryId
            };

            _projectRepository.Add(project);

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.LiveDemoUrl ?? string.Empty
            };
        }

        public void Update(int id, ProjectCreateDto updateProjectDto)
        {
            var project = _projectRepository.GetById(id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {id} not found");

            var user = _userRepository.GetById(updateProjectDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateProjectDto.UserId} not found");

            var category = _categoryRepository.GetById(updateProjectDto.ProjectCategoryId);
            if (category == null)
                throw new KeyNotFoundException($"ProjectCategory with ID {updateProjectDto.ProjectCategoryId} not found");

            project.Title = updateProjectDto.Title;
            project.LiveDemoUrl = updateProjectDto.LiveDemoUrl;
            project.UserId = updateProjectDto.UserId;
            project.ProjectCategoryId = updateProjectDto.ProjectCategoryId;

            _projectRepository.Update(project);
        }
    }
}