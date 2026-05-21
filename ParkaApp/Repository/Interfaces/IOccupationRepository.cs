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
    }
}