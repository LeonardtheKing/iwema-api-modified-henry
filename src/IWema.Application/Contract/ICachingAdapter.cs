namespace IWema.Application.Contract;

public interface ICachingAdapter
{
    void AddCache<T>(string name, T value);
    T GetCache<T>(string name);
    void RemoveCache(string name);
}