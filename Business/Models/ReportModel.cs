#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class ReportModel
    {
        #region Display
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDate { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Store")]
        public string StoreName { get; set; }

        [DisplayName("Virtual")]
        public string Virtual { get; set; }
        #endregion

        #region Filter
        public int? CategoryId { get; set; }
        public double UnitPriceValue { get; set; }
        public DateTime? ExpirationDateValue { get; set; }
        public int? StoreId { get; set; }
        #endregion
    }
}
