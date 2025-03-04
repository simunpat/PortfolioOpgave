using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface ISkillService : IService<Skill>
    {
        IEnumerable<SkillDto> GetAllWithDetails();
        SkillDto GetByIdWithDetails(int id);
        SkillDto Create(SkillCreateDto createSkillDto);
        void Update(int id, SkillCreateDto updateSkillDto);
    }
}