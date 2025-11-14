using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace UI_NaviGO
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public static class UserManager
    {
        private static List<User> _users = new List<User>();

        // User dummy untuk testing
        static UserManager()
        {
            // Tambahkan user dummy
            _users.Add(new User
            {
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = HashPassword("password123")
            });
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        public static bool RegisterUser(string name, string email, string password)
        {
            // Cek jika email sudah terdaftar
            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            // Validasi password
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return false;
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _users.Add(newUser);
            return true;
        }

        public static User LoginUser(string email, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        // Method untuk debugging: lihat semua user
        public static List<User> GetAllUsers()
        {
            return new List<User>(_users);
        }
    }
}