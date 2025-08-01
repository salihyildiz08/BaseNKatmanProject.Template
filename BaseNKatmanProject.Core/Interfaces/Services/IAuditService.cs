using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BaseNKatmanProject.Core.Interfaces.Services
{
    public interface IAuditService
    {
        Task AddAuditLogAsync(EntityEntry entry);

        Task LogAsync(string tabloAdi, Guid kayitId, string islemTipi, object eskiDegerler, object yeniDegerler);
    }
}
