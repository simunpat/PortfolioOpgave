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

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
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
            var educations = _educationService.Find(e => e.UserId == userId);
            return Ok(educations);
        }

        // POST: api/Education
        [Authorize]
        [HttpPost]
        public ActionResult<EducationDto> PostEducation(EducationCreateDto educationDto)
        {
            try
            {
                var createdEducation = _educationService.Create(educationDto);
                return CreatedAtAction(nameof(GetEducation), new { id = createdEducation.Id }, createdEducation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Education/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutEducation(int id, EducationCreateDto educationDto)
        {
            try
            {
                _educationService.Update(id, educationDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Education/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteEducation(int id)
        {
            try
            {
                var education = _educationService.GetById(id);
                if (education == null)
                {
                    return NotFound();
                }

                _educationService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}