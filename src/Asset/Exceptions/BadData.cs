using System;
using System.Runtime.Serialization;

namespace Asset.Common.Exceptions
{
    public class BadData : Exception
    {
        public BadData()
        {
        }

        public BadData(string message) : base(message)
        {
        }

        public BadData(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadData(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}