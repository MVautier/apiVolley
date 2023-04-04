using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IEncryption
    {
        string GeneratePasswordHash(string value);
        bool CompareWithPasswordHash(string value, string hash);
        string DecryptElement(string hash);
    }
}
