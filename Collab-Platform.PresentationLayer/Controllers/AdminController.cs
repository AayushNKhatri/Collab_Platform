using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.ApplicationLayer.Service;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        public readonly IAdminInterface _adminInterface;
        public readonly IUserInterface _userInterface;
        public readonly IProjectInterface _projectService;
        public AdminController(IAdminInterface adminInterface, IUserInterface userInterface, IProjectInterface projectService) {
            _adminInterface = adminInterface;
            _userInterface = userInterface;
            _projectService = projectService;
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser() {
            try
            { 
                var result = await _adminInterface.GetAllUserData();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Messege = e.Message});
            }
        }
        [HttpDelete("DeleteUser/{userID}")]
        public async Task<IActionResult> DeletUserById([FromRoute]string userID)
        {
            try 
            {
                if(userID == null) throw new BadHttpRequestException("Please provide UserId");
                await _userInterface.DeleteUserById(userID);
                return Ok(new { Messege = $"User with UserID {userID} has been deleted"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Messege = e.Message});
            }
        }
        [HttpGet("All-Project")]
        public async Task<ActionResult<APIResponse>> GetAllProject()
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
