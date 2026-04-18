using MeteorCRM.API.Core.Enums;
namespace MeteorCRM.API.Core.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        // Relations
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}