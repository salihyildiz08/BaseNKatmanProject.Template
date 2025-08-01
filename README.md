# 🚀 Base N Katman Project Template

![.NET 8.0+](https://img.shields.io/badge/.NET%208.0%2B-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-✔-green?style=for-the-badge)
![EF Core 7.0](https://img.shields.io/badge/EF%20Core-7.0-blue?style=for-the-badge&logo=nuget)
![MIT License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)

BaseNKatmanProject.Template, **ASP.NET Core** ve **Clean Architecture** prensipleriyle geliştirilmiş, üretime hazır **4 katmanlı profesyonel** bir proje şablonudur.

---

## 📌 Temel Özellikler

- ✅ **Clean Architecture**: Sürdürülebilir ve test edilebilir yazılım mimarisi.
- 🚀 **ASP.NET Core Web API**: Modern ve hızlı API geliştirme altyapısı.
- 🗃️ **Entity Framework Core**: Code-First yaklaşımı ve Migration desteği.
- 🧱 **Repository & Unit of Work**: Veri erişimini soyutlayan tasarım desenleri.
- 🔄 **AutoMapper**: DTO ↔ Entity dönüşümleri için otomasyon.
- 📋 **Serilog**: Gelişmiş loglama altyapısı.
- 🧬 **Generic Service & Repository**: Tekrar eden kodları ortadan kaldırır.
- 🕵️ **Audit Log**: Veritabanı işlemlerinin geçmiş kaydı.
- 👥 **Role & User Management**: ASP.NET Core Identity ile entegre kullanıcı yönetimi.
- 🧹 **Soft Delete**: Silinen verileri fiziksel olarak kaldırmadan işaretleme.

---

## 📂 Proje Katmanları

Proje, **sorumlulukların ayrılması** ilkesine göre **4 ana katmandan** oluşur:

📦 BaseNKatmanProject.Template
┣ 📂 BaseNKatmanProject.API → API Katmanı (Controller, Endpoint, Middleware)
┣ 📂 BaseNKatmanProject.Application → Uygulama Katmanı (Servisler, DTO, Mapping, Validasyon)
┣ 📂 BaseNKatmanProject.Core → Çekirdek Katman (Entity, Interface, Ortak Yapılar)
┗ 📂 BaseNKatmanProject.Infrastructure→ Altyapı Katmanı (EF Core, Repository, Identity, Logging)

yaml
Kopyala
Düzenle

---

## ⚙️ Başlarken

### 1️⃣ Şablonu Yükleme

```bash
dotnet new --install BaseNKatmanProject.Template
2️⃣ Yeni Proje Oluşturma
bash
Kopyala
Düzenle
dotnet new basenkatman -n ProjeAdi
-n parametresi ile projenize isim verebilirsiniz.
Oluşturulan proje .git içermez, kendi Git reposunuzu başlatmak için:

bash
Kopyala
Düzenle
git init
🛠️ Projeyi Çalıştırma
🔄 Bağımlılıkları Yükleme
bash
Kopyala
Düzenle
dotnet restore
🗃️ Veritabanı Kurulumu
bash
Kopyala
Düzenle
dotnet ef database update
▶️ API'yi Başlatma
bash
Kopyala
Düzenle
dotnet run
🎯 Visual Studio ile Kullanım
Yeni Proje Oluştur ekranını açın.

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
Katkılarınızı bekliyoruz!
Fork'layın, geliştirin ve bir Pull Request açın. 🎉

📄 Lisans
Bu proje MIT Lisansı ile lisanslanmıştır.
Dilediğiniz gibi kullanabilir, geliştirebilir ve paylaşabilirsiniz.

💬 İletişim
Soru ve önerileriniz için GitHub Profilim üzerinden bana ulaşabilirsiniz.

yaml
Kopyala
Düzenle

---

Eğer istersen ben bunun **GitHub README.md**’si olarak görsel, renkli rozetler ve modern başlık yapısıyla **daha profesyonel** bir versiyonunu da hazırlayabilirim.  
İster misin ben onu da yapayım?
