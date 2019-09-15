using System.ComponentModel.DataAnnotations;

namespace Asset.Common.Provider.Enumerable
{
    public enum SendType
    {
        [Display(Description = "New Password")]
        NewPassword = 0,
        [Display(Description = "Reset Password")]
        ResetPassword = 1,
        [Display(Description = "Reset Company Info")]
        ResetCompanyInfo = 2
    }
}
