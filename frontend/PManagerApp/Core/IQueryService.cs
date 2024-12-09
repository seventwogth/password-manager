using PManager.API;

namespace PManagerApp.Core
{
    public interface IPasswordService
    {
        Task SavePasswordAsync(PasswordModel password);

        Task<string> GetPasswordAsync(string login);

        Task ChangePasswordAsync(PasswordModel password);

        Task<List<PasswordModel>> GetAllPasswordsAsync();
    }
}

