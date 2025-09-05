using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserInterface _userInterface;
        public UserController(IUserInterface userInterface) { 
            _userInterface = userInterface;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto) {
            try
            {
                var result = await _userInterface.CreateUser(registerDto);
                return Ok(new { Message = "User Created", Result = result });
            }
            catch (InvalidOperationException e) {
                return Conflict(new { Messege = e.Message});
            }
            catch (ArgumentException e) 
            {
                return BadRequest(new { Messege = e.Message});
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = "Internal Server Error" ,Error = e});
            }
        }
        [HttpGet("UserProfile")]
        [Authorize]
        public async Task<IActionResult> UserProfile() {
            try {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userInterface.UserProfile(userId);
                return Ok(new { UserProfile = result});
            }
            catch (KeyNotFoundException e) {
                return NotFound(new { Messege = e.Message});
            }
            catch (Exception e) { 
                return StatusCode(500, new { Messege = e.Message});
            }
        }
        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _userInterface.DeleteUserById(userID);
                return Ok(new { Messege = "User Deleted sucessfully"});
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { Messege = e.Message});
            }
            catch (Exception e) 
            {
                return StatusCode(500, new { Messege = e.Message });    
            }
        }
        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDTO updateUser) {
            try {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userInterface.UpdateUserProfile(updateUser, userID);
                return Ok(new { Profile = result });
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e) {
                return NotFound(e.Message);
            }
            catch (Exception e) {
                return StatusCode(500, new { Messege = e.Message });
            }
        }
    }
}
