using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sup.Plugin.Supplier
{
    public class RouteProvider
    {
        public void RegisterRoutes(IRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapRoute("Sup.Plugin.Supplier", "Sup/Plugin/Supplier",
                 new { controller = "AdminSuplierContract", action = "List" });

        }
        public int Priority => 0;

    }
}
