using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IJWTFactory
    {
        string GenerateJWT<T>(T value);
        T CheckAndExtract<T>(string token);
    }
}
