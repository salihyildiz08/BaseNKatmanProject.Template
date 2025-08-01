namespace BaseNKatmanProject.Application.DTOs.Auth;
public class AuthDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string KullaniciAdi { get; set; }
    public IList<string> Roller { get; set; }
}