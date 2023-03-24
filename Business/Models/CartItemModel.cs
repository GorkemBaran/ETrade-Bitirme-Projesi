#nullable disable

namespace Business.Models
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
    }
}
