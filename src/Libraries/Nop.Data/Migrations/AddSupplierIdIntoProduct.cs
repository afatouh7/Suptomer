using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;

namespace Nop.Data.Migrations;

[NopMigration("2024/03/13 12:00:00:0000000", "Order. Add SupplierContractId",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddSupplierIdIntoOrder : AutoReversingMigration
{
    public override void Up()
    {
        Create.Column(nameof(Order.SupplierContractId))
            .OnTable(nameof(Order))
            .AsInt32()
             .Nullable();
    }
}
