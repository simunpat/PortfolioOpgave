using Microsoft.AspNetCore.Mvc;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // GET: api/Skill
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<SkillDto>> GetSkills()
        {
            var skills = _skillService.GetAllWithDetails();
            return Ok(skills);
        }

        // GET: api/Skill/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<SkillDto> GetSkill(int id)
        {
            var skill = _skillService.GetByIdWithDetails(id);
            if (skill == null)
            {
                return NotFound();
            }

            return Ok(skill);
        }

        // GET: api/Skill/User/5
        [AllowAnonymous]
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<SkillDto>> GetUserSkills(int userId)
        {
            var skills = _skillService.Find(s => s.UserId == userId);
            return Ok(skills);
        }

        // POST: api/Skill
        [Authorize]
        [HttpPost]
        public ActionResult<SkillDto> PostSkill(SkillCreateDto skillDto)
        {
            try
            {
                var createdSkill = _skillService.Create(skillDto);
                return CreatedAtAction(nameof(GetSkill), new { id = createdSkill.Id }, createdSkill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Skill/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutSkill(int id, SkillCreateDto skillDto)
        {
            try
            {
                _skillService.Update(id, skillDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Skill/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var skill = _skillService.GetById(id);
                if (skill == null)
                {
                    return NotFound();
                }

                _skillService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}