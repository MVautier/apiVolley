using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Business;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Business;
using ApiColomiersVolley.BLL.DMGallery.Business.Interfaces;
using ApiColomiersVolley.BLL.DMGallery.Business;

namespace ApiColomiersVolley.BLL
{
    public static class ServiceLocator
    {
        public static void AddBusinessLocator(this IServiceCollection services)
        {
            // Businesses
            services.AddScoped<IBSAuthentication, BSAuthentication>();
            services.AddScoped<IBSLogin, BSLogin>();
            services.AddScoped<IBSItem, BSItem>();
            services.AddScoped<IBSConnexion, BSConnexion>();
            services.AddScoped<IBSGallery, BSGallery>();

            // Core tools
            services.AddScoped<IJWTFactory, JWTFactory>();
            services.AddScoped<IServiceSendMail, SendMail>();
            services.AddScoped<IEncryption, Encryption>();
        }
    }
}
