namespace ParkaApp.Services.Interfaces
{
    public interface IMVolaService
    {
        Task<string> GetAccessTokenAsync();
        Task<string?> MerchantPayAsync(string customerNumber, string amount);
        Task<string?> GetStatusAsync(string serverId);
    }
}