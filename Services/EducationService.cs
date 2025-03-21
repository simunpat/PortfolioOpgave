using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using AutoMapper;
using PortfolioOpgave.Data;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioOpgave.Services
{
    public class EducationService : Service<Education>, IEducationService
    {
        private readonly IRepository<Education> _educationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public EducationService(
            IRepository<Education> educationRepository,
            IRepository<User> userRepository,
            IMapper mapper) : base(educationRepository)
        {
            _educationRepository = educationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<EducationDto> GetAllWithDetails()
        {
            var educations = _educationRepository.GetAll();
            return _mapper.Map<IEnumerable<EducationDto>>(educations);
        }

        public EducationDto GetByIdWithDetails(int id)
        {
            var education = _educationRepository.GetById(id);
            if (education == null)
                return null;

            return _mapper.Map<EducationDto>(education);
        }

        public EducationDto Create(CreateEducationDto createEducationDto)
        {
            var user = _userRepository.GetById(createEducationDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createEducationDto.UserId} not found");

            var education = _mapper.Map<Education>(createEducationDto);
            _educationRepository.Add(education);

            return _mapper.Map<EducationDto>(education);
        }

        public void Update(int id, CreateEducationDto updateEducationDto)
        {
            var education = _educationRepository.GetById(id);
            if (education == null)
                throw new KeyNotFoundException($"Education with ID {id} not found");

            var user = _userRepository.GetById(updateEducationDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateEducationDto.UserId} not found");

            _mapper.Map(updateEducationDto, education);
            _educationRepository.Update(education);
        }

        public IEnumerable<EducationDto> GetUserEducations(int userId)
        {
            var educations = _educationRepository.Find(e => e.UserId == userId);
            return _mapper.Map<IEnumerable<EducationDto>>(educations);
        }
    }
}