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
    public class WorkExperienceController : ControllerBase
    {
        private readonly IWorkExperienceService _workExperienceService;
        private const string SessionKeyUserId = "_UserId";

        public WorkExperienceController(IWorkExperienceService workExperienceService)
        {
            _workExperienceService = workExperienceService;
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

        // GET: api/WorkExperience
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<WorkExperienceDto>> GetWorkExperiences()
        {
            var workExperiences = _workExperienceService.GetAllWithDetails();
            return Ok(workExperiences);
        }

        // GET: api/WorkExperience/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<WorkExperienceDto> GetWorkExperience(int id)
        {
            var workExperience = _workExperienceService.GetByIdWithDetails(id);
            if (workExperience == null)
            {
                return NotFound();
            }

            return Ok(workExperience);
        }

        // GET: api/WorkExperience/User/5
        [AllowAnonymous]
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<WorkExperienceDto>> GetUserWorkExperiences(int userId)
        {
            var workExperiences = _workExperienceService.GetUserWorkExperiences(userId);
            return Ok(workExperiences);
        }

        // POST: api/WorkExperience
        [HttpPost]
        public ActionResult<WorkExperienceDto> PostWorkExperience(CreateWorkExperienceDto workExperienceDto)
        {
            try
            {
                Console.WriteLine($"Received work experience creation request: {workExperienceDto.Company}");
                Console.WriteLine($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    Console.WriteLine("Work experience creation unauthorized: No user ID in session or headers");
                    return Unauthorized("You must be logged in to create a work experience record");
                }

                // Set the userId in the DTO
                workExperienceDto.UserId = userId.Value;

                Console.WriteLine($"Creating work experience for user ID: {userId}");
                var createdWorkExperience = _workExperienceService.Create(workExperienceDto);
                Console.WriteLine($"Work experience created successfully with ID: {createdWorkExperience.Id}");

                return CreatedAtAction(nameof(GetWorkExperience), new { id = createdWorkExperience.Id }, createdWorkExperience);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Not found during work experience creation: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating work experience: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/WorkExperience/5
        [HttpPut("{id}")]
        public IActionResult PutWorkExperience(int id, CreateWorkExperienceDto workExperienceDto)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to update a work experience record");
                }

                // Set the userId in the DTO
                workExperienceDto.UserId = userId.Value;

                // Check if the work experience belongs to the user
                var existingWorkExperience = _workExperienceService.GetById(id);
                if (existingWorkExperience == null)
                {
                    return NotFound();
                }

                if (existingWorkExperience.UserId != userId.Value)
                {
                    return Unauthorized("You do not have permission to update this work experience record");
                }

                _workExperienceService.Update(id, workExperienceDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating work experience: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/WorkExperience/5
        [HttpDelete("{id}")]
        public IActionResult DeleteWorkExperience(int id)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to delete a work experience record");
                }

                var workExperience = _workExperienceService.GetById(id);
                if (workExperience == null)
                {
                    return NotFound();
                }

                // Verify that the user owns this work experience record
                if (workExperience.UserId != userId.Value)
                {
                    return Unauthorized("You do not have permission to delete this work experience record");
                }

                _workExperienceService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting work experience: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}