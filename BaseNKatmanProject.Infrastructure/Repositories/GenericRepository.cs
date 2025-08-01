using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Interfaces.Respositories;
using BaseNKatmanProject.Core.Interfaces.Services;
using BaseNKatmanProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseNKatmanProject.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IAuditService _auditService;

        public GenericRepository(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
            _dbSet = context.Set<T>();
        }

        public async Task<ResponseMessage<T>> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

                if (entity == null)
                    return ResponseMessage<T>.Failure("Kayıt bulunamadı.");

                return ResponseMessage<T>.SuccessResult(entity);
            }
            catch (Exception ex)
            {
                return ResponseMessage<T>.Failure($"GetByIdAsync hatası: {ex.Message}");
            }
        }

        public async Task<ResponseMessage<IEnumerable<T>>> GetAllAsync(
            PageParameters pageParameters = null,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                // Soft delete varsa SilindiMi == false filtresi uygula
                var silindiMiProp = typeof(T).GetProperty("SilindiMi");
                if (silindiMiProp != null)
                {
                    query = query.Where(e => EF.Property<bool>(e, "SilindiMi") == false);
                }

                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                    query = orderBy(query);

                // Pagination uygulanacak mı kontrol et
                if (pageParameters != null)
                {
                    pageParameters.Validate();

                    query = query
                        .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                        .Take(pageParameters.PageSize);
                }

                var list = await query.ToListAsync();

                return ResponseMessage<IEnumerable<T>>.SuccessResult(list);
            }
            catch (Exception ex)
            {
                return ResponseMessage<IEnumerable<T>>.Failure($"GetAllAsync hatası: {ex.Message}");
            }
        }

        public async Task<ResponseMessage<IEnumerable<T>>> FindAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                    query = query.Include(include);

                query = query.Where(filter);

                // Soft delete varsa SilindiMi == false filtresi uygula
                var silindiMiProp = typeof(T).GetProperty("SilindiMi");
                if (silindiMiProp != null)
                {
                    query = query.Where(e => EF.Property<bool>(e, "SilindiMi") == false);
                }

                var list = await query.ToListAsync();
                return ResponseMessage<IEnumerable<T>>.SuccessResult(list);
            }
            catch (Exception ex)
            {
                return ResponseMessage<IEnumerable<T>>.Failure($"FindAsync hatası: {ex.Message}");
            }
        }

        public async Task<ResponseMessage<T>> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                // Audit Log CREATE
                var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity);
                if (idProp is Guid idGuid)
                    await _auditService.LogAsync(typeof(T).Name, idGuid, "CREATE", null, entity);

                return ResponseMessage<T>.SuccessResult(entity, "Başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return ResponseMessage<T>.Failure($"Ekleme hatası: {ex.Message}");
            }
        }

        public async Task<ResponseMessage<T>> UpdateAsync(T entity)
        {
            try
            {
                var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity);
                if (idProp == null)
                    return ResponseMessage<T>.Failure("Güncelleme için Id bulunamadı.");

                var idGuid = (Guid)idProp;

                var oldEntity = await _dbSet.AsNoTracking()
                    .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == idGuid);

                if (oldEntity == null)
                    return ResponseMessage<T>.Failure("Güncellenecek kayıt bulunamadı.");

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                // Audit Log UPDATE
                await _auditService.LogAsync(typeof(T).Name, idGuid, "UPDATE", oldEntity, entity);

                return ResponseMessage<T>.SuccessResult(entity, "Başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return ResponseMessage<T>.Failure($"Güncelleme hatası: {ex.Message}");
            }
        }

        public async Task<ResponseMessage<T>> RemoveAsync(T entity)
        {
            try
            {
                var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity);
                if (idProp == null)
                    return ResponseMessage<T>.Failure("Silme için Id bulunamadı.");

                var idGuid = (Guid)idProp;

                // Soft Delete varsa
                var silindiMiProp = entity.GetType().GetProperty("SilindiMi");
                if (silindiMiProp != null)
                {
                    silindiMiProp.SetValue(entity, true);
                    _dbSet.Update(entity);
                }
                else
                {
                    _dbSet.Remove(entity);
                }

                await _context.SaveChangesAsync();

                // Audit Log DELETE
                await _auditService.LogAsync(typeof(T).Name, idGuid, "DELETE", entity, null);

                return ResponseMessage<T>.SuccessResult(entity, "Başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return ResponseMessage<T>.Failure($"Silme hatası: {ex.Message}");
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
