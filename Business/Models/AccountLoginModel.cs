#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AccountLoginModel
    {
        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }
    }
}