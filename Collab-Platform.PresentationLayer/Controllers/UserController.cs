﻿using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
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
        public async Task<ActionResult<APIResponse>> RegisterUser([FromBody] RegisterDto registerDto) {
                var result = await _userInterface.CreateUser(registerDto);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User registerd sucessfully",
                });          
        }
        [HttpGet("UserProfile")]
        [Authorize]
        public async Task<ActionResult<APIResponse<UserProfileDto>>> UserProfile() {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _userInterface.DeleteUserById(userID);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User sucessfully deleted"
                });
        }
        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdateProfile([FromBody] UpdateUserDTO updateUser) {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userInterface.UpdateUserProfile(updateUser, userID);
                return Ok(new APIResponse { 
                    Success = true,
                    Messege = "User sucessfully updated"
                });
        }
    }
}
