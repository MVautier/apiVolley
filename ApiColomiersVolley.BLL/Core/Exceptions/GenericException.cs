using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Exceptions
{
    public abstract class GenericException : Exception
    {
        public string CodeException;

        public HttpStatusCode httpStatusCode { get; }

        public new virtual object Data { get; set; }

        public GenericException(string codeException, HttpStatusCode httpStatusCode, string message)
            : base(message)
        {
            CodeException = codeException ?? GetType().Name;
            this.httpStatusCode = httpStatusCode;
        }
    }
}
