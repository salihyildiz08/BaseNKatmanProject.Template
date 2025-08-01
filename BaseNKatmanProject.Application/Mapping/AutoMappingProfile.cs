using AutoMapper;
using BaseNKatmanProject.Application.DTOs.Role;
using BaseNKatmanProject.Application.DTOs.User;
using BaseNKatmanProject.Infrastructure.Identity;
using System.Reflection;

namespace BaseNKatmanProject.Application.Mapping;
public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        // 1️⃣ Tüm ilgili assembly’leri tara
        var assemblies = new[]
        {
                Assembly.GetExecutingAssembly(),                  // Application (DTO'lar burada)
                Assembly.Load("BaseNKatmanProject.Core"),          // Core (Entity'ler burada)
                Assembly.Load("BaseNKatmanProject.Infrastructure") // Gerekirse
            };

        var allTypes = assemblies.SelectMany(a => a.GetTypes()).ToList();

        // 2️⃣ DTO ↔ Entity otomatik eşleme
        var dtoTypes = allTypes.Where(t => t.Name.EndsWith("Dto") && t.IsClass).ToList();
        var entityTypes = allTypes.Where(t => t.IsClass && !t.Name.EndsWith("Dto")).ToList();

        foreach (var dtoType in dtoTypes)
        {
            // CategoryDto → Category / CategoryCreateDto → Category / CategoryUpdateDto → Category
            var baseName = dtoType.Name
                .Replace("CreateDto", "")
                .Replace("UpdateDto", "")
                .Replace("Dto", "");

            var entityType = entityTypes.FirstOrDefault(t => t.Name == baseName);

            if (entityType != null)
            {
                CreateMap(entityType, dtoType).ReverseMap();
            }
        }

        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<UserCreateDto, ApplicationUser>().ReverseMap();
        CreateMap<UserUpdateDto, ApplicationUser>().ReverseMap();

        CreateMap<ApplicationRole, RoleDto>().ReverseMap();
        CreateMap<ApplicationRole, RoleCreateDto>().ReverseMap();
        CreateMap<ApplicationRole, RoleUpdateDto>().ReverseMap();
    }

}
