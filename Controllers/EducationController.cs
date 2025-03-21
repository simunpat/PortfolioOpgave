using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioOpgave.Models;
using PortfolioOpgave.Data;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;
        private const string SessionKeyUserId = "_UserId";

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        private int? GetUserId()
        {
            // Try to get the user ID from the request headers
            if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader) &&
                !string.IsNullOrEmpty(userIdHeader) &&
                int.TryParse(userIdHeader, out int headerUserId))
            {
                Console.WriteLine($"Found user ID in header: {headerUserId}");
                return headerUserId;
            }

            // Try to get the user ID from session as a fallback
            try
            {
                if (HttpContext.Session != null)
                {
                    var sessionUserId = HttpContext.Session.GetInt32(SessionKeyUserId);
                    if (sessionUserId.HasValue)
                    {
                        Console.WriteLine($"Found user ID in session: {sessionUserId}");
                        return sessionUserId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user ID from session: {ex.Message}");
            }

            Console.WriteLine("No user ID found in request");
            return null;
        }

        // GET: api/Education
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<EducationDto>> GetEducations()
        {
            var educations = _educationService.GetAllWithDetails();
            return Ok(educations);
        }

        // GET: api/Education/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<EducationDto> GetEducation(int id)
        {
            var education = _educationService.GetByIdWithDetails(id);
            if (education == null)
            {
                return NotFound();
            }

            return Ok(education);
        }

        // GET: api/Education/User/5
        [AllowAnonymous]
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<EducationDto>> GetUserEducations(int userId)
        {
            var educations = _educationService.GetUserEducations(userId);
            return Ok(educations);
        }

        // POST: api/Education
        [HttpPost]
        public ActionResult<EducationDto> PostEducation(CreateEducationDto educationDto)
        {
            try
            {
                Console.WriteLine($"Received education creation request: {educationDto.Institution}");
                Console.WriteLine($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    Console.WriteLine("Education creation unauthorized: No user ID in session or headers");
                    return Unauthorized("You must be logged in to create an education record");
                }

                // Set the userId in the education DTO
                educationDto.UserId = userId.Value;

                Console.WriteLine($"Creating education for user ID: {userId}");
                var createdEducation = _educationService.Create(educationDto);
                Console.WriteLine($"Education created successfully with ID: {createdEducation.Id}");

                return CreatedAtAction(nameof(GetEducation), new { id = createdEducation.Id }, createdEducation);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized education creation: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Not found during education creation: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating education: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Education/5
        [HttpPut("{id}")]
        public IActionResult PutEducation(int id, CreateEducationDto educationDto)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to update an education record");
                }

                // Set the userId in the education DTO
                educationDto.UserId = userId.Value;

                _educationService.Update(id, educationDto);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Education/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEducation(int id)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to delete an education record");
                }

                var education = _educationService.GetById(id);
                if (education == null)
                {
                    return NotFound();
                }

                // Verify that the user owns this education record
                if (education.UserId != userId.Value)
                {
                    return Unauthorized("You do not have permission to delete this education record");
                }

                _educationService.Delete(id);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}