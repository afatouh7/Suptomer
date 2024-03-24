using FluentMigrator;
using Nop.Core.Domain.Contract;
using Nop.Core.Domain.SuptomerBranches;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;

[NopMigration("2024/03/11 22:45:00:0000000", "Add SupplierContract table",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddSupplierContract : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SupplierContract>();
    } 

}
