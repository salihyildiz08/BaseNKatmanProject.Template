using BaseNKatmanProject.API.Middleware;
using BaseNKatmanProject.Application; // 📌 Application katmanını ekledik
using BaseNKatmanProject.Infrastructure;
using BaseNKatmanProject.Infrastructure.Data;
using BaseNKatmanProject.Infrastructure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ DbContext kaydı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();


// 2️⃣ Identity servisleri
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Şifre, lockout vb. ayarlar buraya eklenebilir
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 3️⃣ Infrastructure servisleri (Repository, UnitOfWork, CurrentUser vb.)
builder.Services.AddInfrastructureServices();

// 4️⃣ Application servisleri (FluentValidation otomatik kaydı dahil)
builder.Services.AddApplicationServices();

// 5️⃣ Authentication & JWT ayarları
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])
        )
    };
});

// 6️⃣ Authorization
builder.Services.AddAuthorization();

// 7️⃣ HttpContextAccessor (CurrentUserService için gerekli)
builder.Services.AddHttpContextAccessor();

// 8️⃣ Controller + FluentValidation desteği
builder.Services
    .AddControllers()
    .AddFluentValidation(config =>
    {
        config.AutomaticValidationEnabled = true;
    });

Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine("Serilog SelfLog: " + msg));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("LuckyPennySoftware.AutoMapper.License", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        tableName: "Logs",
        autoCreateSqlTable: false)
    .CreateLogger();

builder.Host.UseSerilog();


// 9️⃣ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔟 Pipeline ayarları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    using (Serilog.Context.LogContext.PushProperty("UserName", context.User.Identity?.Name ?? "anonymous"))
    using (Serilog.Context.LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress?.ToString()))
    using (Serilog.Context.LogContext.PushProperty("RequestPath", context.Request.Path))
    using (Serilog.Context.LogContext.PushProperty("HttpMethod", context.Request.Method))
    using (Serilog.Context.LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
    {
        await next.Invoke();
    }
});

app.UseHttpsRedirection();

app.UseAuthentication(); // Authorization'dan önce
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseMiddleware<SerilogMiddleware>();

app.MapControllers();

app.Run();
