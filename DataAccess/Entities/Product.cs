#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    //[Table("Urunler")]
    public class Product : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public int StockAmount { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CategoryId { get; set; }

        [StringLength(5)]
        public string ImgExtension { get; set; }

        [Column(TypeName="image")]
        public byte[] Image { get; set; } // varbinary(MAX), image

        public Category Category { get; set; }

        public List<ProductStore> ProductStores { get; set; }
    }
}
