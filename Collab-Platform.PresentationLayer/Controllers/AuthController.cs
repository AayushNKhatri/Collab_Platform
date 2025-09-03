using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
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
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto login) 
        {
            try {
                var loginResult = await _authInterface.Login(login);
                if(String.IsNullOrEmpty(loginResult)) { return); }
            }
            catch (Exception e) {
                return StatusCode(500, new { Messege = $"There is an error{e}"});
            }
        }
    }
}
