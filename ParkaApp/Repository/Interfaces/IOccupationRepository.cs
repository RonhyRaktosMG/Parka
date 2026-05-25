using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IOccupationRepository
    {
        Task<IEnumerable<Occupation>> GetAllAsync();
        Task<Occupation?> GetByIdAsync(int id);
        Task<bool> AddAsync(Occupation Occupation);
        Task<bool> UpdateAsync(Occupation Occupation);
        Task<bool> DeleteAsync(int id);

        Task<bool> AssignPlaceAsync(int placeId, string carPlate);
        Task<bool> ReleasePlaceAsync(int placeId);

        double CalculateTotalAmount(DateTime entryTime, DateTime exitTime);
    
        Task<Dictionary<string, int>> GetAreaStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, Dictionary<string, int>>> GetPlaceStatisticsPerAreaAsync(DateTime startDate, DateTime endDate);
    }
}