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
            _customRoleInterface = customRolInterface;
        }

        [HttpGet("Project/{CustomRoleID}")]
        public async Task<IActionResult> GetCustomeRoleByID(Guid CustomRoleID) {
            var customRole = await _customRoleInterface.GetAllCutomRoleByRoleID(CustomRoleID);
            return Ok( new APIResponse<ProjectRoleDetailDTO> { 
                Success = true,
                Data = customRole,
                Messege = "Custome Role retrived Sucessfully"
            });
        }

        [HttpGet("{ProjectId}")]
        public async Task<IActionResult> GetCustomeRoleByProjectID(Guid ProjectId) 
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
        public async Task<IActionResult> CreateRole(Guid ProjectID, [FromBody] CretaeCustomRoleDTO createRole) {
            await _customRoleInterface.CreateCutomeRole(createRole, ProjectID);
            return Ok(
                new APIResponse { 
                    Success = true,
                    Messege = "Role Created Sucessfully"
                }    
            );
        }
        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> DeleteProjectRole(Guid RoleId) {
            await _customRoleInterface.DeleteCustomRole(RoleId);
            return Ok(
                new APIResponse { 
                    Success = true,
                    Messege = "Role Sucesfully Deleted"
                }    
            );
        }
        [HttpPut("{RoleId}")]
        public async Task<IActionResult> UpdateProjectRole(Guid RoleId, [FromBody] UpdateCustomRoleDTO updateCustomRole) {

            await _customRoleInterface.UpdateCustomRole(RoleId, updateCustomRole);
            return Ok(
                new APIResponse { 
                    Success = true,
                    Messege = "Role Sucessfully Updated"
                }
            );
        }
        [HttpPut("AddUser/{RoleId}")]
        public async Task<IActionResult> AddUserToRole(Guid RoleId, List<string> UserId)
        {
            await _customRoleInterface.AddUserToRole(UserId, RoleId);
            return Ok(
                new APIResponse
                {
                    Success = true,
                    Messege = "Added User to Role"
                }
            );
        }
        [HttpPut("RemoveUser/{RoleID}")]
        public async Task<IActionResult> RemoveUserFormRole(Guid RoleID, List<string> UserId)
        {
            await _customRoleInterface.RemoveUserFromRole(UserId, RoleID);
            return Ok(
                new APIResponse
                {
                    Success = true,
                    Messege = "User removed from Role"
                }
            );
        }
        [HttpPut("AddPermision/{RoleID}")]
        public async Task<IActionResult> AddPermissionToRole(Guid RoleID, List<int> PermissionId)
        {
            await _customRoleInterface.AddPermissionToRole(PermissionId, RoleID);
            return Ok( new APIResponse
            {
                Success = true,
                Messege = "Permission Added to role"
            });
        }
        [HttpPut("RemovePermission/{RoleId}")]
        public async Task<IActionResult> RemovePermissionFormRole(Guid RoleId, List<int> PemissionId)
        {
            await _customRoleInterface.RemovePermissionFormRole(PemissionId, RoleId);
            return Ok( new APIResponse
            {
                    Success = true,
                    Messege = "Permission Removed Sucessfully"
            });
        }
    }
}
