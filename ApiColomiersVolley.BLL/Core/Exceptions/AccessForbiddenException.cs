using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public AccessForbiddenException(string? message)
            : base(string.Format("Message: {0}", message))
        {
        }

        public AccessForbiddenException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected AccessForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
