using FluentMigrator;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Migrations;

[NopMigration("2023/08/20 12:00:00:0000000", "Customer. Add user type",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class ChangeCustomerUserTypeToAllowNull : AutoReversingMigration
{
    public override void Up()
    {
        Alter.Table(nameof(Customer))
            .AlterColumn(nameof(Customer.UserType))
            .AsInt32()
            .Nullable();
    }
}
