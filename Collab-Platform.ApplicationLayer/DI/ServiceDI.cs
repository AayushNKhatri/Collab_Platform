using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.ApplicationLayer.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.ApplicationLayer.DI
{
    public static class ServiceDI
    {
        public static IServiceCollection ServiceDependencyInjecttion(this IServiceCollection services) {
            services.AddScoped<IProjectInterface, ProjectService>();
            services.AddScoped<IUserInterface, UserService>();
            return services;
        }

    }
}
