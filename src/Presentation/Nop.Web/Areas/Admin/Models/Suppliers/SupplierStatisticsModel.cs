using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Suppliers;

public record SupplierStatisticsModel : BaseNopModel
{
    public int NumberOfOrders { get; set; }
    public decimal NumberOfOrdersPercentage { get; set; }

    public decimal TotalIncome { get; set; }
    public decimal TotalIncomePercentage { get; set; }

    public int NumberOfSalesReturn { get; set; }
    public decimal NumberOfSalesReturnPercentage { get; set; }

    public int NumberOfInvoices { get; set; }
    public decimal NumberOfInvoicesPercentge { get; set; }
}
