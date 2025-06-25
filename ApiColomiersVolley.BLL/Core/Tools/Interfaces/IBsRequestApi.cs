using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IBsRequestApi
    {
        Task<T> CallWithToken<T>(string fournisseur, string route, string token, HttpMethod method);
        Task<T> PostForm<T>(string fournisseur, string route, string clientId, string clientSecret);
        Task<T> PostJsonWithToken<T>(string fournisseur, string route, string token, object body, HttpStatusCode statusToCheck = HttpStatusCode.NoContent);
        Task<T> PostFormData<T>(string fournisseur, string route, FormUrlEncodedContent data);
        Task<T> PostFormDataWithToken<T>(string fournisseur, string route, string token, FormUrlEncodedContent data);
    }
}
