using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Interfaces
{
    public interface IUserService : IService<User>
    {
        IEnumerable<UserDto> GetAllWithDetails();
        UserDto GetByIdWithDetails(int id);
        UserDto Create(CreateUserDto createUserDto);
        void Update(int id, UpdateUserDto updateUserDto);
    }
}