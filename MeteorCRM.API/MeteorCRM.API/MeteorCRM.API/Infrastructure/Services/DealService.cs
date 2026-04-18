using MeteorCRM.API.Core.DTOs;
using MeteorCRM.API.Core.Entities;
using MeteorCRM.API.Core.Enums;
using MeteorCRM.API.Core.Interfaces;
using MeteorCRM.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeteorCRM.API.Infrastructure.Services
{
    public class DealService : IDealService
    {
        private readonly MeteorDbContext _context;

        public DealService(MeteorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DealResponseDTO>> GetAllAsync(string userId, string role)
        {
            var query = _context.Deals.Include(d => d.Customer).AsQueryable();

            if (role != "Admin" && role != "Manager")
                query = query.Where(d => d.UserId == userId);

            var deals = await query.OrderByDescending(d => d.CreatedAt).ToListAsync();
            return deals.Select(MapToResponse);
        }

        public async Task<DealResponseDTO> GetByIdAsync(Guid id, string userId, string role)
        {
            var deal = await _context.Deals.Include(d => d.Customer).FirstOrDefaultAsync(d => d.Id == id);

            if (deal == null)
                throw new Exception("Deal not found.");

            if (role != "Admin" && role != "Manager" && deal.UserId != userId)
                throw new Exception("Unauthorized.");

            return MapToResponse(deal);
        }

        public async Task<DealResponseDTO> CreateAsync(CreateDealDTO dto, string userId)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            var deal = new Deal
            {
                Title = dto.Title,
                Value = dto.Value,
                ExpectedCloseDate = dto.ExpectedCloseDate,
                Notes = dto.Notes,
                CustomerId = dto.CustomerId,
                UserId = userId,
                Stage = DealStage.New
            };

            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();

            deal.Customer = customer;
            return MapToResponse(deal);
        }

        public async Task<DealResponseDTO> UpdateAsync(Guid id, UpdateDealDTO dto, string userId, string role)
        {
            var deal = await _context.Deals.Include(d => d.Customer).FirstOrDefaultAsync(d => d.Id == id);

            if (deal == null)
                throw new Exception("Deal not found.");

            if (role != "Admin" && role != "Manager" && deal.UserId != userId)
                throw new Exception("Unauthorized.");

            deal.Title = dto.Title;
            deal.Value = dto.Value;
            deal.Stage = (DealStage)dto.Stage;
            deal.ExpectedCloseDate = dto.ExpectedCloseDate;
            deal.Notes = dto.Notes;
            deal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToResponse(deal);
        }

        public async Task DeleteAsync(Guid id, string userId, string role)
        {
            var deal = await _context.Deals.FindAsync(id);

            if (deal == null)
                throw new Exception("Deal not found.");

            if (role != "Admin" && role != "Manager" && deal.UserId != userId)
                throw new Exception("Unauthorized.");

            deal.IsDeleted = true;
            deal.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private DealResponseDTO MapToResponse(Deal deal)
        {
            return new DealResponseDTO
            {
                Id = deal.Id,
                Title = deal.Title,
                Value = deal.Value,
                Stage = deal.Stage.ToString(),
                ExpectedCloseDate = deal.ExpectedCloseDate,
                Notes = deal.Notes,
                CustomerId = deal.CustomerId,
                CustomerName = $"{deal.Customer?.FirstName} {deal.Customer?.LastName}",
                CreatedAt = deal.CreatedAt
            };
        }
    }
}