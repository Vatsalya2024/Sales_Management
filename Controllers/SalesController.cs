using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales_Management.Interface;
using Sales_Management.Models;
using Sales_Management.Service;
using System.Linq;
using System.Security.Claims;

namespace Sales_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var sales = await _saleService.GetSalesByUser(userId, isAdmin);
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var sale = await _saleService.GetSaleById(id, userId, isAdmin);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] Sale sale)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var createdSale = await _saleService.CreateSale(sale, userId, isAdmin);
            return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.Id }, createdSale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(string id, [FromBody] Sale sale)
        {
            sale.Id = id;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var updatedSale = await _saleService.UpdateSale(sale, userId, isAdmin);
            if (updatedSale == null)
            {
                return NotFound();
            }
            return Ok(updatedSale);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var deletedSale = await _saleService.DeleteSale(id, userId, isAdmin);
            if (deletedSale == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
