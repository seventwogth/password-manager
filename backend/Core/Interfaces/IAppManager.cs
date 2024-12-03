namespace PManager.Core.Interfaces
{
    public interface IAppManager
    {
        Task StartAsync();

        void Stop();
    }
}
