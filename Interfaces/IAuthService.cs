using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Login(LoginDto loginDto);
        AuthResponseDto Register(RegisterDto registerDto);
    }
}