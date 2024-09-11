using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sales_Management.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required]
        public string Role { get; set; } = string.Empty;

        public ICollection<Sale> Sales { get; set; } = new List<Sale>(); // Collection navigation property
    }
}
