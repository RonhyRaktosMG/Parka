using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetLastPaymentAsync(int clientId);
        Task AddAsync(Payment Payment);
        Task UpdateAsync(Payment Payment);
        Task DeleteAsync(int id);
    }
}