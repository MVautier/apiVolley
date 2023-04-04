using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Business;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Business;
using ApiColomiersVolley.BLL.DMGallery.Business.Interfaces;
using ApiColomiersVolley.BLL.DMGallery.Business;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMFile.Business.Interfaces;
using ApiColomiersVolley.BLL.DMFile.Business;

namespace ApiColomiersVolley.BLL
{
    public static class ServiceLocator
    {
        public static void AddBusinessLocator(this IServiceCollection services)
        {
            // Business
            services.AddScoped<IBSAuthentication, BSAuthentication>();
            services.AddScoped<IBSLogin, BSLogin>();
            services.AddScoped<IBSItem, BSItem>();
            services.AddScoped<IBSConnexion, BSConnexion>();
            services.AddScoped<IBSGallery, BSGallery>();
            services.AddScoped<IBSAdherent, BSAdherent>();
            services.AddScoped<IBSCategory, BSCategory>();
            services.AddScoped<IBSDocument, BSDocument>();

            // Core tools
            services.AddScoped<IJWTFactory, JWTFactory>();
            services.AddScoped<IServiceSendMail, SendMail>();
            services.AddScoped<IEncryption, Encryption>();
            services.AddScoped<IFileManager, FileManager>();
        }
    }
}
