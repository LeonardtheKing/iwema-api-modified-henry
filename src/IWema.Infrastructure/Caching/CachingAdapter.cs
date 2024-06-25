using IWema.Application.Contract;
using Microsoft.Extensions.Caching.Memory;

namespace IWema.Infrastructure.Caching;

public class CachingAdapter(IMemoryCache memoryCache) : ICachingAdapter
{
    public void AddCache<T>(string name, T value)
    {
        var today = DateTime.UtcNow.AddHours(1);
        var endOfDay = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59) - today;
        memoryCache.Set(name, value, endOfDay);
    }

    public T GetCache<T>(string name)
    {
        if (!memoryCache.TryGetValue(name, out T value))
        {
            return default;
        }

        return value;
    }

    public void RemoveCache(string name)
    {
        memoryCache.Remove(name);

    }
}
