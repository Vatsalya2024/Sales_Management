using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales_Management.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        // These fields store the hashed password and salt as byte arrays
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } // "Admin" or "User"
    }
}
