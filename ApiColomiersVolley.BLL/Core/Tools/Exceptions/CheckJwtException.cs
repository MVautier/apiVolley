using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Exceptions
{
    public class CheckJwtException : Exception
    {
        public CheckJwtException() : base("Le token n'est pas valide") { }
        public CheckJwtException(Exception inner) : base("Le token n'est pas valide", inner) { }
    }
}
