using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Cnblogs.Academy.Common
{
    public class RedisManager
    {
        public static ConnectionMultiplexer Connect(IConfigurationSection redisConfiguration, out string redisConnectionString)
        {
            var redisOption = new CnblogsRedisOptions();
            redisConfiguration.Bind(redisOption);
            redisConnectionString = redisOption.ToString();
            return ConnectionMultiplexer.Connect(redisConnectionString);
        }
    }

    public class CnblogsRedisOptions
    {
        public string Host { get; set; } = "redis";

        public int Port { get; set; } = 6379;

        public string Password { get; set; }

        public int SyncTimeout { get; set; } = 2000;

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Host))
            {
                throw new ArgumentNullException(nameof(Host));
            }

            var connectionString = $"{Host}:{Port}";
            if (!string.IsNullOrEmpty(Password))
            {
                connectionString += $",password={Password},syncTimeout={SyncTimeout}";
            }

            return connectionString;
        }
    }
}
