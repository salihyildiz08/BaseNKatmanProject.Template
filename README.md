# 🚀 Base N Katman Project Template

<div align="center">

![Project Logo](https://img.shields.io/badge/🏗️-BaseN%20Katman-blue?style=for-the-badge&logoColor=white)

**Modern, Clean Architecture ile geliştirilmiş profesyonel .NET proje şablonu**

[![.NET](https://img.shields.io/badge/.NET%208.0+-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-✔-4CAF50?style=for-the-badge)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![EF Core](https://img.shields.io/badge/EF%20Core-7.0+-FF6F00?style=for-the-badge&logo=nuget)](https://docs.microsoft.com/en-us/ef/core/)
[![MIT License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

[![Stars](https://img.shields.io/github/stars/yourusername/BaseNKatmanProject.Template?style=social)](https://github.com/yourusername/BaseNKatmanProject.Template/stargazers)
[![Forks](https://img.shields.io/github/forks/yourusername/BaseNKatmanProject.Template?style=social)](https://github.com/yourusername/BaseNKatmanProject.Template/network/members)
[![Issues](https://img.shields.io/github/issues/yourusername/BaseNKatmanProject.Template?style=social)](https://github.com/yourusername/BaseNKatmanProject.Template/issues)

</div>

---

## 🎯 Nedir Bu Proje?

**BaseNKatmanProject.Template**, modern yazılım geliştirme standartlarına uygun, **Clean Architecture** prensipleriyle tasarlanmış, ölçeklenebilir ve sürdürülebilir **4 katmanlı** bir .NET proje şablonudur. 

Hem bireysel geliştiriciler hem de enterprise projeler için **üretime hazır** bir temel sağlar.

## ✨ Öne Çıkan Özellikler

<table>
<tr>
<td align="center">
  <img src="https://img.shields.io/badge/🏗️-Clean%20Architecture-blue?style=flat-square" alt="Clean Architecture"/>
  <br><strong>Clean Architecture</strong>
  <br>Sürdürülebilir ve test edilebilir
</td>
<td align="center">
  <img src="https://img.shields.io/badge/⚡-High%20Performance-green?style=flat-square" alt="Performance"/>
  <br><strong>Yüksek Performans</strong>
  <br>Optimized kod yapısı
</td>
<td align="center">
  <img src="https://img.shields.io/badge/🔄-Auto%20Mapping-orange?style=flat-square" alt="AutoMapper"/>
  <br><strong>AutoMapper</strong>
  <br>Otomatik DTO dönüşümleri
</td>
</tr>
<tr>
<td align="center">
  <img src="https://img.shields.io/badge/🗃️-EF%20Core-purple?style=flat-square" alt="EF Core"/>
  <br><strong>Entity Framework</strong>
  <br>Code-First yaklaşımı
</td>
<td align="center">
  <img src="https://img.shields.io/badge/👥-Identity%20System-red?style=flat-square" alt="Identity"/>
  <br><strong>Kullanıcı Yönetimi</strong>
  <br>Role-based authentication
</td>
<td align="center">
  <img src="https://img.shields.io/badge/📋-Advanced%20Logging-yellow?style=flat-square" alt="Logging"/>
  <br><strong>Gelişmiş Loglama</strong>
  <br>Serilog entegrasyonu
</td>
</tr>
</table>

### 🔥 Teknik Özellikler

- 🚀 **ASP.NET Core Web API** - Modern REST API geliştirme
- 🗃️ **Entity Framework Core** - Güçlü ORM desteği
- 🧱 **Repository & Unit of Work** patterns
- 🔄 **AutoMapper** - Otomatik nesne eşlemeleri  
- 📋 **Serilog** - Yapılandırılabilir loglama sistemi
- 🧬 **Generic Service & Repository** - DRY prensibi
- 🕵️ **Audit Logging** - Değişiklik geçmişi takibi
- 👥 **ASP.NET Core Identity** - Kullanıcı ve rol yönetimi
- 🧹 **Soft Delete** - Güvenli silme işlemleri
- 📊 **Swagger/OpenAPI** - Otomatik API dokümantasyonu

## 📁 Proje Mimarisi

```
📦 BaseNKatmanProject.Template/
├── 🌐 BaseNKatmanProject.API/              # Presentation Layer
│   ├── Controllers/                        # API Controllers
│   ├── Middleware/                         # Custom Middlewares
│   ├── Extensions/                         # Service Extensions
│   └── Program.cs                          # Application Entry Point
│
├── 🚀 BaseNKatmanProject.Application/      # Application Layer  
│   ├── Services/                           # Business Logic Services
│   ├── DTOs/                              # Data Transfer Objects
│   ├── Mapping/                           # AutoMapper Profiles
│   ├── Interfaces/                        # Service Interfaces
│   └── Validations/                       # Input Validations
│
├── 💎 BaseNKatmanProject.Core/             # Domain Layer
│   ├── Entities/                          # Domain Models
│   ├── Enums/                             # Enumerations
│   ├── Interfaces/                        # Repository Interfaces
│   └── Common/                            # Shared Components
│
└── 🔧 BaseNKatmanProject.Infrastructure/   # Infrastructure Layer
    ├── Data/                              # DbContext & Configurations
    ├── Repositories/                      # Data Access Layer
    ├── Identity/                          # Authentication & Authorization
    ├── Logging/                           # Serilog Configuration
    └── Extensions/                        # Infrastructure Services
```

## 🚀 Hızlı Başlangıç

### 📋 Önkoşullar

- ✅ [.NET 8.0 SDK](https://dotnet.microsoft.com/download) veya üzeri
- ✅ [SQL Server](https://www.microsoft.com/sql-server) (LocalDB desteklenir)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) veya [VS Code](https://code.visualstudio.com/)

### 1️⃣ Şablonu Yükleme

```bash
# NuGet'ten şablonu yükleyin
dotnet new install BaseNKatmanProject.Template
```

### 2️⃣ Yeni Proje Oluşturma

```bash
# Yeni proje oluşturun
dotnet new basenkatman -n "MuhteşemProjem"

# Proje dizinine geçin
cd MuhteşemProjem
```

### 3️⃣ Kurulum ve Çalıştırma

```bash
# Bağımlılıkları yükleyin
dotnet restore

# Veritabanını oluşturun
dotnet ef database update

# Projeyi çalıştırın
dotnet run --project BaseNKatmanProject.API
```

🎉 **Tebrikler!** API'niz `https://localhost:7000` adresinde çalışıyor.

## 📸 Ekran Görüntüleri

### 🎨 Swagger UI
<div align="center">
  <img src="https://via.placeholder.com/800x400/0d1117/58a6ff?text=Swagger+API+Documentation" alt="Swagger UI" style="border-radius: 8px;"/>
</div>

### 🏗️ Proje Yapısı
<div align="center">
  <img src="https://via.placeholder.com/600x500/0d1117/58a6ff?text=Clean+Architecture+Layers" alt="Project Structure" style="border-radius: 8px;"/>
</div>

## 🛠️ Geliştirme Rehberi

### 🔄 Yeni Entity Ekleme

```csharp
// 1. Core katmanında entity oluşturun
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}

// 2. Application katmanında DTO oluşturun
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 3. AutoMapper profile'ı güncelleyin
CreateMap<Product, ProductDto>().ReverseMap();
```

### 📊 Migration Oluşturma

```bash
# Yeni migration ekleyin
dotnet ef migrations add "ProductEntityAdded" --project Infrastructure

# Veritabanını güncelleyin
dotnet ef database update --project Infrastructure
```

## 🎯 Kullanım Örnekleri

### 📝 Generic Repository Kullanımı

```csharp
// Dependency Injection ile kullanım
public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;
    
    public ProductService(IGenericRepository<Product> repository)
    {
        _repository = repository;
    }
    
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(product);
    }
}
```

### 🔍 Audit Log Takibi

```csharp
// Tüm değişiklikler otomatik olarak loglanır
var product = new Product { Name = "Yeni Ürün", Price = 100 };
await _repository.AddAsync(product); // Audit log otomatik oluşturulur
```

## 🤝 Katkıda Bulunma

Katkılarınızı memnuniyetle karşılarız! 

1. **Fork** edin 🍴
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. **Pull Request** açın 🚀

### 📋 Geliştirme Kuralları

- ✅ Clean Code prensiplerini takip edin
- ✅ Unit testler yazın
- ✅ [Conventional Commits](https://www.conventionalcommits.org/) kullanın
- ✅ Dokümantasyonu güncelleyin

## ⭐ Yol Haritası

- [ ] 🔐 JWT Authentication geliştirmeleri
- [ ] 📊 Redis Cache entegrasyonu  
- [ ] 🔄 Background Jobs (Hangfire)
- [ ] 📧 Email servis entegrasyonu
- [ ] 🐳 Docker desteği
- [ ] ☁️ Azure/AWS deployment templateları
- [ ] 📱 Xamarin/MAUI client template

## 📈 İstatistikler

<div align="center">

![GitHub Stats](https://github-readme-stats.vercel.app/api?username=yourusername&show_icons=true&theme=dark&hide_border=true)

</div>


## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır. Detaylar için `LICENSE` dosyasını inceleyebilirsiniz.

---

<div align="center">

**Beğendiyseniz ⭐ vermeyi unutmayın!**

Made with ❤️ by [SALİH YILDIZ](https://github.com/salihyildiz08)

![Footer](https://img.shields.io/badge/Made%20with-❤️-red?style=for-the-badge)

</div>
