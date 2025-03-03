using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo532
{
    [NopMigration("2025-03-04 02:00:00", "5.30", UpdateMigrationType.Data)]
    public class UpdateMixProductTable : Migration
    {
        public override void Up()
        {
            Console.WriteLine($"Assembly Name: {this.GetType().Assembly.FullName}");
           
            Alter.Table("MixProduct")
                .AlterColumn("OrderId").AsInt32().Nullable()
                .AlterColumn("Status").AsInt32().NotNullable()
                .AlterColumn("Note").AsString(int.MaxValue).Nullable();

            Delete.Column("Deleted").FromTable("MixProduct");
        }

        public override void Down()
        {
            Delete.Column("OrderId").FromTable("MixProduct");
            Delete.Column("Note").FromTable("MixProduct");
        }
    }
}