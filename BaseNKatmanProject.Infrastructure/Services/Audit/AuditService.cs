using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces.Services;
using BaseNKatmanProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace BaseNKatmanProject.Infrastructure.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AuditService(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task AddAuditLogAsync(EntityEntry entry)
        {
            var tableName = entry.Metadata.GetTableName();
            var keyName = entry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name;

            if (keyName == null)
                throw new Exception("Entity primary key bulunamadı.");

            var keyValue = entry.Property(keyName).CurrentValue?.ToString();

            switch (entry.State)
            {
                case EntityState.Added:
                    await LogAsync(
                        tableName ?? "Bilinmiyor",
                        Guid.TryParse(keyValue, out var parsedId) ? parsedId : Guid.NewGuid(),
                        "CREATE",
                        null,
                        entry.CurrentValues.ToObject());
                    break;

                case EntityState.Modified:
                    await LogAsync(
                        tableName ?? "Bilinmiyor",
                        Guid.TryParse(keyValue, out parsedId) ? parsedId : Guid.NewGuid(),
                        "UPDATE",
                        entry.OriginalValues.ToObject(),
                        entry.CurrentValues.ToObject());
                    break;

                case EntityState.Deleted:
                    await LogAsync(
                        tableName ?? "Bilinmiyor",
                        Guid.TryParse(keyValue, out parsedId) ? parsedId : Guid.NewGuid(),
                        "DELETE",
                        entry.OriginalValues.ToObject(),
                        null);
                    break;
            }
        }

        public async Task LogAsync(string tabloAdi, Guid kayitId, string islemTipi, object eskiDegerler, object yeniDegerler)
        {
            var audit = new AuditLog
            {
                TabloAdi = tabloAdi,
                KayitId = kayitId,
                IslemTipi = islemTipi,
                IslemYapanKullanici = _currentUserService.UserId?.ToString() ?? "Anonim",
                IslemTarihi = DateTime.UtcNow,
                EskiDegerler = eskiDegerler != null ? JsonConvert.SerializeObject(eskiDegerler) : null,
                YeniDegerler = yeniDegerler != null ? JsonConvert.SerializeObject(yeniDegerler) : null,
            };

            await _context.AuditLogs.AddAsync(audit);
            await _context.SaveChangesAsync();
        }
    }
}
