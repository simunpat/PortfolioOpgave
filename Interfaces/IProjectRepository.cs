using System;
using System.Linq.Expressions;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        // Add any project-specific repository methods here if needed
    }
}