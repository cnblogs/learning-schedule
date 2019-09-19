namespace Cnblogs.Academy.Common
{
    public class BooleanResult
    {
        public BooleanResult() { }

        public BooleanResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public static BooleanResult Succeed(string message = "")
        {
            return new BooleanResult { Success = true, Message = message };
        }

        public static BooleanResult Fail(string message = "")
        {
            return new BooleanResult { Success = false, Message = message };
        }
    }
}