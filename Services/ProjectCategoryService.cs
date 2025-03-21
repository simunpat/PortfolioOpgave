using AutoMapper;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Data;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioOpgave.Services
{
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly IRepository<ProjectCategory> _repository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public ProjectCategoryService(
            IRepository<ProjectCategory> repository,
            IRepository<Project> projectRepository,
            IMapper mapper)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public IEnumerable<ProjectCategoryDto> GetAllWithDetails()
        {
            var categories = _repository.GetAll();
            return _mapper.Map<IEnumerable<ProjectCategoryDto>>(categories);
        }

        public ProjectCategoryDto GetByIdWithDetails(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
                return null;

            return _mapper.Map<ProjectCategoryDto>(category);
        }

        public ProjectCategoryDto GetByProjectId(int projectId)
        {
            var project = _projectRepository.GetById(projectId);
            if (project == null)
                return null;

            var category = _repository.GetById(project.ProjectCategoryId);
            if (category == null)
                return null;

            return _mapper.Map<ProjectCategoryDto>(category);
        }

        public ProjectCategoryDto Create(CreateProjectCategoryDto createProjectCategoryDto)
        {
            var category = _mapper.Map<ProjectCategory>(createProjectCategoryDto);
            _repository.Add(category);
            return _mapper.Map<ProjectCategoryDto>(category);
        }

        public void Update(int id, CreateProjectCategoryDto updateProjectCategoryDto)
        {
            var category = _repository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"ProjectCategory with ID {id} not found.");

            _mapper.Map(updateProjectCategoryDto, category);
            _repository.Update(category);
        }

        public void Delete(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"ProjectCategory with ID {id} not found.");

            _repository.Delete(id);
        }
    }
}