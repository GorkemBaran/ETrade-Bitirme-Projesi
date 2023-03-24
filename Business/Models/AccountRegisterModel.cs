#nullable disable

using DataAccess.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AccountRegisterModel
    {
        #region User
        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        [Compare("ConfirmPassword", ErrorMessage = "Password and confirm password must be same!")]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string ConfirmPassword { get; set; }
        #endregion

        #region UserDetail
        public UserDetailModel UserDetail { get; set; }
        #endregion
    }
}