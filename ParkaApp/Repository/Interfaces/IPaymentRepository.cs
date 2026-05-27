using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetLastPaymentAsync(int clientId);
        Task<bool> AddAsync(Payment Payment, string carPlate);
        Task<bool> UpdateAsync(Payment Payment);
        Task<bool> DeleteAsync(int id);

        Task<Dictionary<string, double>> GetTotalRevenuePerDateAsync(DateTime startDate, DateTime endDate);

    }
}