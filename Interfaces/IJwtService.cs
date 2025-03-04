using PortfolioOpgave.Models;

namespace PortfolioOpgave.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}