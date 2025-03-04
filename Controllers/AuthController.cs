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
                var response = _authService.Login(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
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
    }
}