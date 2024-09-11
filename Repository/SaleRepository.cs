using System;
using Microsoft.EntityFrameworkCore;
using Sales_Management.Context;
using Sales_Management.Interface;
using Sales_Management.Models;

namespace Sales_Management.Repository
{
    public class SaleRepository : IRepository<string, Sale>
    {
        private readonly ApplicationDbContext _context;
        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> Add(Sale item)
        {
            _context.Sales.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Sale?> Delete(string key)
        {
            var sale = await _context.Sales.FindAsync(key);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
            return sale;
        }

        public async Task<Sale?> Get(string key)
        {
            return await _context.Sales.FindAsync(key);
        }

        public async Task<List<Sale>?> GetAll()
        {
            return await _context.Sales.ToListAsync();
        }

        public async Task<Sale> Update(Sale item)
        {
            _context.Sales.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}

