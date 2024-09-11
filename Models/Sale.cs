using System;

namespace Sales_Management.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateOfSale { get; set; }
        public string Status { get; set; } = string.Empty;

        public int UserId { get; set; } // Foreign key
        public User User { get; set; } = null!; // Navigation property
    }
}
