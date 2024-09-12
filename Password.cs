using System;

namespace password_manager 
{
  public class Password
  {
    private string login {get; init;}
    private string passwordValue {get; init;}0

    public Password(string login, string password)
    {
      this.login = login;
      this.passwordValue = password;
    }

    public void print()
    {
      Console.WriteLine("Login: '" + login + "'  Password: " + passwordValue);
    }
  }
}
