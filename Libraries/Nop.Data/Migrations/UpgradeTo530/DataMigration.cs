using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations.UpgradeTo530
{
    [NopMigration("2025-03-03 01:00:00", "5.30", UpdateMigrationType.Data)]
    public class UpdateMixProductTable : Migration
    {
        public override void Up()
        {
            Console.WriteLine($"Assembly Name: {this.GetType().Assembly.FullName}");
           
            Alter.Table("MixProduct")
                .AddColumn("CustomerId").AsInt32().NotNullable()
                .AddColumn("Status").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("CustomerId").FromTable("MixProduct");
            Delete.Column("Status").FromTable("MixProduct");
        }
    }
}