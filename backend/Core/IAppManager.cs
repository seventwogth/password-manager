namespace PManager.Core
{
    public interface IAppManager
    {
        Task StartAsync();

        void Stop();
    }
}
