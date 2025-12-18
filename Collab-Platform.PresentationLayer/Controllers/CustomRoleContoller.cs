using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.PresentationLayer.CustomAttribute;
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

        [HttpGet("Project/{CustomRoleId}")]
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
        [HttpPost("{ProjectId}")]
        [PermissionValidation("add_role")]
        public async Task<IActionResult> CreateRole(Guid ProjectId, [FromBody] CretaeCustomRoleDTO createRole) {
            await _customRoleInterface.CreateCutomeRole(createRole, ProjectId);
            return Ok(
                new APIResponse { 
                    Success = true,
                    Messege = "Role Created Sucessfully"
                }    
            );
        }
        [HttpDelete("{RoleId}")]
        [PermissionValidation("remove_role")]
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
        [PermissionValidation("update_role")]
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
        [PermissionValidation("edit_user_form_role")]
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
        [HttpPut("RemoveUser/{RoleId}")]
        [PermissionValidation("edit_user_form_role")]
        public async Task<IActionResult> RemoveUserFormRole(Guid RoleId, List<string> UserId)
        {
            await _customRoleInterface.RemoveUserFromRole(UserId, RoleId);
            return Ok(
                new APIResponse
                {
                    Success = true,
                    Messege = "User removed from Role"
                }
            );
        }
        [HttpPut("AddPermision/{RoleId}")]
        [PermissionValidation("update_role")]
        public async Task<IActionResult> AddPermissionToRole(Guid RoleId, List<int> PermissionId)
        {
            await _customRoleInterface.AddPermissionToRole(PermissionId, RoleId);
            return Ok( new APIResponse
            {
                Success = true,
                Messege = "Permission Added to role"
            });
        }
        [HttpPut("RemovePermission/{RoleId}")]
        [PermissionValidation("update_role")]
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
