using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class ProjectCategoryService : Service<ProjectCategory>, IProjectCategoryService
    {
        private readonly IRepository<ProjectCategory> _categoryRepository;

        public ProjectCategoryService(
            IRepository<ProjectCategory> categoryRepository) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<ProjectCategoryDto> GetAllWithDetails()
        {
            var categories = _categoryRepository.GetAll();
            return categories.Select(c => new ProjectCategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public ProjectCategoryDto GetByIdWithDetails(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return null;

            return new ProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public ProjectCategoryDto Create(ProjectCategoryCreateDto createProjectCategoryDto)
        {
            var category = new ProjectCategory
            {
                Name = createProjectCategoryDto.Name
            };

            _categoryRepository.Add(category);

            return new ProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public void Update(int id, ProjectCategoryCreateDto updateProjectCategoryDto)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"ProjectCategory with ID {id} not found");

            category.Name = updateProjectCategoryDto.Name;

            _categoryRepository.Update(category);
        }
    }
}