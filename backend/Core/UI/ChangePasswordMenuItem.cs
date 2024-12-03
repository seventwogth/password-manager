using System;
using System.Threading.Tasks;
using PManager.Core.Interfaces;

namespace PManager.Core
{
    public class ChangePasswordMenuItem : MenuItem
    {

        private readonly IQueryManager _qManager;

        public ChangePasswordMenuItem(IQueryManager qManager)
          : base("Change password by login")
        {
            _qManager = qManager;
        }

        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Enter login:");
            string login = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Enter new password:");
            string newPassword = Console.ReadLine();
            Console.WriteLine();

            if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(newPassword))
            {
                await _qManager.ChangePasswordAsync(login, newPassword);
                Console.WriteLine("Password changed successfully.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Login or password cannot be empty.");
                Console.WriteLine();
            }
        }
    }
}
