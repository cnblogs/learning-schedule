namespace Cnblogs.Academy.DTO
{
    public enum AccountStatus
    {
        Deleted = -2, // 0xFFFFFFFE
        NotExist = -1, // 0xFFFFFFFF
        Disabled = 0,
        Normal = 1,
        Locked = 2,
        NotActivate = 3,
    }
}
