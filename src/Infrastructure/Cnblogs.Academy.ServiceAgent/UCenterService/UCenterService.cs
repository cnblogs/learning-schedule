using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Cnblogs.Academy.DTO;
using Enyim.Caching;
using Enyim.Caching.Memcached.KeyTransformers;
using Microsoft.Extensions.Logging;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public class UCenterService : IUCenterService
    {
        private const string UcenterService = "ucenter";
        private readonly HttpClient _httpClient;
        private readonly ILogger<UCenterService> _logger;
        private readonly IMemcachedClient _memcachedClient;

        public UCenterService(HttpClient httpClient, ILogger<UCenterService> logger, IMemcachedClient memcachedClient)
        {
            _httpClient = httpClient;
            _logger = logger;
            _memcachedClient = memcachedClient;
        }

        private async Task<Exception> NotOkException(HttpResponseMessage response)
        {
            string errorTitle =
                $"{(object)(int)response.StatusCode}. Failed to {response.RequestMessage.Method} " +
                response.RequestMessage.RequestUri.AbsoluteUri;
            string str = errorTitle;
            _logger.LogError(str + "\n" + await response.Content.ReadAsStringAsync());
            return new HttpRequestException(errorTitle);
        }

        private async Task<bool> IsSiteAdminInternal(string query)
        {
            var apiUrl = $"/api/v2/roles/siteAdmin/user?{query}";

            var response = await _httpClient.HeadAsync(apiUrl, UcenterService).ConfigureAwait(false);
            if (!response.IsValidated)
                throw await NotOkException(response.Response);
            if (response.Response.StatusCode == HttpStatusCode.NotFound)
                return false;
            if (response.Response.IsSuccessStatusCode)
                return true;
            throw await NotOkException(response.Response);
        }

        /// <summary>
        /// 检查用户是否为 网站管理员
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<bool> CheckUserIsManager(string loginName)
        {
            if (string.IsNullOrWhiteSpace(loginName))
                throw new ArgumentNullException(nameof(loginName));
            return await IsSiteAdminInternal($"{nameof(loginName)}={HttpUtility.UrlEncode(loginName)}");
        }

        #region User

        public async Task<UserDto> GetUser<T>(Expression<Func<UserQuery, T>> queryBy, T value)
        {
            var member = (queryBy.Body as MemberExpression)?.Member;
            if (member == null)
                throw new ArgumentOutOfRangeException(nameof(queryBy));

            switch (member.Name)
            {
                case nameof(UserQuery.LoginName):
                    return await GetUserByLoginName((string)(object)value);
                case nameof(UserQuery.DisplayName):
                    return await GetUserByDisplayName((string)(object)value);
                case nameof(UserQuery.BlogApp):
                    return await GetUserByBlogApp((string)(object)value);
                case nameof(UserQuery.UserId):
                    return await GetUserByUserId((Guid)(object)value);
                case nameof(UserQuery.BlogId):
                    return await GetUserByBlogId((int)(object)value);
                case nameof(UserQuery.SpaceUserId):
                    return await GetUserBySpaceUserId((int)(object)value);
                case nameof(UserQuery.Alias):
                    return await GetUserByAlias((string)(object)value);
                default:
                    return null;
            }
        }

        public async Task<TResult> GetUser<TSource, TResult>(
            Expression<Func<UserQuery, TSource>> queryBy,
            TSource queryValue,
            Func<UserDto, TResult> selector)
        {
            var user = await GetUser(queryBy, queryValue);
            if (user == null)
                return default(TResult);
            return selector(user);
        }

        public async Task<UserDto> GetUserByLoginName(string loginName, bool cached = true)
        {
            if (string.IsNullOrWhiteSpace(loginName))
                throw new ArgumentNullException(nameof(loginName));

            if (!cached)
            {
                return await GetUserByLoginNameInternal(loginName);
            }

            var cacheKey = "ucenter_userdto_loginname_" + KeyTransformerUtility.ToSHA1Hash(loginName);
            return await _memcachedClient.GetValueOrCreateAsync(
                cacheKey,
                3600,
                () => GetUserByLoginNameInternal(loginName));
        }

        private async Task<UserDto> GetUserByLoginNameInternal(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return null;
            }

            var response = await _httpClient.GetAsync(
                "/api/v2/users?loginName="
                + WebUtility.UrlEncode(loginName)).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<UserDto>();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            else
            {
                throw await NotOkException(response);
            }
        }

        public async Task<UserDto> GetUserByDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            var path = "/api/v2/user/loginName?displayName=" + WebUtility.UrlEncode(displayName);
            var loginName = await GetResponseString(path);
            return await GetUserByLoginNameInternal(loginName);
        }

        public async Task<UserDto> GetUserByBlogApp(string blogApp)
        {
            if (string.IsNullOrWhiteSpace(blogApp))
                throw new ArgumentNullException(nameof(blogApp));

            var path = $"/api/v2/user/blogapp:{blogApp}/loginName";
            var loginName = await GetResponseString(path);
            return await GetUserByLoginNameInternal(loginName);
        }

        public async Task<UserDto> GetUserByBlogId(int blogId)
        {
            if (blogId < 0) return null;
            var path = $"/api/v2/user/blogId:{blogId}/loginName";
            var loginName = await GetResponseString(path);
            return await GetUserByLoginNameInternal(loginName);
        }

        public async Task<UserDto> GetUserBySpaceUserId(int spaceUserId)
        {
            if (spaceUserId <= 0) return null;
            var path = $"/api/v2/user/spaceUserId:{spaceUserId}/loginName";
            var loginName = await GetResponseString(path);
            return await GetUserByLoginNameInternal(loginName);
        }

        public async Task<UserDto> GetUserByUserId(Guid userId)
        {
            if (userId == default(Guid))
                return null;

            var path = $"/api/v2/user/{userId}/loginName";
            var loginName = await GetResponseString(path);
            return await GetUserByLoginNameInternal(loginName);
        }

        public async Task<UserDto> GetUserByAlias(string alias)
        {
            var user = await GetUserByBlogApp(alias);
            if (user == null)
            {
                if (int.TryParse(alias, out var uid))
                {
                    user = await GetUserBySpaceUserId(uid);
                }
            }

            return user;
        }

        public async Task<UserDto[]> GetUsersByUserIds(Guid[] userIds)
        {
            var path = "/api/v2/users/[userIds]/loginNames";
            var response = await _httpClient.PostAsJsonAsync(path, userIds);
            if (!response.IsSuccessStatusCode)
            {
                throw await NotOkException(response);
            }

            var loginNames = await response.Content.ReadAsAsync<string[]>();
            if (loginNames == null || loginNames.Length == 0)
            {
                return Array.Empty<UserDto>();
            }

            var tasks = new Task<UserDto>[loginNames.Length];
            for (var i = 0; i < loginNames.Length; i++)
            {
                tasks[i] = GetUserByLoginName(loginNames[i]);
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<UserDto[]> GetUsersByBlogIds(int[] blogIds)
        {
            var path = "/api/v2/users/[blogIds]/loginNames";
            var response = await _httpClient.PostAsJsonAsync(path, blogIds);
            if (!response.IsSuccessStatusCode)
            {
                throw await NotOkException(response);
            }

            var loginNames = await response.Content.ReadAsAsync<string[]>();
            if (loginNames == null || loginNames.Length == 0)
            {
                return Array.Empty<UserDto>();
            }

            var tasks = new Task<UserDto>[loginNames.Length];
            for (var i = 0; i < loginNames.Length; i++)
            {
                tasks[i] = GetUserByLoginName(loginNames[i]);
            }

            return await Task.WhenAll(tasks);
        }

        private async Task<string> GetResponseString(string path)
        {
            var response = await _httpClient.GetAsync(path);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return string.Empty;
            }
            else
            {
                throw await NotOkException(response);
            }
        }

        #endregion
    }

    public class UserQuery
    {
        public Guid UserId { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string BlogApp { get; set; }
        public int BlogId { get; set; }
        public int SpaceUserId { get; set; }
        public string Alias { get; set; }
    }
}
