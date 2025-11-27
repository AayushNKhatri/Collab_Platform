using Collab_Platform.ApplicationLayer.DTO.PermissionDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;


namespace Collab_Platform.ApplicationLayer.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _permissionRepo;
        public PermissionService(IPermissionRepo permissionRepo)
        {
            _permissionRepo = permissionRepo;
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
    }
}
