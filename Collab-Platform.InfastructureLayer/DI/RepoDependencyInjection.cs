using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.InfastructureLayer.Database;
using Collab_Platform.InfastructureLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Collab_Platform.InfastructureLayer.DI
{
    public static class RepoDependencyInjection
    {
        public static IServiceCollection RepoDI(this IServiceCollection services) {
            services.AddScoped<IProjectRepo, ProjectRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IEmailRepo, EmailRepo>();
            services.AddScoped<ISeedService, SeedDB>();
            services.AddScoped<ITaskRepo, TaskRepo>();
            services.AddScoped<IPermissionRepo, PermissionRepo>();
            return services;
        } 
    }
}
