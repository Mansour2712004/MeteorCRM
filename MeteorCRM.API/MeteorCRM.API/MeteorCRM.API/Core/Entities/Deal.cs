using MeteorCRM.API.Core.Enums;
namespace MeteorCRM.API.Core.Entities
{
    public class Deal : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DealStage Stage { get; set; } = DealStage.New;
        public DateTime? ExpectedCloseDate { get; set; }
        public string? Notes { get; set; }

        // Relations
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}