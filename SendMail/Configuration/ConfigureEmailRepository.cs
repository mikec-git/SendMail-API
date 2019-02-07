using Microsoft.Extensions.DependencyInjection;
using SendMail.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendMail.Configuration
{
    public static class ConfigureEmailRepository
    {
        public static void ConfigureEmailRepo(this IServiceCollection services)
        {
            services.AddScoped<IEmailRepository, EmailRepository>();
        }
    }
}
