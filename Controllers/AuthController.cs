using System;
using Microsoft.AspNetCore.Mvc;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDto> Login(LoginDto loginDto)
        {
            try
            {
                Console.WriteLine($"Login request received: Email={loginDto.Email}, Password={new string('*', loginDto.Password?.Length ?? 0)}");

                if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                {
                    Console.WriteLine("Invalid login data: Login DTO is null or has empty values");
                    return BadRequest("Invalid login data");
                }

                var response = _authService.Login(loginDto);
                Console.WriteLine($"Login successful: UserId={response.UserId}, Name={response.Name}");
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.GetType().Name} - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public ActionResult<AuthResponseDto> Register(RegisterDto registerDto)
        {
            try
            {
                var response = _authService.Register(registerDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("debug")]
        public IActionResult DebugUsers()
        {
            try
            {
                var userRepository = HttpContext.RequestServices.GetService<IUserRepository>();
                var users = userRepository.GetAll().ToList();

                Console.WriteLine($"Debug: Found {users.Count} users in database");

                if (users.Count == 0)
                {
                    return Ok(new { message = "No users found in database" });
                }

                // Return anonymized user info for debugging
                var userInfo = users.Select(u => new
                {
                    id = u.Id,
                    email = u.Email,
                    name = u.Name,
                    passwordHashLength = u.PasswordHash?.Length ?? 0
                }).ToList();

                return Ok(new { count = users.Count, users = userInfo });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Debug error: {ex.Message}");
                return StatusCode(500, new { error = "Error retrieving users", message = ex.Message });
            }
        }

        [HttpPost("create-test-user")]
        public IActionResult CreateTestUser()
        {
            try
            {
                var userRepository = HttpContext.RequestServices.GetService<IUserRepository>();
                var users = userRepository.Find(u => u.Email == "john@example.com").ToList();

                if (users.Any())
                {
                    return Ok(new { message = "Test user already exists", userId = users.First().Id });
                }

                // Create a test user
                var user = new PortfolioOpgave.Models.User
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123")
                };

                userRepository.Add(user);
                Console.WriteLine($"Created test user with ID {user.Id}");

                return Ok(new { message = "Test user created", userId = user.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test user: {ex.Message}");
                return StatusCode(500, new { error = "Error creating test user", message = ex.Message });
            }
        }
    }
}