namespace Nop.Web.Models.Catalog
{
    public class ProductsMixInfoModel
    {
        public int ProductId { get; set; }
        public int ProductType { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class ProductsMixRequestModel
    {
        public string ProductIds { get; set; }
        public int Status { get; set; } = 1;
    }
}
