using System.Text.Json;
using DeliveryAppAPI.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace DeliveryAppAPI.Helpers;

public static class CacheHelper
{
    
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? slidingExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = slidingExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<string?> GetRecordAsync(this IDistributedCache _, string recordId)
    {
         var configurationOptions = new ConfigurationOptions
         {
             EndPoints = { ConnectionStrings.Redis }
         };
         
         var serviceProvider = new ServiceCollection()
             .AddStackExchangeRedisCache(options => options.ConfigurationOptions = configurationOptions)
             .BuildServiceProvider();
        
        var cache = serviceProvider.GetRequiredService<IDistributedCache>();
        return await cache.GetStringAsync(recordId);
    }
}