using Sales_Management.Interface;
using System.Collections.Generic;
using System;
using Sales_Management.Context;
using Microsoft.EntityFrameworkCore;

namespace Sales_Management.Repository
{
    public class Repository<K, T> : IRepository<K, T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>?> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> Get(K key)
        {
            return await _dbSet.FindAsync(key);
        }

        public async Task<T> Add(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> Update(T item)
        {
            _dbSet.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T?> Delete(K key)
        {
            var item = await _dbSet.FindAsync(key);
            if (item != null)
            {
                _dbSet.Remove(item);
                await _context.SaveChangesAsync();
            }
            return item;
        }
    }

}
