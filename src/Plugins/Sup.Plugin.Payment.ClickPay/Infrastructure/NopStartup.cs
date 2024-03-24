using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Sup.Plugin.Payment.ClickPay.Helpers;

namespace Sup.Plugin.Payment.ClickPay.Infrastructure
{
    public class NopStartup : INopStartup

    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ClickPayPaymentService>();
        }
        public void Configure(IApplicationBuilder application)
        {

        }

      
        public int Order => 3000;
    }
}
