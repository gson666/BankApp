namespace WebApplication1.Services.KeyService
{
    public interface IKeyService
    {
        Task<string> GenerateKeyAsync(string userId);
        Task<bool> ValidateKeyAsync(string userId, string key);
        Task InvalidateKeyAsync(string userId, string key);
    }
}
