using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales_Management.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin" or "User"
    }
}
