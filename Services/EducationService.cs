using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class EducationService : Service<Education>, IEducationService
    {
        private readonly IRepository<Education> _educationRepository;
        private readonly IRepository<User> _userRepository;

        public EducationService(
            IRepository<Education> educationRepository,
            IRepository<User> userRepository) : base(educationRepository)
        {
            _educationRepository = educationRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<EducationDto> GetAllWithDetails()
        {
            var educations = _educationRepository.GetAll();
            return educations.Select(e => new EducationDto
            {
                Id = e.Id,
                Institution = e.Institution,
                Degree = e.Degree,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                UserId = e.UserId
            }).ToList();
        }

        public EducationDto GetByIdWithDetails(int id)
        {
            var education = _educationRepository.GetById(id);
            if (education == null)
                return null;

            return new EducationDto
            {
                Id = education.Id,
                Institution = education.Institution,
                Degree = education.Degree,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                UserId = education.UserId
            };
        }

        public EducationDto Create(EducationCreateDto createEducationDto)
        {
            var user = _userRepository.GetById(createEducationDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createEducationDto.UserId} not found");

            var education = new Education
            {
                Institution = createEducationDto.Institution,
                Degree = createEducationDto.Degree,
                StartDate = createEducationDto.StartDate,
                EndDate = createEducationDto.EndDate ?? DateTime.MaxValue,
                UserId = createEducationDto.UserId
            };

            _educationRepository.Add(education);

            return new EducationDto
            {
                Id = education.Id,
                Institution = education.Institution,
                Degree = education.Degree,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                UserId = education.UserId
            };
        }

        public void Update(int id, EducationCreateDto updateEducationDto)
        {
            var education = _educationRepository.GetById(id);
            if (education == null)
                throw new KeyNotFoundException($"Education with ID {id} not found");

            var user = _userRepository.GetById(updateEducationDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateEducationDto.UserId} not found");

            education.Institution = updateEducationDto.Institution;
            education.Degree = updateEducationDto.Degree;
            education.StartDate = updateEducationDto.StartDate;
            education.EndDate = updateEducationDto.EndDate ?? DateTime.MaxValue;
            education.UserId = updateEducationDto.UserId;

            _educationRepository.Update(education);
        }
    }
}