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

        public WorkExperienceController(IWorkExperienceService workExperienceService)
        {
            _workExperienceService = workExperienceService;
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
            var workExperiences = _workExperienceService.Find(w => w.UserId == userId);
            return Ok(workExperiences);
        }

        // POST: api/WorkExperience
        [Authorize]
        [HttpPost]
        public ActionResult<WorkExperienceDto> PostWorkExperience(WorkExperienceCreateDto workExperienceDto)
        {
            try
            {
                var createdWorkExperience = _workExperienceService.Create(workExperienceDto);
                return CreatedAtAction(nameof(GetWorkExperience), new { id = createdWorkExperience.Id }, createdWorkExperience);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/WorkExperience/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutWorkExperience(int id, WorkExperienceCreateDto workExperienceDto)
        {
            try
            {
                _workExperienceService.Update(id, workExperienceDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/WorkExperience/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteWorkExperience(int id)
        {
            try
            {
                var workExperience = _workExperienceService.GetById(id);
                if (workExperience == null)
                {
                    return NotFound();
                }

                _workExperienceService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}