using ApiColomiersVolley.BLL;
using ApiColomiersVolley.DAL;
using ApiColomiersVolley.Security;
using ApiColomiersVolley.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiColomiersVolley
{
    public static class LocatorService
    {
        public static void InitLocator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IOAuthHelper, OAuthHelper>();
            services.AddDbContext<ColomiersVolleyContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("ColomiersVolley")));

            services.AddBusinessLocator();
            services.AddDataLayerLocator();
            services.AddHttpContextAccessor();
        }
    }
}
