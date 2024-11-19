namespace PManager.Core
{
    public class FindPasswordMenuItem : MenuItem
    {
        private readonly IQueryManager _qManager;

        public FindPasswordMenuItem(IQueryManager qManager)
          : base("Find password by login")
        {
            _qManager = qManager;
        }

        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Enter login:");
            string login = Console.ReadLine();
            Console.WriteLine();

            if (!String.IsNullOrEmpty(login))
            {
                string password = await _qManager.FindPasswordAsync(login);
                if (!String.IsNullOrEmpty(password))
                {
                    Console.WriteLine($"Password for '{login}' : {password}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Password not found.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Login cannot be empty.");
                Console.WriteLine();
            }
        }
    }
}
