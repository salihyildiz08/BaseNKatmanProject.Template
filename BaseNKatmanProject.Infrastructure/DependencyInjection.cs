using BaseNKatmanProject.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseNKatmanProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, Services.CurrentUser.CurrentUserService>();


            // Scrutor ile otomatik servis kaydı
            services.Scan(scan => scan
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.Where(type =>
                    type.Name.EndsWith("Service") ||
                    type.Name.EndsWith("Repository") ||
                    type.Name.EndsWith("UnitOfWork")))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }
    }
}
