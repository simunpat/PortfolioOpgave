using System;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;

namespace PortfolioOpgave.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public AuthResponseDto Login(LoginDto loginDto)
        {
            var user = _userRepository.Find(u => u.Email == loginDto.Email).FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }

        public AuthResponseDto Register(RegisterDto registerDto)
        {
            if (_userRepository.Find(u => u.Email == registerDto.Email).Any())
                throw new InvalidOperationException("Email already in use");

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _userRepository.Add(user);

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }
    }
}