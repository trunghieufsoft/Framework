using System;
using Asset.Common.Timing.Abstractions;

namespace Asset.Common.Timing
{
    public static class Clock
    {
        private static IClockProvider _provider;

        /// <summary>
        /// This object is used to perform all <see cref="Clock"/> operations.
        /// Default value: <see cref="DotNetClockProvider"/>.
        /// </summary>
        public static IClockProvider Provider
        {
            get => _provider;
            set
            {
                if (value == null)
                {
                    throw new ApplicationException("Can not set Clock to null!");
                }

                _provider = value;
            }
        }

        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// </summary>
        public static DateTime Now => Provider.Now;

        public static DateTimeKind Kind => Provider.Kind;

        static Clock()
        {
            Provider = new DotNetClockProvider();
        }

        /// <summary>
        /// Normalizes given <see cref="DateTime"/> using current <see cref="Provider"/>.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized.</param>
        /// <returns>Normalized DateTime</returns>
        public static DateTime? NormalizeNullable(DateTime? dateTime)
        {
            return Provider.NormalizeNullable(dateTime);
        }

        /// <summary>
        /// Normalizes given <see cref="DateTime"/> using current <see cref="Provider"/>.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized.</param>
        /// <returns>Normalized DateTime</returns>
        public static DateTime Normalize(DateTime dateTime)
        {
            return Provider.Normalize(dateTime);
        }

        public static object NormalizeObject(object @object)
        {
            return Provider.NormalizeObject(@object);
        }
    }
}