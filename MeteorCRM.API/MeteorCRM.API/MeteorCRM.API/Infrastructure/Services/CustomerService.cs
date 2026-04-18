using MeteorCRM.API.Core.DTOs;
using MeteorCRM.API.Core.Entities;
using MeteorCRM.API.Core.Enums;
using MeteorCRM.API.Core.Interfaces;
using MeteorCRM.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeteorCRM.API.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly MeteorDbContext _context;

        public CustomerService(MeteorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerResponseDTO>> GetAllAsync(string userId, string role)
        {
            var query = _context.Customers.AsQueryable();

            if (role != "Admin" && role != "Manager")
                query = query.Where(c => c.UserId == userId);

            var customers = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return customers.Select(MapToResponse);
        }

        public async Task<CustomerResponseDTO> GetByIdAsync(Guid id, string userId, string role)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                throw new Exception("Customer not found.");

            if (role != "Admin" && role != "Manager" && customer.UserId != userId)
                throw new Exception("Unauthorized.");

            return MapToResponse(customer);
        }

        public async Task<CustomerResponseDTO> CreateAsync(CreateCustomerDTO dto, string userId)
        {
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Company = dto.Company,
                Address = dto.Address,
                Notes = dto.Notes,
                UserId = userId,
                Status = CustomerStatus.New
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return MapToResponse(customer);
        }

        public async Task<CustomerResponseDTO> UpdateAsync(Guid id, UpdateCustomerDTO dto, string userId, string role)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                throw new Exception("Customer not found.");

            if (role != "Admin" && role != "Manager" && customer.UserId != userId)
                throw new Exception("Unauthorized.");

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Company = dto.Company;
            customer.Address = dto.Address;
            customer.Notes = dto.Notes;
            customer.Status = (CustomerStatus)dto.Status;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToResponse(customer);
        }

        public async Task DeleteAsync(Guid id, string userId, string role)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                throw new Exception("Customer not found.");

            if (role != "Admin" && role != "Manager" && customer.UserId != userId)
                throw new Exception("Unauthorized.");

            customer.IsDeleted = true;
            customer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private CustomerResponseDTO MapToResponse(Customer customer)
        {
            return new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Company = customer.Company,
                Address = customer.Address,
                Notes = customer.Notes,
                Status = customer.Status.ToString(),
                CreatedAt = customer.CreatedAt
            };
        }
    }
}