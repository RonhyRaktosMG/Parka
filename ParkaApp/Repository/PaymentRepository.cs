using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

namespace ParkaApp.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(string? searchString = null)
        {
            var query = _context.Payments.OrderByDescending(p => p.Id).Include(p => p.Client).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Client!.Name!.ToLower().Contains(searchString.ToLower()) || p.Client!.CarPlate!.ToLower().Contains(searchString.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetLastPaymentAsync(int clientId)
        {
            return await _context.Payments
                .Where(p => p.ClientId == clientId)
                .OrderByDescending(p => p.EndDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddAsync(Payment Payment, string carPlate)
        {
            // Find the client by car plate
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == carPlate);
            if (client == null)            {
                return false; // Client not found
            }

            Payment.ClientId = client.Id;

            _context.Payments.Add(Payment);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdateAsync(Payment Payment)
        {
            // check if the payment exists
            var existingPayment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Payment.Id);
            if (existingPayment == null)
            {
                return false; // Payment not found
            }

            // Check if user exists
            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Payment.ClientId);
            if (existingClient == null)
            {
                return false; // Client not found
            }


            _context.Payments.Update(Payment);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Payment = await GetByIdAsync(id);
            if (Payment != null)
            {
                _context.Payments.Remove(Payment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        // Statistics
        public async Task<Dictionary<string, double>> GetTotalRevenuePerDateAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _context.Payments
                .Where(p => p.StartDate >= startDate && p.EndDate <= endDate)
                .ToListAsync();

            var revenueByDay = payments
                .GroupBy(p => p.StartDate.Date)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Sum(p => p.Amount));

            return revenueByDay;
        }
    }
}