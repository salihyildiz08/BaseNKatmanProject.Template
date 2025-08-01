using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Infrastructure.Identity;

namespace BaseNKatmanProject.Application.Interfaces.Services.Role;
public interface IRoleService : IGenericService<ApplicationRole>
{
    Task<ResponseMessage<ApplicationRole>> UpdateAsync(ApplicationRole entity);
    Task<ResponseMessage<ApplicationRole>> RemoveAsync(ApplicationRole entity);
}