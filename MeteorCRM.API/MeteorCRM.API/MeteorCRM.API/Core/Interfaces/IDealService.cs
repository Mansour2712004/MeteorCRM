using MeteorCRM.API.Core.DTOs;

namespace MeteorCRM.API.Core.Interfaces
{
    public interface IDealService
    {
        Task<IEnumerable<DealResponseDTO>> GetAllAsync(string userId, string role);
        Task<DealResponseDTO> GetByIdAsync(Guid id, string userId, string role);
        Task<DealResponseDTO> CreateAsync(CreateDealDTO dto, string userId);
        Task<DealResponseDTO> UpdateAsync(Guid id, UpdateDealDTO dto, string userId, string role);
        Task DeleteAsync(Guid id, string userId, string role);
    }
}