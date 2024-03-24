using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Sup.Plugin.Payment.ClickPay
{
    public class RouteProvider 
    {

        public void RegisterRoutes(IRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapRoute("Sup.Plugin.Payment.ClickPay", "plugins/Payment/ClickPay",
                 new { controller = "Payment", action = "DataDeletionCallback" });

        }
        public int Priority => 0;

    }
}
