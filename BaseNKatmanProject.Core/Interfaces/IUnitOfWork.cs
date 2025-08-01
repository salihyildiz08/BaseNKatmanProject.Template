using BaseNKatmanProject.Core.Interfaces.Respositories;

namespace BaseNKatmanProject.Core.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;

    Task<int> CommitAsync();
}