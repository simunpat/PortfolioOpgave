using System.Collections.Generic;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface ISkillService
    {
        SkillDto GetById(int id);
        IEnumerable<SkillDto> GetAll();
        IEnumerable<SkillDto> GetAllByUserId(int userId);
        SkillDto Create(CreateSkillDto createSkillDto, int userId);
        SkillDto Update(int id, UpdateSkillDto updateSkillDto, int userId);
        void Delete(int id, int userId);
    }
}