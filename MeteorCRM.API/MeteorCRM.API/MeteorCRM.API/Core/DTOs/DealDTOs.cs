namespace MeteorCRM.API.Core.DTOs
{
    public class CreateDealDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime? ExpectedCloseDate { get; set; }
        public string? Notes { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class UpdateDealDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int Stage { get; set; }
        public DateTime? ExpectedCloseDate { get; set; }
        public string? Notes { get; set; }
    }

    public class DealResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Stage { get; set; } = string.Empty;
        public DateTime? ExpectedCloseDate { get; set; }
        public string? Notes { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}