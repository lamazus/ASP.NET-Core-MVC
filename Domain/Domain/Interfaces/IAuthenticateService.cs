
namespace Domain.Interfaces
{
    public interface IAuthenticateService
    {
        public bool VerifyPassword(string password, byte[] passwordHash);
    }
}
