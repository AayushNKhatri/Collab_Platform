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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetProjectByUserID() {
            var result = await _projectService.GetProjectByUserId();
            return Ok(new APIResponse<List<ProjectDetailDto>> { 
                Success = true,
                Data = result,
                Messege = "User project retrived"
            });
        }
        [HttpGet("{ProjectId}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetProjectByProjectId([FromRoute] Guid ProjectId) {
            var result = await _projectService.GetProjectById(ProjectId);
            return Ok(new APIResponse<ProjectDetailDto> { 
                Success = true, 
                Data = result,
                Messege = "Project Retrived Sucessfully"
            });
        }
        [HttpPut("AddUser")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> AddUserToProject([FromBody] AddUserProjectDto addUserProject) {
            await _projectService.AddUserToProject(addUserProject) ;
            return Ok(new APIResponse { 
                Success = true,
                Messege = "Sucessfully added user's to the project"
            });
        }
        [HttpPut("{ProjectID}")]
        public async Task<ActionResult<APIResponse<ProjectDetailDto>>> UpdateProject([FromBody] UpdateProjectDto updateProject, [FromRoute]Guid ProjectID)
        { 
            var result = await _projectService.UpdateProject(ProjectID, updateProject) ;
            return Ok(new APIResponse<ProjectDetailDto> { Success = true, Messege = "Sucessfully updated project", Data=result });  
        }
        [HttpDelete("{ProjectID}")]
        public async Task<ActionResult<APIResponse>> DeleteProejct([FromRoute] Guid ProjectID)
        {
            await _projectService.DeleteProject(ProjectID) ;
            return Ok(new APIResponse { Success = true, Messege = "Project Sucessfully deleted" });
        }
    }
}
