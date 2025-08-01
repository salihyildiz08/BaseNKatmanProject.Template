using BaseNKatmanProject.Application.Interfaces.Services.Role;
using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Infrastructure.Identity;

namespace BaseNKatmanProject.Application.Services
{
    public class RoleService : GenericService<ApplicationRole>, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResponseMessage<ApplicationRole>> AddAsync(ApplicationRole entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Name))
                entity.NormalizedName = entity.Name.ToUpperInvariant();

            return await base.AddAsync(entity);
        }

        public override async Task<ResponseMessage<ApplicationRole>> UpdateAsync(ApplicationRole entity)
        {
            // Var olan rolü getirip kontrol et
            var existingRoleResponse = await GetByIdAsync(entity.Id);
            if (!existingRoleResponse.Success || existingRoleResponse.Data.SilindiMi)
                return ResponseMessage<ApplicationRole>.Failure("Rol bulunamadı.");

            var existingRole = existingRoleResponse.Data;

            existingRole.Name = entity.Name;
            existingRole.NormalizedName = !string.IsNullOrWhiteSpace(entity.Name)
                ? entity.Name.ToUpperInvariant()
                : existingRole.NormalizedName;

            // Base sınıftaki UpdateAsync'i çağır
            return await base.UpdateAsync(existingRole);
        }

        public override async Task<ResponseMessage<ApplicationRole>> RemoveAsync(ApplicationRole entity)
        {
            var existingRoleResponse = await GetByIdAsync(entity.Id);
            if (!existingRoleResponse.Success || existingRoleResponse.Data.SilindiMi)
                return ResponseMessage<ApplicationRole>.Failure("Rol bulunamadı.");

            var existingRole = existingRoleResponse.Data;

            // Soft delete uygulama
            existingRole.SilindiMi = true;

            return await base.UpdateAsync(existingRole);
        }
    }
}
