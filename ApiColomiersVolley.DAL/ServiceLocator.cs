using ApiColomiersVolley.BLL.DMArticle.Repositories;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
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
            services.AddScoped<IDMArticleRepo, DPArticle>();
            services.AddScoped<IDMUserRepo, DPUser>();
        }
    }
}
