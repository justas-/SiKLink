using System;
using System.Collections.Generic;
using System.Text;

namespace SiKLink
{
    [Serializable]
    public class NotInCommandModeException : Exception
    {
        public NotInCommandModeException() { }
        public NotInCommandModeException(string message) : base(message) { }
        public NotInCommandModeException(string message, Exception inner) : base(message, inner) { }
        protected NotInCommandModeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
