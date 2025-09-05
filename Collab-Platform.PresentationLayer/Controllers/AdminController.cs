using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class AdminController : ControllerBase
    {
        public readonly IAdminInterface _adminInterface;
        public readonly IUserInterface _userInterface;
        public AdminController(IAdminInterface adminInterface, IUserInterface userInterface) {
            _adminInterface = adminInterface;
            _userInterface = userInterface;
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
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeletUserById([FromRoute] string userID)
        {
            try 
            {
                if(userID == null) throw new BadHttpRequestException("Please provide UserId");
                await _userInterface.DeleteUserById(userID);
                return Ok(new { Messege = $"User with UserID$ {userID} has been deleted"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Messege = e.Message});
            }
        }
    }
}
