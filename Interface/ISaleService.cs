using System;
using Sales_Management.Models;

namespace Sales_Management.Interface
{
    public interface ISaleService
    {
        Task<List<Sale>?> GetSalesByUser(string userId, bool isAdmin);
        Task<Sale?> GetSaleById(string id, string userId, bool isAdmin);
        Task<Sale> CreateSale(Sale sale, string userId, bool isAdmin);
        Task<Sale?> UpdateSale(Sale sale, string userId, bool isAdmin);
        Task<Sale?> DeleteSale(string id, string userId, bool isAdmin);
    }
}

