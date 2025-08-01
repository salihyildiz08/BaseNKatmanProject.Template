using BaseNKatmanProject.Application.Services;
using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Core.Interfaces.Services;

namespace BaseNKatmanProject.Application.Interfaces.Services;
public class CategoryService : GenericService<Category>, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

}