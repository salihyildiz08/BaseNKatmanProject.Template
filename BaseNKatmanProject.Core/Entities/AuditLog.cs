using System.ComponentModel.DataAnnotations.Schema;

namespace BaseNKatmanProject.Core.Entities
{
    public class AuditLog : BaseEntity
    {
        [Column("TabloAdi")]
        public string TabloAdi { get; set; }

        [Column("KayitId")]
        public Guid KayitId { get; set; }

        [Column("IslemTipi")]
        public string? IslemTipi { get; set; }

        [Column("EskiDegerler", TypeName = "nvarchar(max)")]
        public string? EskiDegerler { get; set; }

        [Column("YeniDegerler", TypeName = "nvarchar(max)")]
        public string? YeniDegerler { get; set; }

        [Column("IslemYapanKullanici")]
        public string? IslemYapanKullanici { get; set; }

        [Column("IslemTarihi")]
        public DateTime IslemTarihi { get; set; } = DateTime.UtcNow;
    }
}
