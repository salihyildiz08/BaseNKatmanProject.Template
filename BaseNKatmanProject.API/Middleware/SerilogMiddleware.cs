namespace BaseNKatmanProject.API.Middleware;

using Serilog;
using Serilog.Context;
using System.Text.Json;

public class SerilogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public SerilogMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        if (ip == "::1" || ip == "127.0.0.1")
            ip = "Localhost";

        string city = "Unknown";
        string userName = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";
        Guid? userId = null;

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c =>
                c.Type == "sub" || c.Type == "UserId" || c.Type == "nameidentifier");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedId))
                userId = parsedId;
        }

        try
        {
            if (ip != "Localhost" && ip != "Unknown")
            {
                var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
                var geoInfo = JsonSerializer.Deserialize<GeoInfo>(response);
                city = geoInfo?.City ?? "Unknown";
            }
        }
        catch
        {
            // Geo info alırken hata olursa yok say
        }

        await _next(context); // request devam eder

        var statusCode = context.Response.StatusCode;

        using (LogContext.PushProperty("IpAddress", ip))
        using (LogContext.PushProperty("City", city))
        using (LogContext.PushProperty("UserName", userName))
        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path.ToString()))
        using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        using (LogContext.PushProperty("StatusCode", statusCode))
        using (LogContext.PushProperty("MachineName", Environment.MachineName))
        using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            Log.Information("HTTP {HttpMethod} {RequestPath} responded {StatusCode}", context.Request.Method, context.Request.Path, statusCode);
        }


    }
}

public class GeoInfo
{
    public string Status { get; set; }
    public string Country { get; set; }
    public string RegionName { get; set; }
    public string City { get; set; }
}