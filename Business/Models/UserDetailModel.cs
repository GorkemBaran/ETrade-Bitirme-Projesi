#nullable disable

using DataAccess.Entities;
using DataAccess.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserDetailModel
    {
        public int UserId { get; set; }

        public Sex Sex { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(25)]
        [Required]
        [Phone]
        public string Phone { get; set; }

        [StringLength(500)]
        [Required]
        public string Address { get; set; }

        [Required]
        [DisplayName("City")]
        public int? CityId { get; set; }

        [Required]
        [DisplayName("Country")]
        public int? CountryId { get; set; }
    }
}
