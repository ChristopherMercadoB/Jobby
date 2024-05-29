using Jobby.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Persistence
{
    public static class ServiceExtension
    {
        public static void AddInfrastructurePersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationContext>(op=>op.UseSqlServer(config.GetConnectionString("DefaultConnection"), m=>m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

        }
    }
}
