using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class SkillService : Service<Skill>, ISkillService
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<User> _userRepository;

        public SkillService(
            IRepository<Skill> skillRepository,
            IRepository<User> userRepository) : base(skillRepository)
        {
            _skillRepository = skillRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<SkillDto> GetAllWithDetails()
        {
            var skills = _skillRepository.GetAll();
            return skills.Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Level = s.ProficiencyLevel
            }).ToList();
        }

        public SkillDto GetByIdWithDetails(int id)
        {
            var skill = _skillRepository.GetById(id);
            if (skill == null)
                return null;

            return new SkillDto
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.ProficiencyLevel
            };
        }

        public SkillDto Create(SkillCreateDto createSkillDto)
        {
            var user = _userRepository.GetById(createSkillDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createSkillDto.UserId} not found");

            var skill = new Skill
            {
                Name = createSkillDto.Name,
                ProficiencyLevel = createSkillDto.ProficiencyLevel,
                UserId = createSkillDto.UserId
            };

            _skillRepository.Add(skill);

            return new SkillDto
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.ProficiencyLevel
            };
        }

        public void Update(int id, SkillCreateDto updateSkillDto)
        {
            var skill = _skillRepository.GetById(id);
            if (skill == null)
                throw new KeyNotFoundException($"Skill with ID {id} not found");

            var user = _userRepository.GetById(updateSkillDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {updateSkillDto.UserId} not found");

            skill.Name = updateSkillDto.Name;
            skill.ProficiencyLevel = updateSkillDto.ProficiencyLevel;
            skill.UserId = updateSkillDto.UserId;

            _skillRepository.Update(skill);
        }
    }
}