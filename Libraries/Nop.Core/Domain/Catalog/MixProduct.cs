using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Catalog;

/// <summary>
/// Sản phẩm kết hợp
/// </summary>
public partial class MixProduct : BaseEntity, ISoftDeletedEntity
{
    /// <summary>
    /// Danh sách các sản phẩm thành phần
    /// </summary>
    public string ProductIds { get; set; }
    /// <summary>
    /// Trang thái xoá
    /// </summary>
    public bool Deleted { get; set; }

}