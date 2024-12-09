using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using PManagerApp.Core;
using PManagerApp.UI;

public partial class App : Application
{
    public App()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IPasswordService, PasswordService>()
            .BuildServiceProvider();

        var mainWindow = new MainWindow
        {
            DataContext = serviceProvider.GetService<MainWindow>()
        };

        mainWindow.Show();
    }
}
