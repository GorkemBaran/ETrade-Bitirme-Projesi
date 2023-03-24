#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        #region Entity Özellikleri
        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        #endregion


        public string RoleNameDisplay { get; set; }

        public UserDetailModel UserDetail { get; set; }
    }
}
