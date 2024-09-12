using Sales_Management.Interface;
using Sales_Management.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sales_Management.Service
{
    public class UserService
    {
        private readonly IRepository<int, User> _userRepository;

        public UserService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
        }

        // Authenticate a user based on username and password
        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetAll().Result.FirstOrDefault(u => u.Username == username);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; // Authentication failed
            }

            return user; // Authentication successful
        }

        // Register a new user
        public User Register(string username, string password, string role)
        {
            // Check if the username already exists
            if (_userRepository.GetAll().Result.Any(u => u.Username == username))
            {
                throw new Exception("User already exists");
            }

            // Generate password hash and salt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create and store the user
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            return _userRepository.Add(user).Result;
        }

        // Retrieve a user by ID
        public User? GetUserById(int id)
        {
            return _userRepository.Get(id).Result;
        }

        // Update user details
        public void UpdateUser(User user)
        {
            _userRepository.Update(user).Wait();
        }

        // Delete a user by ID
        public void DeleteUser(int id)
        {
            _userRepository.Delete(id).Wait();
        }

        // Private method to create a password hash and salt
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key; // Store the key as the salt
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Compute the hash
            }
        }

        // Private method to verify password hash
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash); // Compare the stored and computed hash
            }
        }
    }
}

