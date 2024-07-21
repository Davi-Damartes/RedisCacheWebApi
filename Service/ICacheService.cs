namespace WebApiCaching.Service
{
    public interface ICacheService
    {
        T GetData<T>(string Key);

        bool SetData<T>(string Key, T value, DateTimeOffset expirationTime);

        object RemoveData(string Key);
    }
}
