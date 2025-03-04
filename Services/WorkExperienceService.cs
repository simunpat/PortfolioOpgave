using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class WorkExperienceService : Service<WorkExperience>, IWorkExperienceService
    {
        private readonly IRepository<WorkExperience> _workExperienceRepository;
        private readonly IRepository<User> _userRepository;

        public WorkExperienceService(
            IRepository<WorkExperience> workExperienceRepository,
            IRepository<User> userRepository) : base(workExperienceRepository)
        {
            _workExperienceRepository = workExperienceRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<WorkExperienceDto> GetAllWithDetails()
        {
            var workExperiences = _workExperienceRepository.GetAll();
            return workExperiences.Select(w => new WorkExperienceDto
            {
                Id = w.Id,
                Company = w.Company,
                Position = w.Position,
                StartDate = w.StartDate,
                EndDate = w.EndDate,
                UserId = w.UserId
            }).ToList();
        }

        public WorkExperienceDto GetByIdWithDetails(int id)
        {
            var workExperience = _workExperienceRepository.GetById(id);
            if (workExperience == null)
                return null;

            return new WorkExperienceDto
            {
                Id = workExperience.Id,
                Company = workExperience.Company,
                Position = workExperience.Position,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate,
                UserId = workExperience.UserId
            };
        }

        public WorkExperienceDto Create(WorkExperienceCreateDto createWorkExperienceDto)
        {
            var user = _userRepository.GetById(createWorkExperienceDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createWorkExperienceDto.UserId} not found");

            var workExperience = new WorkExperience
            {
                Company = createWorkExperienceDto.Company,
                Position = createWorkExperienceDto.Position,
                StartDate = createWorkExperienceDto.StartDate,
                EndDate = createWorkExperienceDto.EndDate ?? DateTime.MaxValue,
                UserId = createWorkExperienceDto.UserId
            };

            _workExperienceRepository.Add(workExperience);

            return new WorkExperienceDto
            {
                Id = workExperience.Id,
                Company = workExperience.Company,
                Position = workExperience.Position,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate,
                UserId = workExperience.UserId
            };
        }

        public void Update(int id, WorkExperienceCreateDto updateWorkExperienceDto)
        {
            var workExperience = _workExperienceRepository.GetById(id);
            if (workExperience == null)
                throw new KeyNotFoundException($"WorkExperience with ID {id} not found");

            var user = _userRepository.GetById(updateWorkExperienceDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateWorkExperienceDto.UserId} not found");

            workExperience.Company = updateWorkExperienceDto.Company;
            workExperience.Position = updateWorkExperienceDto.Position;
            workExperience.StartDate = updateWorkExperienceDto.StartDate;
            workExperience.EndDate = updateWorkExperienceDto.EndDate ?? DateTime.MaxValue;
            workExperience.UserId = updateWorkExperienceDto.UserId;

            _workExperienceRepository.Update(workExperience);
        }
    }
}