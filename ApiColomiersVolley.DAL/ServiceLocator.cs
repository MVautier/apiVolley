using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using ApiColomiersVolley.BLL.DMItem.Repositories;
using ApiColomiersVolley.BLL.DMUser.Repositories;
using ApiColomiersVolley.DAL.DataProviders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL
{
    public static class ServiceLocator
    {
        public static void AddDataLayerLocator(this IServiceCollection services)
        {
            services.AddScoped<IDMItemRepo, DPItem>();
            services.AddScoped<IDMUserRepo, DPUser>();
            services.AddScoped<IDMUser, DPUser>();
            services.AddScoped<IDMConnexionRepo, DPConnexion>();
            services.AddScoped<IDMTokenRepo, DPToken>();
            services.AddScoped<IDMArticlePageRepo, DPArticlePage>();
            services.AddScoped<IDMAdherentRepo, DPAdherent>();
            services.AddScoped<IDMSectionRepo, DPSection>();
            services.AddScoped<IDMCategoryRepo, DPCategory>();
            services.AddScoped<IDMRoleRepo, DPRole>();
        }
    }
}
