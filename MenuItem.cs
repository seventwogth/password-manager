using System;
using System.Threading.Tasks;

namespace password_manager
{
  public abstract class MenuItem
  {
    public string _id { get; }

    protected MenuItem(string id)
    {
      _id = id;
    }

    public abstract Task ExecuteAsync();
  }
}
