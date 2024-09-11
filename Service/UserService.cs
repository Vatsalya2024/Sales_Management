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

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetAll().Result.FirstOrDefault(u => u.Username == username);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; // Authentication failed
            }

            return user; // Authentication successful
        }

        public User Register(string username, string password, string role)
        {
            // Check if user already exists
            if (_userRepository.GetAll().Result.Any(u => u.Username == username))
            {
                throw new Exception("User already exists");
            }

            // Create password hash and salt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            return _userRepository.Add(user).Result;
        }

        public User? GetUserById(int id)
        {
            return _userRepository.Get(id).Result;
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user).Wait();
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id).Wait();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}

