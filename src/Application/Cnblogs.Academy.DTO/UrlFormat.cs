namespace Cnblogs.Academy.DTO
{
    public class UrlFormat
    {
        public static string GetIconUrl(string iconName)
        {
            if (string.IsNullOrEmpty(iconName))
                return "//pic.cnblogs.com/face/sample_face.gif";
            if (!iconName.StartsWith("http://") && !iconName.StartsWith("//"))
                return "//pic.cnblogs.com/face/" + iconName;
            return iconName;
        }

        public static string GetAvatarUrl(string avatarName)
        {
            if (string.IsNullOrEmpty(avatarName))
                return "//pic.cnblogs.com/avatar/simple_avatar.gif";
            if (!avatarName.StartsWith("http://") && !avatarName.StartsWith("//"))
                return "//pic.cnblogs.com/avatar/" + avatarName;
            return avatarName;
        }
    }
}
