using System;

namespace password_manager 
{
  public class Password
  {
    public string Login {get; init;}
    public string PasswordValue {get; init;}

    public Password(string login, string password)
    {
      this.Login = login;
      this.PasswordValue = password;
    }

    public void print()
    {
      Console.WriteLine("Login: '" + Login + "'  Password: " + PasswordValue);
    }
  }
}
