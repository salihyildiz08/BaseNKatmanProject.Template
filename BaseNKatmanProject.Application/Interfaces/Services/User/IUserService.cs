using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Infrastructure.Identity;

namespace BaseNKatmanProject.Application.Interfaces.Services.User;
public interface IUserService : IGenericService<ApplicationUser>
{
    Task<ResponseMessage<bool>> ChangePasswordAsync(Guid id, string newPassword);
    Task<ResponseMessage<IList<string>>> GetRolesAsync(Guid userId);
    Task<ResponseMessage<bool>> AddToRoleAsync(Guid userId, string roleName);
    Task<ResponseMessage<bool>> RemoveFromRoleAsync(Guid userId, string roleName);

    Task<ResponseMessage<ApplicationUser>> UpdateAsync(ApplicationUser entity);
    Task<ResponseMessage<ApplicationUser>> RemoveAsync(ApplicationUser entity);
}

