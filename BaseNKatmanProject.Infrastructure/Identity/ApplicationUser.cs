using Microsoft.AspNetCore.Identity;

namespace BaseNKatmanProject.Infrastructure.Identity;
public class ApplicationUser : IdentityUser<Guid>
{
    // 👤 Ek kullanıcı bilgileri
    public string Ad { get; set; }
    public string Soyad { get; set; }

    // 🕓 Audit bilgileri
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
    public string? OlusturanKullanici { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }
    public string? GuncelleyenKullanici { get; set; }

    public bool SilindiMi { get; set; } = false;
}