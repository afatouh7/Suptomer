using FluentMigrator;
using Nop.Core.Domain.Shipping;

namespace Nop.Data.Migrations;

[NopMigration("2023/10/14 12:45:00:0000000", "Warehouse. Add user id",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddWarehouseUserId : AutoReversingMigration
{
    public override void Up()
    {
        Create.Column(nameof(Warehouse.UserId))
            .OnTable(nameof(Warehouse))
            .AsInt32()
            .Nullable();
    }
}
