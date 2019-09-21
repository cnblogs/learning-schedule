using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public interface IUCenterService
    {
        /// <summary>
        /// 检查用户是否为 网站管理员
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        Task<bool> CheckUserIsManager(string loginName);

        Task<UserDto> GetUser<T>(Expression<Func<UserQuery, T>> queryBy, T value);
        Task<UserDto[]> GetUsersByUserIds(Guid[] userIds);
    }
}