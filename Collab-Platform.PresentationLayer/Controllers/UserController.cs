using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserInterface _userInterface;
        public readonly IHelperService _helperService;
        public UserController(IUserInterface userInterface, IHelperService helperService) { 
            _userInterface = userInterface;
            _helperService = helperService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto) {
                await _userInterface.CreateUser(registerDto);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User registerd sucessfully",
                });          
        }
        [HttpGet("UserProfile")]
        [Authorize]
        public async Task<IActionResult> UserProfile() {
                string userId = _helperService.GetTokenDetails().userId;
                var result = await _userInterface.UserProfile(userId);
                return Ok(new APIResponse<UserProfileDto> 
                {
                    Data = result,
                    Success = true,
                    Messege = "User seucessfully retrived"
                });
           
        }
        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> DeleteUser()
        {
            string userId = _helperService.GetTokenDetails().userId;
            await _userInterface.DeleteUserById(userId);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User sucessfully deleted"
                });
        }
        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDTO updateUser) {
            string userId = _helperService.GetTokenDetails().userId;
            await _userInterface.UpdateUserProfile(updateUser, userId);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User sucessfully updated"
                });
        }
    }
}
