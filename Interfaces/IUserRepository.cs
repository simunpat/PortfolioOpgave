using PortfolioOpgave.Models;

namespace PortfolioOpgave.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllWithDetails();
        User GetByIdWithDetails(int id);
    }
}