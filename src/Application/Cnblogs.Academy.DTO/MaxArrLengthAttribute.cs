using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cnblogs.Academy.DTO
{
    public class MaxArrLengthAttribute : ValidationAttribute
    {
        protected readonly int _length;

        public MaxArrLengthAttribute(int length, string errorMessage) : base(errorMessage)
        {
            _length = length;
        }

        public override bool IsValid(object value)
        {
            if (value is ICollection == false) { return false; }
            return ((ICollection)value).Count <= _length;
        }
    }

    public class MinArrLengthAttribute : MaxArrLengthAttribute
    {
        public MinArrLengthAttribute(int length, string errorMessage) : base(length, errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value is ICollection == false) { return false; }
            return ((ICollection)value).Count >= _length;
        }
    }
}
