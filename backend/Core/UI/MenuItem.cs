namespace PManager.Core
{
    public abstract class MenuItem
    {
        public string id { get; }

        protected MenuItem(string id)
        {
            this.id = id;
        }

        public abstract Task ExecuteAsync();
    }
}
