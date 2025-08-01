using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Core.Interfaces.Respositories;
using System.Linq.Expressions;

namespace BaseNKatmanProject.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _repository;

        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<T>();
        }

        public async Task<ResponseMessage<T>> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetByIdAsync(id, includes);
        }

        public async Task<ResponseMessage<IEnumerable<T>>> GetAllAsync(
            PageParameters pageParameters = null,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetAllAsync(pageParameters, filter, orderBy, includes);
        }

        public async Task<ResponseMessage<IEnumerable<T>>> FindAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes)
        {
            return await _repository.FindAsync(filter, includes);
        }

        public virtual async Task<ResponseMessage<T>> AddAsync(T entity)
        {
            var result = await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return result;
        }

        public virtual async Task<ResponseMessage<T>> UpdateAsync(T entity)
        {
            var result = await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return result;
        }

        public virtual async Task<ResponseMessage<T>> RemoveAsync(T entity)
        {
            var result = await _repository.RemoveAsync(entity);
            await _repository.SaveChangesAsync();
            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }
    }
}
