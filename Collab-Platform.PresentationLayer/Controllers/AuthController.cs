using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
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
                var loginResult = await _authInterface.Login(login);
                return Ok(new APIResponse<string> { 
                    Data = loginResult,
                    Messege = "login Sucessfull",
                    Success = true
                });
        }
        [HttpGet("CheckLogin")]
        public async Task<IActionResult> CheckIfLoggedIn()
        {
                var JWTToken = HttpContext.Request.Headers.Authorization.ToString();
                if (string.IsNullOrEmpty(JWTToken)) {
                    return Unauthorized("You are not authorize");
                }
                return Ok(new { Token = JWTToken , Mesesege = "You are Authorize"});
        }
    }
}
