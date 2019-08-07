using System;
using Cnblogs.Academy.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public static class CnblogsAuthServiceCollectionExtensions
    {
        public static IServiceCollection AddCnblogsAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<CookieAuthenticationOptions> configureOption = null)
        {
            return services.AddCnblogsAuthentication(configuration.GetSection("redis"), configureOption);
        }

        public static IServiceCollection AddCnblogsAuthentication(
            this IServiceCollection services,
            IConfigurationSection redisConfiguration,
            Action<CookieAuthenticationOptions> configureOption = null)
        {
            var redisConn = RedisManager.Connect(redisConfiguration, out var redisConnectionString);
            return services.AddCnblogsAuthentication(redisConn, redisConnectionString, configureOption);
        }

        public static IServiceCollection AddCnblogsAuthentication(
            this IServiceCollection services,
            ConnectionMultiplexer redisConn,
            string redisConnectionString,
            Action<CookieAuthenticationOptions> configureOption = null)
        {
            services.AddDistributedRedisCache(options => options.Configuration = redisConnectionString);
            services.AddDataProtection(options => options.ApplicationDiscriminator = "cnblogs")
                .PersistKeysToRedis(redisConn, "DataProtection-Keys-Cnblogs");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
                        options.Cookie.HttpOnly = true;
                        options.Cookie.Name = ".Cnblogs.AspNetCore.Cookies";
                        options.Cookie.Domain = ".cnblogs.com";
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.LoginPath = "/signin";

                        options.Events = new CnblogsCookieAuthenticationEvents();

                        configureOption?.Invoke(options);
                    });

            return services;
        }
    }
}
