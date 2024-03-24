using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Web.Infrastructure;

public partial class SuptomerRouteProvider : IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantHome",
            pattern: "Merchant",
            defaults: new { controller = "MerchantHome", action = "Index" });

        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantProfile",
            pattern: "Merchant/Profile",
            defaults: new { controller = "MerchantProfile", action = "Index" });

        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantProfileFileDownload",
            pattern: "Merchant/Profile/DownloadFile/{id}",
            defaults: new { controller = "MerchantProfile", action = "DownloadFile" });

        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantProfileBranch",
            pattern: "MerchantProfile/SaveBranch",
            defaults: new { controller = "MerchantProfile", action = "SaveBranch" });

        endpointRouteBuilder.MapControllerRoute(
            name: "SupplierHome",
            pattern: "Admin/Supplier",
            defaults: new { area = "Admin", controller = "SupplierHome", action = "Index" });

        endpointRouteBuilder.MapControllerRoute(
            name: "SupplierProfile",
            pattern: "SupplierProfile",
            defaults: new { controller = "SupplierProfile", action = "Index" });

        endpointRouteBuilder.MapControllerRoute(
            name: "SupplierProfileFileDownload",
            pattern: "SupplierProfile/DownloadFile/{id}",
            defaults: new { controller = "SupplierProfile", action = "DownloadFile" });

        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantProfileGetBranch",
            pattern: $"MerchantProfile/GetBranch/{{branchId}}",
            defaults: new { controller = "MerchantProfile", action = "GetBranch" });

        endpointRouteBuilder.MapControllerRoute(
            name: "MerchantProfileGetAllBranch",
            pattern: $"MerchantProfile/GetAllBranches",
            defaults: new { controller = "MerchantProfile", action = "GetAllBranches" });

        endpointRouteBuilder.MapControllerRoute(
           name: "MerchantProfileDeleteBranch",
           pattern: $"MerchantProfile/DeleteBranch/{{branchId}}",
           defaults: new { controller = "MerchantProfile", action = "DeleteBranch" });
    }
    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}
