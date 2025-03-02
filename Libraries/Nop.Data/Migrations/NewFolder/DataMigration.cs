using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo500
{
    [NopMigration("2025-03-02 00:00:00", "5.10", UpdateMigrationType.Data)]
    public class AddMixProductTable : Migration
    {
        public override void Up()
        {
            Create.Table("MixProduct")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ProductIds").AsString(int.MaxValue).Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MixProduct");
        }
    }
}