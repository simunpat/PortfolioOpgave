using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using AutoMapper;
using PortfolioOpgave.Data;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioOpgave.Services
{
    public class WorkExperienceService : Service<WorkExperience>, IWorkExperienceService
    {
        private readonly IRepository<WorkExperience> _workExperienceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public WorkExperienceService(
            IRepository<WorkExperience> workExperienceRepository,
            IRepository<User> userRepository,
            IMapper mapper) : base(workExperienceRepository)
        {
            _workExperienceRepository = workExperienceRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<WorkExperienceDto> GetAllWithDetails()
        {
            var workExperiences = _workExperienceRepository.GetAll();
            return _mapper.Map<IEnumerable<WorkExperienceDto>>(workExperiences);
        }

        public WorkExperienceDto GetByIdWithDetails(int id)
        {
            var workExperience = _workExperienceRepository.GetById(id);
            if (workExperience == null)
                return null;

            return _mapper.Map<WorkExperienceDto>(workExperience);
        }

        public WorkExperienceDto Create(CreateWorkExperienceDto createWorkExperienceDto)
        {
            var user = _userRepository.GetById(createWorkExperienceDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createWorkExperienceDto.UserId} not found");

            var workExperience = _mapper.Map<WorkExperience>(createWorkExperienceDto);
            _workExperienceRepository.Add(workExperience);

            return _mapper.Map<WorkExperienceDto>(workExperience);
        }

        public void Update(int id, CreateWorkExperienceDto updateWorkExperienceDto)
        {
            var workExperience = _workExperienceRepository.GetById(id);
            if (workExperience == null)
                throw new KeyNotFoundException($"WorkExperience with ID {id} not found");

            var user = _userRepository.GetById(updateWorkExperienceDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateWorkExperienceDto.UserId} not found");

            _mapper.Map(updateWorkExperienceDto, workExperience);
            _workExperienceRepository.Update(workExperience);
        }

        public IEnumerable<WorkExperienceDto> GetUserWorkExperiences(int userId)
        {
            var workExperiences = _workExperienceRepository.Find(w => w.UserId == userId);
            return _mapper.Map<IEnumerable<WorkExperienceDto>>(workExperiences);
        }
    }
}