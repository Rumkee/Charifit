using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calorie.Models.Companys
{


    public class AllCompanysVM
    {
        public List<ApplicationUser>  Companys{ get; set; }
    }


    public class EditCompanyVM
    {

        public EditCompanyVM() { }

        public EditCompanyVM(ApplicationUser User)
        {
            CompanyName = User.UserName;
            CompanyDescription = User.CompanyDescription;
            ImageID = User.ProfilePictureID;
            EmailAddress = User.Email;
            FirstPartyEmails = User.RecieveFirstPartyEmails;
            ThirdPartyEmails = User.RecieveThirdPartyEmails;
            UserID = User.Id;
        }

        public string UserID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Description")]
        [DataType(DataType.MultilineText)]
        public string CompanyDescription { get; set; }
        public int? ImageID { get; set; }

        [Display(Name = "Company Description")]
        public string EmailAddress { get; set; }

        [Display(Name = "Receive First Party Emails")]
        public bool FirstPartyEmails { get; set; }

        [Display(Name = "Receive Third Party Emails")]
        public bool ThirdPartyEmails { get; set; }


    }

    public class CompanyPartialVM
    {
        public ApplicationUser Company { get; set; }
        public bool ShowFullDetails { get; set; }
    }

    public class CompanyDetailsVM
    {
        public ApplicationUser Company{ get; set; }
        public List<Pledges.Pledge> CurrentPledges{ get; set; }
        public List<Pledges.Pledge> CompletedPledges { get; set; }
        public bool IsAdmin { get; set; }

    }

    public class CompanySearchResultsVM
    {
        public List<ApplicationUser> CorporateResults { get; set; }

        public bool ShowSelector { get; set; }
    }

}