namespace MeteorCRM.API.Core.DTOs
{
    public class CreateTaskDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public Guid? CustomerId { get; set; }
    }

    public class UpdateTaskDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public Guid? CustomerId { get; set; }
    }

    public class TaskResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}