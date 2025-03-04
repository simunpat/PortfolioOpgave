using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<UserDto> GetAllWithDetails()
        {
            var users = _userRepository.GetAllWithDetails();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Projects = u.Projects?.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.LiveDemoUrl ?? string.Empty
                }).ToList() ?? new List<ProjectDto>(),
                Skills = u.Skills?.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.ProficiencyLevel
                }).ToList() ?? new List<SkillDto>()
            }).ToList();
        }

        public UserDto GetByIdWithDetails(int id)
        {
            var user = _userRepository.GetByIdWithDetails(id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Projects = user.Projects?.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.LiveDemoUrl ?? string.Empty
                }).ToList() ?? new List<ProjectDto>(),
                Skills = user.Skills?.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.ProficiencyLevel
                }).ToList() ?? new List<SkillDto>()
            };
        }

        public UserDto Create(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password)
            };

            _userRepository.Add(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Projects = new List<ProjectDto>(),
                Skills = new List<SkillDto>()
            };
        }

        public void Update(int id, UpdateUserDto updateUserDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;

            _userRepository.Update(user);
        }
    }
}