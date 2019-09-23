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
        public string Username { get; set; }
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
    public class UserInfoModel
    {
        private static UserInfoModel instance = null;

        public static UserInfoModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserInfoModel();
                }
                return instance;
            }
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string UserType { get; set; }
        public string ExpiredPassword { get; set; }
        public DateTime? PasswordLastUpdate { get; set; }
    }
}
