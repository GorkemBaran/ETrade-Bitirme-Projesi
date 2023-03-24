#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class ReportFilterModel
    {
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("-- All Categories --")]
        public int? CategoryId { get; set; }

        [DisplayName("Unit Price")]
        public double? UnitPriceBegin { get; set; }

        public double? UnitPriceEnd { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDateBegin { get; set; }

        public DateTime? ExpirationDateEnd { get; set; }

        [DisplayName("Store")]
        public List<int> StoreIds { get; set; }
    }
}
