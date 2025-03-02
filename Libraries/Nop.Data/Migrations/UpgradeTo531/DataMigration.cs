using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo531
{
    [NopMigration("2025-03-03 02:00:00", "5.30", UpdateMigrationType.Data)]
    public class UpdateMixProductTable : Migration
    {
        public override void Up()
        {
            Console.WriteLine($"Assembly Name: {this.GetType().Assembly.FullName}");
           
            Alter.Table("MixProduct")
                .AddColumn("OrderId").AsInt32().Nullable()
                .AddColumn("Note").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("OrderId").FromTable("MixProduct");
            Delete.Column("Note").FromTable("MixProduct");
        }
    }
}