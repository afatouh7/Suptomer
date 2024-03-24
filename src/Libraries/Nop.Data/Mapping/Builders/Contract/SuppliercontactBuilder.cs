using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Contract;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers
{
    /// <summary>
    /// Represents a customer address mapping entity builder
    /// </summary>
    //public partial class SuppliercontactBuilder : NopEntityBuilder<SupplierContract>
    //{
    //    #region Methods

    //    /// <summary>
    //    /// Apply entity configuration
    //    /// </summary>
    //    /// <param name="table">Create table expression builder</param>
    //    public override void MapEntity(CreateTableExpressionBuilder table)
    //    {
    //        table
    //            .WithColumn(nameof(SupplierContract.StartDate)).AsDateTime().NotNullable()
    //            .WithColumn(nameof(SupplierContract.EnterDate)).AsDateTime().NotNullable()
    //            .WithColumn(nameof(SupplierContract.Interval)).AsString(255).NotNullable()
    //            .WithColumn(nameof(SupplierContract.Deleted)).AsBoolean().NotNullable()
    //            .WithColumn(nameof(SupplierContract.LimitedToStores)).AsBoolean().NotNullable();

    //        // Add foreign key column for Product
    //        table
    //            .WithColumn(nameof(SupplierContract.Products)).AsInt32().NotNullable().ForeignKey("Products", "Id");
    //    }

    //    #endregion
    //}
}