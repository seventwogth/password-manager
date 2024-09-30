using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PManager.Data
{
  public interface IAppManager
  {
    Task StartAsync();

    void Stop();
  }
}
