using System.Security.Cryptography;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly BeautyShopDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthenticateService(BeautyShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
       
        public bool VerifyPassword(string password, byte[] passwordHash)
        {
            var salt = "d278893f-5b29-442a-83bc-688f25d2cde0";
            using (var hmac = SHA256.Create())
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"{password}{salt}"));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (passwordHash[i] != computedHash[i])
                        return false;
                }

                return true;
            }
        }
    }
}
