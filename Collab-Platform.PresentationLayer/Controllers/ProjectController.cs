using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.PresentationLayer.CustomAttribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectInterface _projectService;
        public ProjectController(IProjectInterface projectService) { 
            _projectService = projectService;
        }

        [HttpPost("Create-Project")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectDto createProject) {
                await _projectService.CreateProject(createProject);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "Project sucessfully created"
                });
            
        }
        [HttpGet]
        public async Task<IActionResult> GetProjectByUserID() {
            var result = await _projectService.GetProjectByUserId();
            return Ok(new APIResponse<List<ProjectDetailDto>> { 
                Success = true,
                Data = result,
                Messege = "User project retrived"
            });
        }
        [HttpGet("{ProjectId}")]
        public async Task<IActionResult> GetProjectByProjectId([FromRoute] Guid ProjectId) {
            var result = await _projectService.GetProjectById(ProjectId);
            return Ok(new APIResponse<ProjectDetailDto> { 
                Success = true, 
                Data = result,
                Messege = "Project Retrived Sucessfully"
            });
        }
        [HttpPut("AddUser/{ProjectId}")]
        [PermissionValidation("edit_user_from_project")]
        public async Task<IActionResult> AddUserToProject(List<string> UserId, Guid ProjectId) {
            await _projectService.AddUserToProject(ProjectId, UserId) ;
            return Ok(new APIResponse { 
                Success = true,
                Messege = "Sucessfully added user's to the project"
            });
        }

        [PermissionValidation("edit_user_from_project")]
        [HttpPut("RemoveUser/{ProjectId}")]
        public async Task<IActionResult> UpdateUserProject(List<string> UserID, Guid ProjectId)
        {
            await _projectService.RemoveUserFormProject(ProjectId, UserID);
            return Ok(new APIResponse { Success = true, Messege = "User sucessfully removed form project" });
        }
        [HttpPut("{ProjectId}")]
        [PermissionValidation("project_edit")]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectDto updateProject, [FromRoute]Guid ProjectId)
        { 
            await _projectService.UpdateProject(ProjectId, updateProject) ;
            return Ok(new APIResponse<ProjectDetailDto> { Success = true, Messege = "Sucessfully updated project" });  
        }
        [HttpDelete("{ProjectId}")]
        [PermissionValidation("project_edit")]
        public async Task<IActionResult> DeleteProejct([FromRoute] Guid ProjectId)
        {
            await _projectService.DeleteProject(ProjectId) ;
            return Ok(new APIResponse { Success = true, Messege = "Project Sucessfully deleted" });
        }
        [HttpGet("All-Project")]
        public async Task<IActionResult> GetAllProject()
        {
            var result = await _projectService.GetAllProject();
            return Ok(new APIResponse<List<ProjectDetailDto>>
            {
                Success = true,
                Data = result,
                Messege = "Project Detail retrived"
            });
        }
    }
}
