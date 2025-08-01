🚀 Base N Katman Project Template
!.NET 8.0+ !Clean Architecture !EF Core 7.0 !MIT License

BaseNKatmanProject.Template, ASP.NET Core ve Clean Architecture prensipleriyle geliştirilmiş, üretime hazır 4 katmanlı profesyonel bir proje şablonudur.

📌 Temel Özellikler
✅ Clean Architecture: Sürdürülebilir ve test edilebilir yazılım mimarisi.
🚀 ASP.NET Core Web API: Modern ve hızlı API geliştirme altyapısı.
🗃️ Entity Framework Core: Code-First yaklaşımı ve Migration desteği.
🧱 Repository & Unit of Work: Veri erişimini soyutlayan tasarım desenleri.
🔄 AutoMapper: DTO ↔ Entity dönüşümleri için otomasyon.
📋 Serilog: Gelişmiş loglama altyapısı.
🧬 Generic Service & Repository: Tekrar eden kodları ortadan kaldırır.
🕵️ Audit Log: Veritabanı işlemlerinin geçmiş kaydı.
👥 Role & User Management: ASP.NET Core Identity ile entegre kullanıcı yönetimi.
🧹 Soft Delete: Silinen verileri fiziksel olarak kaldırmadan işaretleme.
📂 Proje Katmanları
Proje, sorumlulukların ayrılması ilkesine göre 4 ana katmandan oluşur:

📦 BaseNKatmanProject.Template
┣ 📂 BaseNKatmanProject.API           → API Katmanı (Controller, Endpoint, Middleware)
┣ 📂 BaseNKatmanProject.Application    → Uygulama Katmanı (Servisler, DTO, Mapping, Validasyon)
┣ 📂 BaseNKatmanProject.Core           → Çekirdek Katman (Entity, Interface, Ortak Yapılar)
┗ 📂 BaseNKatmanProject.Infrastructure → Altyapı Katmanı (EF Core, Repository, Identity, Logging)
⚙️ Başlarken
1️⃣ Şablonu Yükleme

2️⃣ Yeni Proje Oluşturma

-n parametresi ile projenize isim verebilirsiniz. Oluşturulan proje .git içermez, kendi Git reposunuzu başlatmak için git init komutunu kullanabilirsiniz.

🛠️ Projeyi Çalıştırma
🔄 Bağımlılıkları Yükleme

🗃️ Veritabanı Kurulumu

▶️ API'yi Başlatma

🎯 Visual Studio ile Kullanım
Visual Studio'da Yeni Proje Oluştur ekranını açın.
Base N Katman Project Template şablonunu seçin.
Proje adını girin ve Oluştur butonuna tıklayın.
Şablon görünmüyorsa Visual Studio’yu yeniden başlatmayı deneyin.

💡 İpuçları ve Öneriler
🔄 AutoMapper: Yeni eşlemeler için Application/Mapping klasörünü genişletin.
🕵️ Audit Log: Tüm değişiklikler AuditLog tablosunda otomatik olarak izlenir.
🧬 Generic Yapılar: CRUD işlemleri için GenericRepository ve GenericService sınıflarını kullanın.
📷 Ekran Görüntüleri
Buraya proje oluşturma ekranı, Swagger UI veya örnek API çıktılarının görsellerini ekleyebilirsiniz.

🤝 Katkıda Bulunma
Katkılarınızı bekliyoruz! 👇


Sonrasında bir Pull Request açmanız yeterli. 🎉

📄 Lisans
Bu proje MIT Lisansı ile lisanslanmıştır. Dilediğiniz gibi kullanabilir, geliştirebilir ve paylaşabilirsiniz.

💬 İletişim
Soru ve önerileriniz için GitHub Profilim üzerinden bana ulaşabilirsiniz.
