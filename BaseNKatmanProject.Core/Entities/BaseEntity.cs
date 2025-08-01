using System.ComponentModel.DataAnnotations.Schema;

namespace BaseNKatmanProject.Core.Entities;
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("OlusturmaTarihi")]
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

    [Column("OlusturanKullaniciId")]
    public Guid OlusturanKullaniciId { get; set; }

    [Column("OlusturanKullaniciAdi")]
    public string? OlusturanKullaniciAdi { get; set; }

    [Column("GuncellemeTarihi")]
    public DateTime? GuncellemeTarihi { get; set; }

    [Column("GuncelleyenKullaniciId")]
    public Guid? GuncelleyenKullaniciId { get; set; }

    [Column("GuncelleyenKullaniciAdi")]
    public string? GuncelleyenKullaniciAdi { get; set; }

    [Column("SilmeTarihi")]
    public DateTime? SilmeTarihi { get; set; }

    [Column("SilenKullaniciId")]
    public Guid? SilenKullaniciId { get; set; }

    [Column("SilenKullaniciAdi")]
    public string? SilenKullaniciAdi { get; set; }

    [Column("SilindiMi")]
    public bool SilindiMi { get; set; } = false;
}