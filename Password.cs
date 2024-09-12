using System;

namespace password_manager 
{
  public class Password
  {
    public string login {get; init;}
    public string passwordValue {get; init;}

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
