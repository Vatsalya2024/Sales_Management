using System;
using Microsoft.EntityFrameworkCore;
using Sales_Management.Context;
using Sales_Management.Interface;
using Sales_Management.Models;

namespace Sales_Management.Repository
{
    public class UserRepository : IRepository<string, User>
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User?> Delete(string key)
        {
            var user = await _context.Users.FindAsync(key);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User?> Get(string key)
        {
            return await _context.Users.FindAsync(key);
        }

        public async Task<List<User>?> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Update(User item)
        {
            _context.Users.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}

