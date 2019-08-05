namespace Cnblogs.Academy.Common
{
    //https://q.cnblogs.com/q/90558/

    public class BooleanResult<T> : BooleanResult
    {
        public static BooleanResult<T> Succeed(T value, string message = "")
        {
            return new BooleanResult<T> { Success = true, Value = value, Message = message };
        }

        public T Value { get; set; }
    }
}
