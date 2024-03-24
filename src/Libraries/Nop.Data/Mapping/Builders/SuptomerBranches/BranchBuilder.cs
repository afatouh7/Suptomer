using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.SuptomerBranches;

namespace Nop.Data.Mapping.Builders.SuptomerBranches;
public partial class BranchBuilder : NopEntityBuilder<Branch>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Branch.BranchName)).AsString(1000).NotNullable()
            .WithColumn(nameof(Branch.BranchManager)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchMobileNumber)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchLocation)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchTimeframeFrom)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchTimeframeTo)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchTimeZoneId)).AsString(1000).Nullable()
            .WithColumn(nameof(Branch.BranchUserId)).AsInt32().NotNullable();
    }
}
