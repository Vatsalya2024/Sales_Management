using Microsoft.AspNetCore.Identity;
using Sales_Management.Interface;
using Sales_Management.Models;

namespace Sales_Management.Service
{
    public class SaleService
    {
        private readonly IRepository<int, Sale> _saleRepository;
        private readonly UserManager<User> _userManager;

        public SaleService(IRepository<int, Sale> saleRepository, UserManager<User> userManager)
        {
            _saleRepository = saleRepository;
            _userManager = userManager;
        }

        public async Task<Sale?> CreateSaleAsync(Sale sale, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                sale.UserId = user.Id;
                return await _saleRepository.Add(sale);
            }
            return null;
        }

        public async Task<List<Sale>?> GetSalesAsync(string username, bool isAdmin)
        {
            if (isAdmin)
            {
                return await _saleRepository.GetAll();
            }
            else
            {
                var user = await _userManager.FindByNameAsync(username);
                return user?.Sales.ToList();
            }
        }

        public async Task<Sale?> UpdateSaleAsync(int saleId, Sale updatedSale, string username, bool isAdmin)
        {
            var sale = await _saleRepository.Get(saleId);
            if (sale != null && (isAdmin || sale.User.Username == username))
            {
                sale.ProductName = updatedSale.ProductName;
                sale.Amount = updatedSale.Amount;
                sale.DateOfSale = updatedSale.DateOfSale;
                sale.Status = updatedSale.Status;
                return await _saleRepository.Update(sale);
            }
            return null;
        }

        public async Task<Sale?> DeleteSaleAsync(int saleId, string username, bool isAdmin)
        {
            var sale = await _saleRepository.Get(saleId);
            if (sale != null && (isAdmin || sale.User.Username == username))
            {
                return await _saleRepository.Delete(saleId);
            }
            return null;
        }
    }

}
