using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo500
{
    [NopUpdateMigration("2025-03-02 00:00:00", "4.40", UpdateMigrationType.Data)]
    public class AddPriceToProductAttributeMigration : Migration
    {
        public override void Up()
        {
            var productAttributeTableName = NameCompatibilityManager.GetTableName(typeof(ProductAttribute));

            // Add column Price if it doesn't exist
            if (!Schema.Table(productAttributeTableName).Column("Price").Exists())
            {
                Alter.Table(productAttributeTableName)
                    .AddColumn("Price").AsDecimal().NotNullable().SetExistingRowsTo(0);
            }
        }

        public override void Down()
        {
            var productAttributeTableName = NameCompatibilityManager.GetTableName(typeof(ProductAttribute));

            // Remove column Price if it exists
            if (Schema.Table(productAttributeTableName).Column("Price").Exists())
            {
                Delete.Column("Price").FromTable(productAttributeTableName);
            }
        }
    }
}