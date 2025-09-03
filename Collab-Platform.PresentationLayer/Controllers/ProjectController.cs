using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectInterface _projectService;
        public ProjectController(IProjectInterface projectService) { 
            _projectService = projectService;
        }

        [HttpPost("Create-Project")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectDto createProject) {
            try { 
                var result = await _projectService.CreateProject(createProject);
                if(!result.Success)
                    return BadRequest(result.Errors);
                return Ok(result);
            }
            catch(Exception e) {
                return StatusCode(500, new { messege = e.Message, Error = "Internal server error" });
            }
        } 
    }
}
