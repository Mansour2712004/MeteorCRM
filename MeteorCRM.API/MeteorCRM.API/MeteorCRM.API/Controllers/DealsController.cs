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
    public class DealsController : ControllerBase
    {
        private readonly IDealService _dealService;

        public DealsController(IDealService dealService)
        {
            _dealService = dealService;
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
                var result = await _dealService.GetAllAsync(GetUserId(), GetUserRole());
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
                var result = await _dealService.GetByIdAsync(id, GetUserId(), GetUserRole());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealDTO dto)
        {
            try
            {
                var result = await _dealService.CreateAsync(dto, GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDealDTO dto)
        {
            try
            {
                var result = await _dealService.UpdateAsync(id, dto, GetUserId(), GetUserRole());
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
                await _dealService.DeleteAsync(id, GetUserId(), GetUserRole());
                return Ok(new { message = "Deal deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}