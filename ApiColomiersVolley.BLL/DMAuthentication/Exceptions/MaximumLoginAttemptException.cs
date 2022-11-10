using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Exceptions
{
    public class MaximumLoginAttemptException : Exception
    {
        public MaximumLoginAttemptException() { }

        public MaximumLoginAttemptException(string message) : base(message) { }

        public MaximumLoginAttemptException(string message, Exception inner) : base(message, inner) { }
    }
}
