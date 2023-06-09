﻿#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Store : RecordBase, ISoftDelete // Mağaza
    {
        [Required]
        [StringLength(150)]
        [DisplayName("Store Name")]
        public string Name { get; set; }

        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductStore> ProductStores { get; set; }
    }
}
