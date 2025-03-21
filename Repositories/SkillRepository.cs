using PortfolioOpgave.Data;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Repositories
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}