using BaseNKatmanProject.Core.Commons;
using System.Linq.Expressions;

namespace BaseNKatmanProject.Core.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<ResponseMessage<T>> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);

        Task<ResponseMessage<IEnumerable<T>>> GetAllAsync(
            PageParameters pageParameters = null,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<ResponseMessage<IEnumerable<T>>> FindAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes);

        Task<ResponseMessage<T>> AddAsync(T entity);

        Task<ResponseMessage<T>> UpdateAsync(T entity);

        Task<ResponseMessage<T>> RemoveAsync(T entity);

        Task<int> SaveChangesAsync();
    }
}
