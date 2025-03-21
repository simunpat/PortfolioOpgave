using System;
using System.Linq.Expressions;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Interfaces
{
    public interface ISkillRepository : IRepository<Skill>
    {
        // Add any skill-specific repository methods here if needed
    }
}