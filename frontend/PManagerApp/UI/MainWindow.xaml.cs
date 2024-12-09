using PManager.API;
using System.Windows;
using System.Windows.Media;
using PManagerApp.Core;


namespace PManagerApp.UI
{
    public partial class MainWindow : Window
    {
        private readonly IPasswordService _passwordService;

        public MainWindow()
        {
            InitializeComponent();
            _passwordService = new PasswordService(); // или внедрение через DI
            LoadPasswords();
        }

        private async void LoadPasswords()
        {
            var passwords = await _passwordService.GetAllPasswordsAsync();
            if (passwords != null)
            {
                PasswordsListView.ItemsSource = passwords;
            }
        }

        private async void SavePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var newPassword = new PasswordModel
            {
                Login = LoginTextBox.Text,
                Password = PasswordTextBox.Password
            };

            await _passwordService.SavePasswordAsync(newPassword);
            MessageBox.Show("Password saved successfully.");
            LoadPasswords();
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var updatedPassword = new PasswordModel
            {
                Login = LoginTextBox.Text,
                Password = PasswordTextBox.Password
            };

            await _passwordService.ChangePasswordAsync(updatedPassword);
            MessageBox.Show("Password changed successfully.");
            LoadPasswords();
        }

        private async void GetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var password = await _passwordService.GetPasswordAsync(LoginTextBox.Text);
            if (password != null)
            {
                MessageBox.Show($"Password: {password}");
            }
            else
            {
                MessageBox.Show("Password not found.");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Login")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.Text = "Login";
                LoginTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password == "Password")
            {
                PasswordTextBox.Password = "";
                PasswordTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                PasswordTextBox.Password = "Password";
                PasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
