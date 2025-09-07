using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult<APIResponse>> CreateProjectAsync([FromBody] CreateProjectDto createProject) {
                var result = await _projectService.CreateProject(createProject);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "Project sucessfully created"
                });
            
        } 
    }
}
