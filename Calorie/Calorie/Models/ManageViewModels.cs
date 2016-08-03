using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Calorie.Models
{
    public class ManageIndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool TwoFactor { get; set; }
        //public bool BrowserRemembered { get; set; }

        public ApplicationUser User { get; set; }

        public List<string> PreferredActivities;

    }

   

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    //public class SetPasswordViewModel
    //{
    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "New password")]
    //    public string NewPassword { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm new password")]
    //    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    //public class WelcomeEmailViewModel
    //{
    //    public string Name { get; set; }
    //    public string EmailConfirmURL { get; set; }
    //    public string RootURL { get; internal set; }
    //}

    public class GenericEmailViewModel
    {

        public GenericEmailViewModel()
        {
            Buttons = new List<GenericEmailButtonViewModel>();
        }

        public string PlaintextMessage{ get; set; }

        public string HTMLContent { get; set; }
        public string RootURL { get; internal set; }
        public string Name { get; set; }
        public List<GenericEmailButtonViewModel> Buttons { get; set; }

    }

    public class GenericEmailButtonViewModel
    {
        public string Text { get; set; }
        public string URL { get; set; }
    }


    //public class PasswordResetEmailViewModel
    //{
    //    public string ResetURL { get; set; }
    //    public string RootURL { get; internal set; }
    //}


    //    public class JustGivingSignUpEmailViewModel
    //    {
    //        public string Name { get; set; }
    //        public string EmailAddress { get; set; }
    //        public string Password { get; set; }
    //        public string RootURL { get; internal set; }
    //    }


}