using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        public AuthController(IAuthInterface authInterface) 
        {
            _authInterface = authInterface;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login) 
        {
            try
            {
                var loginResult = await _authInterface.Login(login);
                return Ok(new { Token = loginResult });
            }
            catch (UnauthorizedAccessException e) { 
                return Unauthorized(new { Message = e.Message});
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Messege = $"There is an error{e}" });
            }
        }
        [HttpGet("CheckLogin")]
        public async Task<IActionResult> CheckIfLoggedIn()
        {
            try
            {
                var JWTToken = HttpContext.Request.Headers.Authorization.ToString();
                if (string.IsNullOrEmpty(JWTToken)) {
                    return Unauthorized("You are not authorize");
                }
                return Ok(new { Token = JWTToken , Mesesege = "You are Authorize"});
            }
            catch (Exception e) 
            {
                return StatusCode(500, new { Messege = $"Error{e}" });
            }
        }
    }
}
