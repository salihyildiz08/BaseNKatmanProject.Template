using Microsoft.AspNetCore.Identity;

namespace BaseNKatmanProject.Infrastructure.Identity;
public class ApplicationRole : IdentityRole<Guid>
{
    public string Aciklama { get; set; }
    public bool SilindiMi { get; set; } = false;
}