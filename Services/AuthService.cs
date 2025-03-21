using System;
using System.Linq;
using AutoMapper;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public AuthResponseDto Login(LoginDto loginDto)
        {
            Console.WriteLine($"AuthService.Login: Searching for user with email {loginDto.Email}");

            var user = _userRepository.Find(u => u.Email == loginDto.Email).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"AuthService.Login: User with email {loginDto.Email} not found");
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            Console.WriteLine($"AuthService.Login: User found, verifying password");
            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                Console.WriteLine($"AuthService.Login: Password verification failed for user {user.Id}");
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            Console.WriteLine($"AuthService.Login: Password verified, creating response");

            // Manually create the response instead of using AutoMapper
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            Console.WriteLine($"AuthService.Login: Login successful for user {response.UserId}");

            return response;
        }

        public AuthResponseDto Register(RegisterDto registerDto)
        {
            if (_userRepository.Find(u => u.Email == registerDto.Email).Any())
                throw new InvalidOperationException("Email already in use");

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            _userRepository.Add(user);

            var response = _mapper.Map<AuthResponseDto>(user);
            return response;
        }
    }
}