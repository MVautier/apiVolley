using ApiColomiersVolley.BLL.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Exceptions
{
    public class InactiveMailException : GenericException
    {
        public InactiveMailException() : base("InactiveMailException", HttpStatusCode.Forbidden, "mail is inactive")
        {
        }
    }
}
