using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Builders.Catalog;

/// <summary>
/// Represents a category entity builder
/// </summary>
public partial class MixProductBuilder : NopEntityBuilder<MixProduct>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(MixProduct.Id)).AsInt32().PrimaryKey().Identity()
             .WithColumn(nameof(MixProduct.ProductIds)).AsString(int.MaxValue).NotNullable()
             .WithColumn(nameof(MixProduct.CustomerId)).AsInt32().NotNullable()
             .WithColumn(nameof(MixProduct.OrderId)).AsInt32().Nullable()
             .WithColumn(nameof(MixProduct.Status)).AsInt32().NotNullable()
             .WithColumn(nameof(MixProduct.Note)).AsString(int.MaxValue).Nullable();
    }

    #endregion
}