using Microsoft.AspNetCore.Mvc;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;
using PortfolioOpgave.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioOpgave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Project
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ProjectDto>> GetProjects()
        {
            var projects = _projectService.GetAllWithDetails();
            return Ok(projects);
        }

        // GET: api/Project/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetProject(int id)
        {
            var project = _projectService.GetByIdWithDetails(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // GET: api/Project/User/5
        [AllowAnonymous]
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<ProjectDto>> GetUserProjects(int userId)
        {
            var projects = _projectService.Find(p => p.UserId == userId);
            return Ok(projects);
        }

        // POST: api/Project
        [Authorize]
        [HttpPost]
        public ActionResult<ProjectDto> PostProject(ProjectCreateDto projectDto)
        {
            try
            {
                var createdProject = _projectService.Create(projectDto);
                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Project/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutProject(int id, ProjectCreateDto projectDto)
        {
            try
            {
                _projectService.Update(id, projectDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Project/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var project = _projectService.GetById(id);
                if (project == null)
                {
                    return NotFound();
                }

                _projectService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}