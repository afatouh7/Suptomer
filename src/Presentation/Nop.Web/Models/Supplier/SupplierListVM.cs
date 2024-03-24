using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Supplier;

public record SupplierListVM : BaseNopEntityModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
}

public record SupplierGridVM : BasePagedListModel<SupplierListVM>
{
}

public record SupplierSearchModel : BaseSearchModel { }

public record PaymentListVM : BaseNopEntityModel
{
    public string Date { get; set; }
    public string OrderNumber { get; set; }
    public string Amount { get; set; }
    public string Products { get; set; }
}

public record PaymentGridVM : BasePagedListModel<PaymentListVM>
{
}

public record PaymentSearchModel : BaseSearchModel { }