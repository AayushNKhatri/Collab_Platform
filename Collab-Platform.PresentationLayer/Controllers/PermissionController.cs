using Collab_Platform.ApplicationLayer.DTO.PermissionDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse<List<ViewPermissionDTO>>>> GetPermission()
        {
            var data = await _permissionService.GetAllPermission();
            return Ok( new APIResponse<List<ViewPermissionDTO>>
            {
                Success = true,
                Messege = "Permission Sucessfully Get",
                Data = data
            });
        }
        [HttpGet("Controller")]
        public async Task<IActionResult> GetContollerName() {
            var asm = Assembly.GetExecutingAssembly();
            var data = await _permissionService.GetContoller(asm);
            return Ok( new APIResponse<List<Type>> { 
                Success = true,
                Messege = "Contoller name",
                Data = data
            });
        }
    }
}