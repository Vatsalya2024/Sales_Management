using Microsoft.AspNetCore.Identity;
using Sales_Management.Interface;
using Sales_Management.Models;

namespace Sales_Management.Service
{
    public class SaleService : ISaleService
    {
        private readonly IRepository<string, Sale> _saleRepository;
        public SaleService(IRepository<string, Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Sale> CreateSale(Sale sale, string userId, bool isAdmin)
        {
            if (!isAdmin)
            {
                sale.UserId = userId;
            }
            return await _saleRepository.Add(sale);
        }

        public async Task<Sale?> DeleteSale(string id, string userId, bool isAdmin)
        {
            var sale = await _saleRepository.Get(id);
            if (sale == null || (!isAdmin && sale.UserId != userId))
            {
                return null;
            }
            return await _saleRepository.Delete(id);
        }

        public async Task<Sale?> GetSaleById(string id, string userId, bool isAdmin)
        {
            var sale = await _saleRepository.Get(id);
            if (sale == null || (!isAdmin && sale.UserId != userId))
            {
                return null;
            }
            return sale;
        }

        public async Task<List<Sale>?> GetSalesByUser(string userId, bool isAdmin)
        {
            var allSales = await _saleRepository.GetAll();
            if (allSales == null)
            {
                return null;
            }

            if (isAdmin)
            {
                return allSales;
            }
            return allSales.FindAll(sale => sale.UserId == userId);
        }

        public async Task<Sale?> UpdateSale(Sale sale, string userId, bool isAdmin)
        {
            var existingSale = await _saleRepository.Get(sale.Id);
            if (existingSale == null || (!isAdmin && existingSale.UserId != userId))
            {
                return null;
            }

            existingSale.ProductName = sale.ProductName;
            existingSale.Amount = sale.Amount;
            existingSale.DateOfSale = sale.DateOfSale;
            existingSale.Status = sale.Status;
            return await _saleRepository.Update(existingSale);
        }
    }

}
