using System.Reflection;
using System.Runtime.CompilerServices;

namespace UserService.Helper
{
    public static class ServiceCollectionExtensionHelper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes()
             .Where(type => type.IsClass && !type.IsAbstract && type.Namespace == "UserService.Services" && !typeof(IAsyncStateMachine).IsAssignableFrom(type)
                   && !type.Name.Contains("<"));
            foreach (var implementationType in serviceTypes)
            {
                var interfaceType = implementationType.GetInterfaces().FirstOrDefault();
                if (interfaceType != null)
                {
                    services.AddTransient(interfaceType, implementationType);
                }
            }
            return services;
        }
    }
}
