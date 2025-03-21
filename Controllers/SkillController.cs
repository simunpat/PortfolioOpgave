using Microsoft.AspNetCore.Mvc;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private const string SessionKeyUserId = "_UserId";

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
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

        // GET: api/Skill
        [HttpGet]
        public ActionResult<IEnumerable<SkillDto>> GetSkills()
        {
            var skills = _skillService.GetAll();
            return Ok(skills);
        }

        // GET: api/Skill/5
        [HttpGet("{id}")]
        public ActionResult<SkillDto> GetSkill(int id)
        {
            var skill = _skillService.GetById(id);
            if (skill == null)
            {
                return NotFound();
            }

            return Ok(skill);
        }

        // GET: api/Skill/User/5
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<SkillDto>> GetUserSkills(int userId)
        {
            var skills = _skillService.GetAllByUserId(userId);
            return Ok(skills);
        }

        // POST: api/Skill
        [HttpPost]
        public ActionResult<SkillDto> PostSkill(CreateSkillDto skillDto)
        {
            try
            {
                Console.WriteLine($"Received skill creation request: {skillDto.Name}");
                Console.WriteLine($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    Console.WriteLine("Skill creation unauthorized: No user ID in session or headers");
                    return Unauthorized("You must be logged in to create a skill");
                }

                Console.WriteLine($"Creating skill for user ID: {userId}");
                var createdSkill = _skillService.Create(skillDto, userId.Value);
                Console.WriteLine($"Skill created successfully with ID: {createdSkill.Id}");

                return CreatedAtAction(nameof(GetSkill), new { id = createdSkill.Id }, createdSkill);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized skill creation: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Not found during skill creation: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating skill: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Skill/5
        [HttpPut("{id}")]
        public IActionResult PutSkill(int id, UpdateSkillDto skillDto)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to update a skill");
                }

                _skillService.Update(id, skillDto, userId.Value);
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

        // DELETE: api/Skill/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to delete a skill");
                }

                _skillService.Delete(id, userId.Value);
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
    }
}