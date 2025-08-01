using BaseNKatmanProject.Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseNKatmanProject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AutoMappingProfile>();
        });

        // FluentValidation Validatorları otomatik ekle
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Tüm servis arayüzlerini ve implementasyonlarını otomatik ekle
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            // Sadece interface isimleri 'I' ile başlayan ve implementasyonu var ise
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
