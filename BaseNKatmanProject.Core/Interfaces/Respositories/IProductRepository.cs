using BaseNKatmanProject.Core.Entities;

namespace BaseNKatmanProject.Core.Interfaces.Respositories;
public interface IProductRepository : IGenericRepository<Product>
{
    Task<(IEnumerable<Product>, int totalCount)> GetPagedProductsAsync(int pageNumber, int pageSize);
}
