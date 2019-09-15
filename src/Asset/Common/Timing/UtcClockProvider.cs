using System;
using Asset.Common.Timing.Abstractions;

namespace Asset.Common.Timing
{
    public class UtcClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTimeKind Kind => DateTimeKind.Utc;

        public DateTime? NormalizeNullable(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            if (dateTime.Value.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
            }

            if (dateTime.Value.Kind == DateTimeKind.Local)
            {
                return dateTime.Value.ToUniversalTime();
            }

            return dateTime;
        }

        public DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }

            return dateTime;
        }

        public object NormalizeObject(object @object)
        {
            if (@object is DateTime d)
            {
                return Normalize(d);
            }

            return @object;
        }
    }
}