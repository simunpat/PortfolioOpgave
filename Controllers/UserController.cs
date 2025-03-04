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
    }
}