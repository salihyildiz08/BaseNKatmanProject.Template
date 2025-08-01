using BaseNKatmanProject.Core.Interfaces;
using BaseNKatmanProject.Core.Interfaces.Respositories;
using BaseNKatmanProject.Core.Interfaces.Services;
using BaseNKatmanProject.Infrastructure.Data;
using BaseNKatmanProject.Infrastructure.Repositories;
using System.Collections;

namespace BaseNKatmanProject.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;
        private Hashtable _repositories;

        public UnitOfWork(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                // AuditService destekli repository oluşturuyoruz
                var repositoryInstance = new GenericRepository<T>(_context, _auditService);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
