using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PManager.Core
{
  public interface IAppManager
  {
    Task StartAsync();

    void Stop();
  }
}
