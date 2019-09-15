using System;
using System.ComponentModel.DataAnnotations;

namespace Asset.Common.ViewModel
{
    public class BaseUserModel : BaseModel
    {
        #region UserModel
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string UserTypeStr { get; set; }
        #endregion
    }

    public class UserModel : BaseUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Address { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserProfileModel : BaseUserModel
    {
        public string Username { get; set; }
        public string Address { get; set; }
    }

    public class ModifiedInfoUserModel : BaseUserModel
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
