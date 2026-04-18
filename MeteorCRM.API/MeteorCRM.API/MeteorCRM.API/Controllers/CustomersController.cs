using MeteorCRM.API.Core.DTOs;
using MeteorCRM.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeteorCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private string GetUserRole() =>
            User.FindFirstValue(ClaimTypes.Role)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _customerService.GetAllAsync(GetUserId(), GetUserRole());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _customerService.GetByIdAsync(id, GetUserId(), GetUserRole());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDTO dto)
        {
            try
            {
                var result = await _customerService.CreateAsync(dto, GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDTO dto)
        {
            try
            {
                var result = await _customerService.UpdateAsync(id, dto, GetUserId(), GetUserRole());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _customerService.DeleteAsync(id, GetUserId(), GetUserRole());
                return Ok(new { message = "Customer deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}