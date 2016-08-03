using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calorie.Models
{

    public class UserEditTeam
    {
        [Required]
        public bool IsAdmin{ get; set; }

        [Required]
        public int TeamID{ get; set; }

        [Required]
        public string UserID{ get; set; }
    }
    
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username Or Email Address")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterFurtherDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> PreferredActivities{ get; set; }

    }

        public class RegisterViewModel
    {

        public bool IsSuperAdmin { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Profile Picture")]
        public int? ProfilePictureImageID { get; set; }

        [Display(Name = "Emails From Third Parties")]
        public bool ThirdPartyEmails { get; set; }

        [Display(Name = "Emails From Us")]
        public bool FirstPartyEmails { get; set; }

        [Display(Name = "Accept Terms & Conditions")]
        public bool AcceptTAndCs { get; set; }

        public bool IsSponsor { get; set; }
        public bool IsExercisor { get; set; }
        public bool IsCorporate { get; set; }

    }

    public class RegisterJustGiving
    {

        public RegisterJustGiving()
        {
            Countries = BusinessLogic.JustGivingLogic.getCountrys();

        }
        
        public Dictionary<string,string> Countries { get; set; }


        [Required]
        [Display(Name = "Title")]
        public string title { get; set; }
        //The user''s title. One of "Mr", "Mrs", "Miss", "Ms", "Dr", "Other" (Required).

        [Display(Name = "First Name")]
        [Required]
        public string firstname{ get; set; }
        //The user''s firstName (Required).

        [Display(Name = "Last Name")]
        [Required]
        public string lastname { get; set; }
        //The user''s lastName (Required).

        [Display(Name = "Address Line 1")]
        [Required]
        public string AddressLine1 { get; set; }
        //The first line of the of the address where the user resides(Required).

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        //The second line of the of the address where the user resides(Optional).

        [Display(Name = "Town Or City")]
        [Required]
        public string AddressTownOrCity { get; set; }
        //The town or city where the user resides(Required).

        [Display(Name = "Country")]
        [Required]
        public string country { get; set; }
        //The country where the user resides(Required). A list of allowable countries is available via the Countries API.

        [Display(Name = "Postcode Or Zip")]
        [Required]
        public string postcodeOrZip { get; set; }
        //The postcode or zip of the address where the user resides(Required).

        //[Required]
        //public string email { get; set; }
        ////The user''s email (Required).

        //[Required]
        //public string password { get; set; }
        ////The user''s password should be at least 8 characters long (Required).

        //[Required]
        //public bool acceptTandC { get; set; }
        ////A Boolean indicating whether user accepts JustGiving''s Terms and conditions. Note providing false will fail validation (Required).
        

      


    }

    public class EditViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> PreferredActivities { get; set; }
        public List<string> PreferredjustGivingCharities { get; set; }
        public List<Teams.Team> TopTeams { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

  
}
