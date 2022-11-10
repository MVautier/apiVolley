using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() { }

        public InvalidPasswordException(string message) : base(message) { }

        public InvalidPasswordException(string message, Exception inner) : base(message, inner) { }
    }
}
