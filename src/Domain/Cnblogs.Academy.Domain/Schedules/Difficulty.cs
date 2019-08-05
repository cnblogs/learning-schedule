namespace Cnblogs.Academy.Domain.Schedules
{
    public enum Difficulty
    {
        easy = 1,
        suitable = 2,
        difficult = 3
    }

    public static class DifficultyExtensions
    {
        public static string ToHumanString(this Difficulty difficulty)
        {
            switch ((int)difficulty)
            {
                case 1:
                    return "比较容易的";
                case 2:
                    return "难度适中";
                case 3:
                    return "有点难的";
                default:
                    return "";
            }
        }
    }
}
