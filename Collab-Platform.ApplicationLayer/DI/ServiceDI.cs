using Collab_Platform.ApplicationLayer.Helper;
using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.ApplicationLayer.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Collab_Platform.ApplicationLayer.DI
{
    public static class ServiceDI
    {
        public static IServiceCollection ServiceDependencyInjecttion(this IServiceCollection services) {
            services.AddScoped<IProjectInterface, ProjectService>();
            services.AddScoped<IUserInterface, UserService>();
            services.AddScoped<IAuthInterface, AuthService>();
            services.AddScoped<IAdminInterface, AdminService>();
            services.AddScoped<IDataHelper, DataHelper>();
            services.AddScoped<ITaskInterface, TaskService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICustomRolInterface, CustomRoleService>();
            services.AddScoped<IChannelService, ChannelService>();
            return services;
        }

    }
}
