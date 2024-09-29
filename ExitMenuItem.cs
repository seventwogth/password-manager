using System;
using System.Threading.Tasks;

namespace password_manager
{
  public class ExitMenuItem : MenuItem
  {
    private readonly AppManager _appManager;

    public ExitMenuItem(AppManager appManager)
      : base("Exit")
    {
      _appManager = appManager;
    }

    public override Task ExecuteAsync()
    {
      _appManager.Stop();
      return Task.CompletedTask;
    }
  }
}
