#nullable disable

namespace Business.Models
{
    public class CartItemGroupByModel
    {
        public int ProductId { get; set; } // çoklayan
        public int UserId { get; set; } // çoklayan
        public string ProductName { get; set; } // çoklayan
        public double TotalUnitPrice { get; set; } // gruplama sonucunda aggregate fonksiyon ile hesaplanacak
        public int TotalCount { get; set; } // gruplama sonucunda aggregate fonksiyon ile hesaplanacak
        public string TotalUnitPriceDisplay { get; set; }
    }
}
