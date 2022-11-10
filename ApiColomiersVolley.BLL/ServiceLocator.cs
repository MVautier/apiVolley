using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools;
using ApiColomiersVolley.BLL.DMArticle.Business;
using ApiColomiersVolley.BLL.DMArticle.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Business;

namespace ApiColomiersVolley.BLL
{
    public static class ServiceLocator
    {
        public static void AddBusinessLocator(this IServiceCollection services)
        {
            // Businesses
            services.AddScoped<IBSArticle, BSArticle>();
            services.AddScoped<IBSAuthentication, BSAuthentication>();
            services.AddScoped<IBSLogin, BSLogin>();

            // Core tools
            services.AddScoped<IJWTFactory, JWTFactory>();
            services.AddScoped<IServiceSendMail, SendMail>();
        }
    }
}
