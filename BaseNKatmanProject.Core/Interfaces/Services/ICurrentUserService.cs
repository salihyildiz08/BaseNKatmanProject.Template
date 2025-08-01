namespace BaseNKatmanProject.Core.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string UserName { get; }
    }
}
