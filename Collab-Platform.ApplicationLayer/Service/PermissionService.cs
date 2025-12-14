using Collab_Platform.ApplicationLayer.DTO.PermissionDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;


namespace Collab_Platform.ApplicationLayer.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _permissionRepo;
        private readonly EndpointDataSource _endPointDataSource;
        public PermissionService(IPermissionRepo permissionRepo, EndpointDataSource endpointDataSource)
        {
            _permissionRepo = permissionRepo;
            _endPointDataSource = endpointDataSource;
        }

        public async Task<List<ViewPermissionDTO>> GetAllPermission()
        {
            var result = await _permissionRepo.GetAllPermission() ?? throw new KeyNotFoundException("Permission are empty");
            var dto = result.Select( p => new ViewPermissionDTO
            {
                PermissionId = p.PermissionId,
                Key = p.Key,
                Description = p.Description,
                name = p.name,
                Category = p.Category
            }).ToList();
            return dto;
        }
        public async Task<List<EndpointDTO>> GetContoller() {
            var apiContoller = _endPointDataSource.Endpoints.OfType<RouteEndpoint>().Select(u => new EndpointDTO
            {
                RoutePattern = u.RoutePattern.RawText,
                HttpMethod = u.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods,
                Controller = u.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault()?.ControllerName,
                Action = u.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault()?.ActionName,
            }).ToList();
            return apiContoller;
        }
    }
}
