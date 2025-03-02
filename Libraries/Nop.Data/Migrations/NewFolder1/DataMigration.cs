using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo510
{
    [NopUpdateMigration("2025-03-02 00:00:00", "4.40", UpdateMigrationType.Data)]
    public class AddIsMixColumnToProductAttribute : Migration
    {
        public override void Up()
        {
            var productAttributeTableName = NameCompatibilityManager.GetTableName(typeof(ProductAttribute));

            // Add column Price if it doesn't exist
            if (!Schema.Table(productAttributeTableName).Column("IsMixed").Exists())
            {
                Alter.Table(productAttributeTableName)
                    .AddColumn("IsMixed").AsBoolean().NotNullable().SetExistingRowsTo(0);
            }
        }

        public override void Down()
        {
            var productAttributeTableName = NameCompatibilityManager.GetTableName(typeof(ProductAttribute));

            // Remove column Price if it exists
            if (Schema.Table(productAttributeTableName).Column("IsMixed").Exists())
            {
                Delete.Column("IsMixed").FromTable(productAttributeTableName);
            }
        }
    }
}

