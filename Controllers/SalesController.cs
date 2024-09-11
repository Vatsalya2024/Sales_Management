using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales_Management.Models;
using Sales_Management.Service;
using System.Linq;

namespace Sales_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesController(SaleService saleService, IHttpContextAccessor httpContextAccessor)
        {
            _saleService = saleService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            var sales = userRole == "Admin" 
                ? _saleService.GetAllSales() 
                : _saleService.GetSalesByUserId(userId);

            return Ok(sales);
        }

        [HttpGet("{id}")]
        public IActionResult GetSale(int id)
        {
            var sale = _saleService.GetSale(id);
            if (sale == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole != "Admin" && sale.UserId != userId)
            {
                return Forbid();
            }

            return Ok(sale);
        }

        [HttpPost]
        public IActionResult CreateSale([FromBody] Sale sale)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole != "Admin")
            {
                sale.UserId = userId; // Regular users can only create sales for themselves
            }

            var createdSale = _saleService.CreateSale(sale);
            return CreatedAtAction(nameof(GetSale), new { id = createdSale.Id }, createdSale);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSale(int id, [FromBody] Sale sale)
        {
            var existingSale = _saleService.GetSale(id);
            if (existingSale == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole != "Admin" && existingSale.UserId != userId)
            {
                return Forbid();
            }

            sale.Id = id; // Ensure the ID remains unchanged
            _saleService.UpdateSale(sale);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSale(int id)
        {
            var existingSale = _saleService.GetSale(id);
            if (existingSale == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole != "Admin" && existingSale.UserId != userId)
            {
                return Forbid();
            }

            _saleService.DeleteSale(id);
            return NoContent();
        }
    }
}
