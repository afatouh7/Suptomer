using FluentMigrator;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Migrations;

[NopMigration("2023/08/19 12:00:00:0000000", "Customer. Add user type",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddCustomerUserType : AutoReversingMigration
{
    public override void Up()
    {
        Create.Column(nameof(Customer.UserType))
            .OnTable(nameof(Customer))
            .AsInt32()
            .SetExistingRowsTo(0)
            .NotNullable();
    }
}
