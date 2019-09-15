using System;
using Asset.Common.Timing.Abstractions;

namespace Asset.Common.Timing
{
    public class DotNetClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTimeKind Kind => DateTimeKind.Local;

        public DateTime? NormalizeNullable(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            if (dateTime.Value.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Local);
            }

            if (dateTime.Value.Kind == DateTimeKind.Utc)
            {
                return dateTime.Value.ToLocalTime();
            }

            return dateTime;
        }

        public DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }

            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.ToLocalTime();
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