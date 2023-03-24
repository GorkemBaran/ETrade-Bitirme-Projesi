#nullable disable

using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;

namespace DataAccess.Entities
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }

        public Sex Sex { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(500)]
        [Required]
        public string Address { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
