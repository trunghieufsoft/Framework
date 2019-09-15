using System;
using System.Runtime.Serialization;

namespace Asset.Common.Exceptions
{
    public class Dupplication : Exception
    {
        public Dupplication()
        {
        }

        public Dupplication(string message) : base(message)
        {
        }

        public Dupplication(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Dupplication(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}