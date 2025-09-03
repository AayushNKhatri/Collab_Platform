using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if (result.Succeeded) {
                    return Ok(new { Messege = "User Registered"});
                }
                return BadRequest(new { Message = "User registration failed", Error = result.Errors});
            }
            catch(Exception e) 
            {
                return StatusCode(500, new { Message = "Internal Server Error"});
            }
        }
    }
}
