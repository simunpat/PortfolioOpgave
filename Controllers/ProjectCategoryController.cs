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
    public class ProjectCategoryController : ControllerBase
    {
        private readonly IProjectCategoryService _categoryService;

        public ProjectCategoryController(IProjectCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/ProjectCategory
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ProjectCategoryDto>> GetProjectCategories()
        {
            var categories = _categoryService.GetAllWithDetails();
            return Ok(categories);
        }

        // GET: api/ProjectCategory/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ProjectCategoryDto> GetProjectCategory(int id)
        {
            var category = _categoryService.GetByIdWithDetails(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/ProjectCategory/project/5
        [AllowAnonymous]
        [HttpGet("project/{projectId}")]
        public ActionResult<ProjectCategoryDto> GetProjectCategoryByProjectId(int projectId)
        {
            var category = _categoryService.GetByProjectId(projectId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/ProjectCategory
        [Authorize]
        [HttpPost]
        public ActionResult<ProjectCategoryDto> PostProjectCategory(CreateProjectCategoryDto categoryDto)
        {
            try
            {
                var createdCategory = _categoryService.Create(categoryDto);
                return CreatedAtAction(nameof(GetProjectCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/ProjectCategory/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutProjectCategory(int id, CreateProjectCategoryDto categoryDto)
        {
            try
            {
                _categoryService.Update(id, categoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/ProjectCategory/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProjectCategory(int id)
        {
            try
            {
                var category = _categoryService.GetByIdWithDetails(id);
                if (category == null)
                {
                    return NotFound();
                }

                _categoryService.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}