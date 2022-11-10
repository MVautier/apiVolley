using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL
{
    public class DbContextFactory //: IDesignTimeDbContextFactory<ColomiersVolleyContext>
    {
        //public ColomiersVolleyContext CreateDbContext(string[] args)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    var dbContextBuilder = new DbContextOptionsBuilder<ColomiersVolleyContext>();

        //    var connectionString = configuration.GetConnectionString("ColomiersVolley");

        //    dbContextBuilder.UseMySQL(connectionString);

        //    return new ColomiersVolleyContext(dbContextBuilder.Options);
        //}
    }
}
