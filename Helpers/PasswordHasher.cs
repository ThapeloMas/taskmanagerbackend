
using System.Security.Cryptography;

namespace TodoApp.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Use BCrypt for secure password hashing
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}