using System;

namespace Cnblogs.Academy.DTO
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string LoginName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterTime { get; set; }

        public AccountStatus Status { get; set; }

        private string _alias;

        public string Alias
        {
            get
            {
                if (string.IsNullOrEmpty(_alias))
                {
                    _alias = string.IsNullOrEmpty(BlogApp) ? SpaceUserId.ToString() : BlogApp;
                }

                return _alias;
            }
            set => _alias = value;
        }

        private string _iconName;

        public string IconName
        {
            get => _iconName;
            set => _iconName = UrlFormat.GetIconUrl(value);
        }

        private string _avatarName;

        public string AvatarName
        {
            get => _avatarName;
            set => _avatarName = UrlFormat.GetAvatarUrl(value);
        }

        public int SpaceUserId { get; set; }

        public string BlogApp { get; set; }

        public int BlogId { get; set; } = -1;

        public int Wealth { get; set; }

        public int FollowingCount { get; set; }

        public int FollowerCount { get; set; }

        public bool ForcePasswordReset { get; set; }

        public bool IsGagged { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotificationEmail { get; set; }

        public string Remark { get; set; }
    }
}
