using Microsoft.AspNetCore.Mvc;
using PortfolioOpgave.Models;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var userDtos = _userService.GetAllWithDetails();
            return Ok(userDtos);
        }

        // GET: api/User/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUser(int id)
        {
            var userDto = _userService.GetByIdWithDetails(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return Ok(userDto);
        }

        // POST: api/User
        [Authorize]
        [HttpPost]
        public ActionResult<UserDto> PostUser(CreateUserDto createUserDto)
        {
            var userDto = _userService.Create(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
        }

        // PUT: api/User/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
            {
                return BadRequest();
            }

            try
            {
                _userService.Update(id, updateUserDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }

                _userService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // This endpoint is accessible to everyone
        [AllowAnonymous]
        [HttpGet("public")]
        public ActionResult<string> PublicEndpoint()
        {
            return Ok("This is a public endpoint");
        }

        // GET: api/User/byEmail/{email}
        [AllowAnonymous]
        [HttpGet("byEmail/{email}")]
        public ActionResult<AuthResponseDto> GetUserByEmail(string email)
        {
            try
            {
                Console.WriteLine($"Finding user by email: {email}");

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required");
                }

                // Get the user by email
                var userRepository = HttpContext.RequestServices.GetService<IUserRepository>();
                var user = userRepository.Find(u => u.Email == email).FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"User with email {email} not found");
                    return NotFound($"User with email {email} not found");
                }

                // Create the response with the correct user ID
                var response = new AuthResponseDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };

                Console.WriteLine($"Found user by email: UserId={response.UserId}, Name={response.Name}, Email={response.Email}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user by email: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}