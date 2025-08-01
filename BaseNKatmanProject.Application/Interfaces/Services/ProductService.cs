using BaseNKatmanProject.Application.Services;
using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Core.Interfaces.Services;

namespace BaseNKatmanProject.Application.Interfaces.Services;
public class ProductService : GenericService<Product>, IProductService
{
    public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

}
