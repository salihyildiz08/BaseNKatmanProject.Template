using BaseNKatmanProject.Application.DTOs.Auth;
using BaseNKatmanProject.Application.Interfaces.Services;
using BaseNKatmanProject.Core.Commons;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ResponseMessage<object>>> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));

            _logger.LogWarning("Kayıt işlemi başarısız. Model doğrulama hatası: {Errors}", errors);
            return BadRequest(ResponseMessage<object>.Failure(errors));
        }

        try
        {
            var result = await _authService.RegisterAsync(model);
            _logger.LogInformation("Yeni kullanıcı kaydı başarılı. Email: {Email}", model.Email);
            return Ok(ResponseMessage<object>.SuccessResult(result, "Kullanıcı başarıyla oluşturuldu."));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı kayıt işlemi sırasında hata oluştu. Email: {Email}", model.Email);
            return BadRequest(ResponseMessage<object>.Failure(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ResponseMessage<AuthDto>>> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));

            _logger.LogWarning("Giriş işlemi başarısız. Model doğrulama hatası: {Errors}", errors);
            return BadRequest(ResponseMessage<AuthDto>.Failure(errors));
        }

        try
        {
            var token = await _authService.LoginAsync(model);
            _logger.LogInformation("Kullanıcı giriş başarılı. Email: {Email}", model.Email);
            return Ok(ResponseMessage<AuthDto>.SuccessResult(token, "Giriş başarılı."));
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "Giriş başarısız. Email: {Email}", model.Email);
            return Unauthorized(ResponseMessage<AuthDto>.Failure(ex.Message));
        }
    }
}
