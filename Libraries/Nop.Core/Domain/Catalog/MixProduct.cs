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
    public int CustomerId { get; set; }

    /// <summary>
    /// order Id
    /// </summary>
    public int? OrderId { get; set; }
    /// <summary>
    /// Danh sách các sản phẩm thành phần
    /// </summary>
    public string ProductIds { get; set; }
    /// <summary>
    /// Trạng thái : 1: in Cart ; 2: Payment 
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public int? Note { get; set; }
    /// <summary>
    /// Trang thái xoá
    /// </summary>
    /// 
    public bool Deleted { get; set; }

}