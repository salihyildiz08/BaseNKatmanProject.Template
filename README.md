🚀 Base N Katman Project Template
![alt text](https://img.shields.io/badge/.NET%207.0%2B-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![alt text](https://img.shields.io/badge/Clean%20Architecture-✔-green?style=for-the-badge)
![alt text](https://img.shields.io/badge/Entity%20Framework%20Core-7.0-blue?style=for-the-badge)
![alt text](https://img.shields.io/badge/License-MIT-orange?style=for-the-badge)
BaseNKatmanProject.Template, ASP.NET Core üzerinde Clean Architecture prensipleriyle geliştirilmiş, üretime hazır 4 katmanlı profesyonel bir proje şablonudur.
📌 Temel Özellikler
Clean Architecture: Sürdürülebilir ve test edilebilir bir yazılım için prensiplere uygun yapı.
ASP.NET Core Web API: Modern ve hızlı API'lar oluşturmak için hazır proje.
Entity Framework Core: Code-First yaklaşımı ve Migration desteği ile güçlü veritabanı yönetimi.
Repository & Unit of Work: Veri erişimini soyutlayan ve yöneten tasarım desenleri.
AutoMapper: DTO ve Entity sınıfları arasında otomatik dönüşüm.
Serilog: Gelişmiş ve yapılandırılabilir loglama altyapısı.
Generic Service & Repository: Tekrar eden kodları ortadan kaldıran jenerik yapılar.
Audit Log: Veritabanında yapılan ekleme, güncelleme ve silme işlemlerinin geçmiş kaydı.
Role & User Management: ASP.NET Core Identity ile hazır rol ve kullanıcı yönetimi.
Soft Delete: Verileri kalıcı olarak silmek yerine "silindi" olarak işaretleme desteği.
📂 Proje Katmanları
Proje, sorumlulukların ayrılması ilkesine göre dört ana katmandan oluşur:
Generated code
📦 BaseNKatmanProject.Template
┣ 📂 BaseNKatmanProject.API           → API katmanı (Controller'lar, Endpoint'ler, Middleware'ler)
┣ 📂 BaseNKatmanProject.Application    → İş mantığı katmanı (Servisler, DTO'lar, Mapping, Validasyon)
┣ 📂 BaseNKatmanProject.Core           → Çekirdek katman (Entity modelleri, Soyutlamalar/Interface'ler)
┗ 📂 BaseNKatmanProject.Infrastructure → Altyapı katmanı (EF Core, Repository, Identity, Logging)
Use code with caution.
🚀 Başlarken
Adım 1: Şablonu Yükleme
Aşağıdaki komutu kullanarak proje şablonunu sisteminize yükleyin:
Generated bash
dotnet new install https://github.com/salihyildiz08/BaseNKatmanProject.Template.git
Use code with caution.
Bash
Adım 2: Yeni Proje Oluşturma
Şablonu kullanarak yeni bir proje oluşturun:
Generated bash
dotnet new basenkatman -n MyERPProject
Use code with caution.
Bash
Not: -n parametresi ile projenize dilediğiniz ismi verebilirsiniz. Yeni oluşturulan proje .git içermez. git init komutuyla kendi Git reponuzu başlatabilirsiniz.
📜 Yeni Projeyi Çalıştırma
Bağımlılıkları Yükleme
Proje dizinine giderek gerekli NuGet paketlerini yükleyin:
Generated bash
cd MyERPProject
dotnet restore
Use code with caution.
Bash
Veritabanı Kurulumu
Entity Framework Core migration'larını oluşturun ve veritabanına uygulayın:
Generated bash
# Migration oluşturma
dotnet ef migrations add InitialCreate -p MyERPProject.Infrastructure -s MyERPProject.API

# Veritabanını güncelleme
dotnet ef database update -p MyERPProject.Infrastructure -s MyERPProject.API
Use code with caution.
Bash
Projeyi Çalıştırma
API projesini başlatın:
Generated bash
dotnet run --project MyERPProject.API
Use code with caution.
Bash
🎯 Visual Studio ile Kullanım
Bu şablon, kurulum sonrası Visual Studio'nun Yeni Proje Oluştur penceresinde görünür.
Yeni Proje Oluştur ekranında Base N Katman Project Template şablonunu seçin.
Proje Adı alanına projenizin ismini yazın.
Oluştur butonuna tıklayın.
İpucu: Eğer şablon listede görünmüyorsa, Visual Studio'yu kapatıp yeniden açmayı deneyin.
💡 İpuçları ve Öneriler
AutoMapper: Yeni Entity/DTO eşlemeleri eklemek için Application katmanındaki Mapping profillerini genişletebilirsiniz.
Audit Log: AuditLog tablosu, veritabanında yapılan tüm değişiklikleri otomatik olarak izler ve kaydeder.
Generic Sınıflar: GenericRepository ve GenericService sınıflarını kullanarak standart CRUD işlemleri için tekrar eden kod yazmaktan kaçının.
📷 Ekran Görüntüleri
README dosyanızı daha çekici hale getirmek için buraya proje oluşturma ekranı veya API Swagger arayüzü gibi görseller ekleyebilirsiniz.
🤝 Katkıda Bulunma
Katkılarınız projeyi daha iyi hale getirecektir!
Bu repoyu fork'layın.
Yeni bir branch oluşturun (git checkout -b feature/yeni-ozellik).
Değişikliklerinizi commit'leyin (git commit -m "Yeni özellik eklendi").
Branch'inizi push'layın (git push origin feature/yeni-ozellik).
Bir Pull Request açın. 🎉
📄 Lisans
Bu proje, MIT Lisansı ile lisanslanmıştır. Detaylar için LICENSE dosyasına göz atın. Projeyi dilediğiniz gibi kullanabilir, geliştirebilir ve paylaşabilirsiniz.
💬 İletişim
Soru ve önerileriniz için GitHub Profilim üzerinden bana ulaşabilirsiniz.
