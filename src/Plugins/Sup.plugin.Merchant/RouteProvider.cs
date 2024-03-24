using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sup.Plugin.Merchant
{
    public class RouteProvider
    {
        public void RegisterRoutes(IRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapRoute("Sup.Plugin.Merchant", "Sup/Plugin/Merchant",
                 new { controller = "Contract", action = "List" });

        }
        public int Priority => 0;

    }
}
