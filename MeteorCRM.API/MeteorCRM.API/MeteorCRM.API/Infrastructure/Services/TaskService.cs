using MeteorCRM.API.Core.DTOs;
using MeteorCRM.API.Core.Entities;
using MeteorCRM.API.Core.Enums;
using MeteorCRM.API.Core.Interfaces;
using MeteorCRM.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeteorCRM.API.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly MeteorDbContext _context;

        public TaskService(MeteorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskResponseDTO>> GetAllAsync(string userId, string role)
        {
            var query = _context.Tasks.Include(t => t.Customer).AsQueryable();

            if (role != "Admin" && role != "Manager")
                query = query.Where(t => t.UserId == userId);

            var tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
            return tasks.Select(MapToResponse);
        }

        public async Task<TaskResponseDTO> GetByIdAsync(Guid id, string userId, string role)
        {
            var task = await _context.Tasks.Include(t => t.Customer).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                throw new Exception("Task not found.");

            if (role != "Admin" && role != "Manager" && task.UserId != userId)
                throw new Exception("Unauthorized.");

            return MapToResponse(task);
        }

        public async Task<TaskResponseDTO> CreateAsync(CreateTaskDTO dto, string userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = (TaskPriority)dto.Priority,
                CustomerId = dto.CustomerId,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            if (dto.CustomerId.HasValue)
                task.Customer = await _context.Customers.FindAsync(dto.CustomerId);

            return MapToResponse(task);
        }

        public async Task<TaskResponseDTO> UpdateAsync(Guid id, UpdateTaskDTO dto, string userId, string role)
        {
            var task = await _context.Tasks.Include(t => t.Customer).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                throw new Exception("Task not found.");

            if (role != "Admin" && role != "Manager" && task.UserId != userId)
                throw new Exception("Unauthorized.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.Priority = (TaskPriority)dto.Priority;
            task.IsCompleted = dto.IsCompleted;
            task.CustomerId = dto.CustomerId;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToResponse(task);
        }

        public async Task DeleteAsync(Guid id, string userId, string role)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                throw new Exception("Task not found.");

            if (role != "Admin" && role != "Manager" && task.UserId != userId)
                throw new Exception("Unauthorized.");

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private TaskResponseDTO MapToResponse(TaskItem task)
        {
            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority.ToString(),
                IsCompleted = task.IsCompleted,
                CustomerId = task.CustomerId,
                CustomerName = task.Customer != null
                    ? $"{task.Customer.FirstName} {task.Customer.LastName}"
                    : null,
                CreatedAt = task.CreatedAt
            };
        }
    }
}