#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Business.Models
{
    // MVC Fluent Validation
    public class ProductModel : RecordBase
    {
        #region Entity Özellikleri
        [Required(ErrorMessage = "{0} is required!")]
        //[StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be betwwen {1} and {2}!")]
        [Required(ErrorMessage = "{0} is required!")]
        public double? UnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        [Range(0, 1000, ErrorMessage = "{0} must be betwwen {1} and {2}!")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        [DisplayName("Category")]
        public int? CategoryId { get; set; } // DropDownList

        [JsonIgnore]
        public string ImgExtension { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
        #endregion

        #region Sayfanın İhtiyacı Olan Özellikler
        [DisplayName("Unit Price")]
        [JsonIgnore]
        public string UnitPriceDisplay { get; set; }

        [DisplayName("Expiration Date")]
        [JsonIgnore]
        public string ExpirationDateDisplay { get; set; }

        [DisplayName("Category")]
        [JsonIgnore]
        public string CategoryDisplay { get; set; }

        [DisplayName("Stores")]
        [JsonIgnore]
        public string StoresDisplay { get; set; }

        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; } // ListBox

        [DisplayName("Image")]
        [JsonIgnore]
        public string ImgSrcDisplay { get; set; }
        #endregion
    }
}
