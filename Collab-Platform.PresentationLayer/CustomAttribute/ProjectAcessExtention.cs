//using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
//using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
//using Microsoft.AspNetCore.Mvc.Filters;
//using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

//namespace Collab_Platform.PresentationLayer.CustomAttribute
//{
//    public class ProjectAcessExtention : Attribute, IActionFilter
//    {

//        private readonly IHelperService _helperService;
//        private readonly ICustomRoleRepo _customRoleRepo;
//        private readonly string _accessType;
//        private readonly IPermissionRepo _permissionRepo;
//        public ProjectAcessExtention(
//            string accessType,
//            ICustomRoleRepo customRoleRepo,
//            IHelperService helperService,
//            IPermissionRepo permissionRepo
//            ) 
//        {
//            _helperService = helperService;
//            _customRoleRepo = customRoleRepo;
//            _accessType = accessType;
//            _permissionRepo = permissionRepo;
//        }
//        public async Task OnActionExecuted(ActionExecutedContext context)
//        {
//            var currentUser = _helperService.GetTokenDetails().userId;
//            var ParamId = _helperService.GetProjectIDFormRequest();
//            var projectRole = await _customRoleRepo.GetRoleofUserInPorjetc(ParamId, currentUser) ?? throw new InvalidRoleException("This User had not been Asingned Role");
//            var permission = projectRole.SelectMany(u => u.RolePermissions).Select(u => u.Permission).ToList();                                                                                               
                                                                                                                  
                                                                                                                     
            
//        }

//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
