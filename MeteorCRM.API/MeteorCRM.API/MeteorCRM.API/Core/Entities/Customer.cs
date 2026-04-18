using MeteorCRM.API.Core.Enums;
namespace MeteorCRM.API.Core.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public CustomerStatus Status { get; set; } = CustomerStatus.New;

        // Owner
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        // Navigation
        public ICollection<Deal> Deals { get; set; } = new List<Deal>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}