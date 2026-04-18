using MeteorCRM.API.Core.DTOs;

namespace MeteorCRM.API.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponseDTO>> GetAllAsync(string userId, string role);
        Task<CustomerResponseDTO> GetByIdAsync(Guid id, string userId, string role);
        Task<CustomerResponseDTO> CreateAsync(CreateCustomerDTO dto, string userId);
        Task<CustomerResponseDTO> UpdateAsync(Guid id, UpdateCustomerDTO dto, string userId, string role);
        Task DeleteAsync(Guid id, string userId, string role);
    }
}