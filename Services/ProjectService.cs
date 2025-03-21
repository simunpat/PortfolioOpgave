using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public ProjectDto GetById(int id)
        {
            var project = _projectRepository.GetById(id);
            return _mapper.Map<ProjectDto>(project);
        }

        public IEnumerable<ProjectDto> GetAll()
        {
            var projects = _projectRepository.GetAll();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public IEnumerable<ProjectDto> GetAllByUserId(int userId)
        {
            var projects = _projectRepository.Find(p => p.UserId == userId);
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public ProjectDto Create(CreateProjectDto createProjectDto, int userId)
        {
            var project = _mapper.Map<Project>(createProjectDto);
            project.UserId = userId;
            project.CreatedAt = DateTime.UtcNow;

            _projectRepository.Add(project);
            return _mapper.Map<ProjectDto>(project);
        }

        public ProjectDto Update(int id, UpdateProjectDto updateProjectDto, int userId)
        {
            var project = _projectRepository.GetById(id);

            if (project == null || project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to update this project");

            _mapper.Map(updateProjectDto, project);
            _projectRepository.Update(project);

            return _mapper.Map<ProjectDto>(project);
        }

        public void Delete(int id, int userId)
        {
            var project = _projectRepository.GetById(id);

            if (project == null || project.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to delete this project");

            _projectRepository.Delete(id);
        }
    }
}