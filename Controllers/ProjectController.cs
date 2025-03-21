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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private const string SessionKeyUserId = "_UserId";

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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

        // GET: api/Project
        [HttpGet]
        public ActionResult<IEnumerable<ProjectDto>> GetProjects()
        {
            var projects = _projectService.GetAll();
            return Ok(projects);
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetProject(int id)
        {
            var project = _projectService.GetById(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // GET: api/Project/User/5
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<ProjectDto>> GetUserProjects(int userId)
        {
            var projects = _projectService.GetAllByUserId(userId);
            return Ok(projects);
        }

        // GET: api/Project/my
        [HttpGet("my")]
        public ActionResult<IEnumerable<ProjectDto>> GetMyProjects()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized("You must be logged in to view your projects");
            }

            var projects = _projectService.GetAllByUserId(userId.Value);
            return Ok(projects);
        }

        // POST: api/Project
        [HttpPost]
        public ActionResult<ProjectDto> PostProject(CreateProjectDto projectDto)
        {
            try
            {
                Console.WriteLine($"Received project creation request: {projectDto.Title}");
                Console.WriteLine($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    Console.WriteLine("Project creation unauthorized: No user ID in session or headers");
                    return Unauthorized("You must be logged in to create a project");
                }

                Console.WriteLine($"Creating project for user ID: {userId}");
                var createdProject = _projectService.Create(projectDto, userId.Value);
                Console.WriteLine($"Project created successfully with ID: {createdProject.Id}");

                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized project creation: {ex.Message}");
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Not found during project creation: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public IActionResult PutProject(int id, UpdateProjectDto projectDto)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to update a project");
                }

                _projectService.Update(id, projectDto, userId.Value);
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

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var userId = GetUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("You must be logged in to delete a project");
                }

                _projectService.Delete(id, userId.Value);
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