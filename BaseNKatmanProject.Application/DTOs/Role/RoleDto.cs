namespace BaseNKatmanProject.Application.DTOs.Role;
public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string Aciklama { get; set; }
    public bool SilindiMi { get; set; }
}