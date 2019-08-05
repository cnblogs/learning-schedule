using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Cnblogs.CAP.RedisUtility
{
    public class RedisUtility
    {
        private readonly IDatabase _redis;
        public RedisUtility(ConnectionMultiplexer conn)
        {
            _redis = conn.GetDatabase();
        }

        public async Task<bool> DistinctAsync(string key, TimeSpan? expeireAt = null)
        {
            var result = await _redis.StringIncrementAsync(nameof(RedisUtility) + key);
            await _redis.KeyExpireAsync(key, expeireAt ?? TimeSpan.FromDays(1));
            return result <= 1;
        }
    }
}
