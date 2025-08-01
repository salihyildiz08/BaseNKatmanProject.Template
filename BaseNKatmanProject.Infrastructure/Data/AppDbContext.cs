using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces.Services;
using BaseNKatmanProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseNKatmanProject.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        // DbSet'ler
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SerilogLog> SerilogLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            var now = DateTime.UtcNow;
            var currentUserId = _currentUserService?.UserId ?? Guid.Empty;
            var currentUserName = _currentUserService?.UserName ?? "system";

            // ChangeTracker snapshot al
            var entries = ChangeTracker.Entries<BaseEntity>().ToList();
            var pendingAudits = new List<AuditLog>(); // Audit kayıtlarını burada toplayacağız

            foreach (var entry in entries)
            {
                if (entry.Entity is AuditLog) // Audit tablosu kendini loglamasın
                    continue;

                var audit = new AuditLog
                {
                    TabloAdi = entry.Metadata.GetTableName(),
                    KayitId = entry.Property("Id").CurrentValue is Guid g ? g : Guid.NewGuid(),
                    IslemTipi = entry.State.ToString(),
                    IslemYapanKullanici = currentUserName,
                    IslemTarihi = now
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.OlusturmaTarihi = now;
                        entry.Entity.OlusturanKullaniciId = currentUserId;
                        entry.Entity.OlusturanKullaniciAdi = currentUserName;
                        entry.Entity.SilindiMi = false;

                        audit.YeniDegerler = JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                        break;

                    case EntityState.Modified:
                        entry.Entity.GuncellemeTarihi = now;
                        entry.Entity.GuncelleyenKullaniciId = currentUserId;
                        entry.Entity.GuncelleyenKullaniciAdi = currentUserName;

                        audit.EskiDegerler = JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                        audit.YeniDegerler = JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                        break;

                    case EntityState.Deleted:
                        // Soft delete yap
                        entry.State = EntityState.Modified;
                        entry.Entity.SilindiMi = true;
                        entry.Entity.SilmeTarihi = now;
                        entry.Entity.SilenKullaniciId = currentUserId;
                        entry.Entity.SilenKullaniciAdi = currentUserName;

                        audit.EskiDegerler = JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                        break;
                }

                pendingAudits.Add(audit); // Şimdilik sadece listeye ekle
            }

            // Foreach bittikten sonra audit kayıtlarını ekle
            if (pendingAudits.Any())
                AuditLogs.AddRange(pendingAudits);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity tablolarını Türkçeleştir
            modelBuilder.Entity<ApplicationUser>().ToTable("Kullanici");
            modelBuilder.Entity<ApplicationRole>().ToTable("Rol");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("KullaniciRol");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("KullaniciClaim");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("KullaniciGiris");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RolClaim");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("KullaniciToken");

            // AuditLog tablosu
            modelBuilder.Entity<AuditLog>().ToTable("DenetimKayitlari");

            // Otomatik tüm entity'leri konfigüre et (reflection ile)
            AutoConfigureEntities(modelBuilder);

            // Soft delete global filter tüm BaseEntity için uygula
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext).GetMethod(nameof(CreateIsDeletedFilter), BindingFlags.NonPublic | BindingFlags.Static);
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    var filter = genericMethod.Invoke(null, null);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter((LambdaExpression)filter);
                }
            }
        }

        // Generic soft delete filtresi
        private static LambdaExpression CreateIsDeletedFilter<TEntity>() where TEntity : BaseEntity
        {
            Expression<Func<TEntity, bool>> filter = e => !e.SilindiMi;
            return filter;
        }

        // Reflection ile otomatik entity konfigürasyonu
        private void AutoConfigureEntities(ModelBuilder modelBuilder)
        {
            var baseEntityType = typeof(BaseEntity);

            // BaseEntity'den türeyen tüm somut entity tiplerini bul
            var entityTypes = baseEntityType.Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && baseEntityType.IsAssignableFrom(t));

            foreach (var type in entityTypes)
            {
                var entityBuilder = modelBuilder.Entity(type);

                // PK varsayılanı: Id property'si varsa key olarak ayarla
                var idProp = type.GetProperty("Id");
                if (idProp != null)
                {
                    entityBuilder.HasKey("Id");
                }

                // string property'lere otomatik max length ve required ayarı
                var stringProps = type.GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                foreach (var prop in stringProps)
                {
                    entityBuilder.Property(prop.Name)
                        .HasMaxLength(200)
                        .IsRequired(false); // Gerektiğinde düzenle
                }

                // decimal tipinde price, amount vb. için örnek konfigürasyon
                var decimalProps = type.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));

                foreach (var prop in decimalProps)
                {
                    entityBuilder.Property(prop.Name).HasColumnType("decimal(18,2)");
                }

                // Buraya isteğe bağlı başka genel konfigürasyonlar ekleyebilirsin
            }
        }
    }
}
