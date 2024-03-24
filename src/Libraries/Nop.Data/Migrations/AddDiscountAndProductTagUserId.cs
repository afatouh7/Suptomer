using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;

namespace Nop.Data.Migrations;

[NopMigration("2023/12/01 12:00:00:0000000", "Product Tag and Discount. Add user id",
    UpdateMigrationType.Data,
    MigrationProcessType.Update)]
public class AddDiscountAndProductTagUserId : AutoReversingMigration
{
    public override void Up()
    {
        Create.Column(nameof(ProductTag.UserId))
            .OnTable(nameof(ProductTag))
            .AsInt32()
            .Nullable();

        Create.Column(nameof(Discount.UserId))
            .OnTable(nameof(Discount))
            .AsInt32()
            .Nullable();
    }
}
