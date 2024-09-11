using Microsoft.EntityFrameworkCore;
using Sales_Management.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Sales_Management.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }

}
