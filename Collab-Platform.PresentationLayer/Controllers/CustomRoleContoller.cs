using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomRoleContoller : ControllerBase
    {
        private readonly ICustomRolInterface _customRoleInterface;
        public CustomRoleContoller(ICustomRolInterface customRolInterface) {
            customRolInterface = _customRoleInterface;
        }

        [HttpGet("{CustomRoleID}")]
        public async Task<ActionResult<APIResponse<ProjectRoleDetailDTO>>> GetCustomeRoleByID(Guid CustomRoleID) {
            var customRole = await _customRoleInterface.GetAllCutomRoleByRoleID(CustomRoleID);
            return Ok( new APIResponse<ProjectRoleDetailDTO> { 
                Success = true,
                Data = customRole,
                Messege = "Custome Role retrived Sucessfully"
            });
        }

        [HttpGet("{ProjectId}")]
        public async Task<ActionResult<List<ProjectRoleDetailDTO>>> GetCustomeRoleByProjectID(Guid ProjectId) 
        {
            var customRole = await _customRoleInterface.GetAllCustomRoleByProject(ProjectId);
            return Ok(
                new APIResponse<List<ProjectRoleDetailDTO>> { 
                    Success = true,
                    Data = customRole,
                    Messege = "Custome Role Retrived Sucessfully"
                }
            );
        }
        [HttpPost("{ProjectID}")]
        public async Task<ActionResult<APIResponse>> AddRoleToProejct(Guid ProjectID, [FromBody] CretaeCustomRoleDTO createRole) {
            await _customRoleInterface.CreateCutomeRole(createRole, ProjectID);
            return Ok(
                new APIResponse { 
                    Success = true,
                    Messege = "Role Created Sucessfully"
                }    
            );
        }
    }
}
