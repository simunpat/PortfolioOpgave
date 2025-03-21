using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IMapper _mapper;

        public SkillService(ISkillRepository skillRepository, IMapper mapper)
        {
            _skillRepository = skillRepository;
            _mapper = mapper;
        }

        public SkillDto GetById(int id)
        {
            var skill = _skillRepository.GetById(id);
            return _mapper.Map<SkillDto>(skill);
        }

        public IEnumerable<SkillDto> GetAll()
        {
            var skills = _skillRepository.GetAll();
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        public IEnumerable<SkillDto> GetAllByUserId(int userId)
        {
            var skills = _skillRepository.Find(s => s.UserId == userId);
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        public SkillDto Create(CreateSkillDto createSkillDto, int userId)
        {
            var skill = _mapper.Map<Skill>(createSkillDto);
            skill.UserId = userId;

            _skillRepository.Add(skill);
            return _mapper.Map<SkillDto>(skill);
        }

        public SkillDto Update(int id, UpdateSkillDto updateSkillDto, int userId)
        {
            var skill = _skillRepository.GetById(id);

            if (skill == null || skill.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to update this skill");

            _mapper.Map(updateSkillDto, skill);
            _skillRepository.Update(skill);

            return _mapper.Map<SkillDto>(skill);
        }

        public void Delete(int id, int userId)
        {
            var skill = _skillRepository.GetById(id);

            if (skill == null || skill.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to delete this skill");

            _skillRepository.Delete(id);
        }
    }
}