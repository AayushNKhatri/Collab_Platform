using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.InfastructureLayer.Database;
using Collab_Platform.InfastructureLayer.Repository;
using Collab_Platform.InfastructureLayer.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Collab_Platform.InfastructureLayer.DI
{
    public static class RepoDependencyInjection
    {
        public static IServiceCollection RepoDI(this IServiceCollection services) {
            services.AddScoped<IProjectRepo, ProjectRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IIdentityService, IdentityServices>();
            services.AddScoped<IEmailRepo, EmailRepo>();
            services.AddScoped<ISeedService, SeedDB>();
            services.AddScoped<ITaskRepo, TaskRepo>();
            services.AddScoped<IPermissionRepo, PermissionRepo>();
            services.AddScoped<ICustomRoleRepo, CustomRoleRepo>();
            services.AddScoped<IChannelRepo, ChannelRepo>();
            return services;
        } 
    }
}
