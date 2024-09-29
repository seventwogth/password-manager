using System;
using System.Threading.Tasks;

namespace PManager.Core
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
