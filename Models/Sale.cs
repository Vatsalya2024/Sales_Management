using System;
using System.ComponentModel.DataAnnotations;

namespace Sales_Management.Models
{
    public class Sale
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfSale { get; set; }
        public string Status { get; set; } // "pending", "completed", "returned"
        public string UserId { get; set; } // Foreign Key
    }
}
