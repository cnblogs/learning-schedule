using System;
using Cnblogs.UCenter.DTO.Users;
using Newtonsoft.Json;

namespace Cnblogs.Academy.DTO
{
    public class AcademyUserDto
    {
        public AcademyUserDto()
        {
        }

        public AcademyUserDto(UserDto user) : this(user.UserId, user.Alias, user.IconName, user.DisplayName)
        {
        }

        public AcademyUserDto(Guid userId, string alias, string icon, string userName)
        {
            UserId = userId;
            Alias = alias;
            Icon = icon;
            UserName = userName;
        }

        [JsonIgnore]
        public Guid UserId { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string UserName { get; set; }
    }
}
