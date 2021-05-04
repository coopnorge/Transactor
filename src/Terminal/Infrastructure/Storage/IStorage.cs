namespace Terminal.Infrastructure.Storage
{
    public interface IStorage
    {
        public void Save<TData>(string key, TData data);

        public TData Get<TData>(string key);
    }
}
