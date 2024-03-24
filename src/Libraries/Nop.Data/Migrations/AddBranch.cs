using FluentMigrator;
using Nop.Core.Domain.SuptomerBranches;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;

[NopMigration("2023/10/25 22:45:00:0000000", "Add branch table",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddBranch : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Branch>();
    }
}
