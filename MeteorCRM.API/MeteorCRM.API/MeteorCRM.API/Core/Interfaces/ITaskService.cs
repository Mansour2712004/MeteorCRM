using MeteorCRM.API.Core.DTOs;

namespace MeteorCRM.API.Core.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDTO>> GetAllAsync(string userId, string role);
        Task<TaskResponseDTO> GetByIdAsync(Guid id, string userId, string role);
        Task<TaskResponseDTO> CreateAsync(CreateTaskDTO dto, string userId);
        Task<TaskResponseDTO> UpdateAsync(Guid id, UpdateTaskDTO dto, string userId, string role);
        Task DeleteAsync(Guid id, string userId, string role);
    }
}