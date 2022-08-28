using Xunit;
using Moq;
using WebUI.Services;
using Domain.Entities;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WebUI.UnitTests
{
    public class AuthenticateServiceTests
    {
        [Fact]
        public void VerifyPassword_Should_ReturnTrue_When_PasswordIsCorrect()
        {
            var authService = new Mock<AuthenticateService>();
            EncryptPass("testPassword", out byte[] hash);

            var result = authService.Object.VerifyPassword("testPassword", hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_Should_ReturnFalse_When_PasswordIsWrong()
        {
            var authService = new Mock<AuthenticateService>();
            EncryptPass("testPassword", out byte[] hash);

            var result = authService.Object.VerifyPassword("wrongPassword", hash);

            Assert.False(result);
        }
        public void EncryptPass(string password, out byte[] hash)
        {
            var salt = "d278893f-5b29-442a-83bc-688f25d2cde0";
            using (var hmac = SHA256.Create())
            {
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"{password}{salt}"));
            }
        }
    }
}
