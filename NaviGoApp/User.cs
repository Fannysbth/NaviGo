using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviGo
{
    public enum UserRole
    {
        Admin,
        Customer
    }


    // User Class

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public User(int userId, string username, string email, UserRole role)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Role = role;
        }

        public virtual bool Login()
        {
            // Logic untuk login - bisa dikembangkan lebih lanjut
            Console.WriteLine($"User {Username} logged in successfully");
            return true;
        }

        public virtual bool Register()
        {
            // Logic untuk registrasi - bisa dikembangkan lebih lanjut
            Console.WriteLine($"User {Username} registered successfully");
            return true;
        }

        public virtual void UpdateProfile()
        {
            Console.WriteLine($"Profile updated for user {Username}");
        }
    }
}
