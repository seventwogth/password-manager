namespace PManager.Core
{
    public class SavePasswordMenuItem : MenuItem
    {
        private readonly IQueryManager _qManager;

        public SavePasswordMenuItem(IQueryManager qManager)
          : base("Save new password")
        {
            _qManager = qManager;
        }

        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Enter login:");
            string login = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            Console.WriteLine();

            if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
            {
                await _qManager.SavePasswordAsync(login, password);
                Console.WriteLine("Password saved successfully.");
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
