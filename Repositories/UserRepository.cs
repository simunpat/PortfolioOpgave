using Microsoft.EntityFrameworkCore;
using PortfolioOpgave.Data;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<User> GetAllWithDetails()
        {
            return _context.Users
                .Include(u => u.Projects)
                .Include(u => u.Skills)
                .Include(u => u.WorkExperience)
                .Include(u => u.Education)
                .ToList();
        }

        public User GetByIdWithDetails(int id)
        {
            return _context.Users
                .Include(u => u.Projects)
                .Include(u => u.Skills)
                .Include(u => u.WorkExperience)
                .Include(u => u.Education)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}