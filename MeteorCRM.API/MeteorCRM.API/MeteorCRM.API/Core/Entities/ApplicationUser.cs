using Microsoft.AspNetCore.Identity;

namespace MeteorCRM.API.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Deal> Deals { get; set; } = new List<Deal>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}