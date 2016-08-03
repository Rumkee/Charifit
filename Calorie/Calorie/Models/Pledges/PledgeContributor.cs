using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Calorie.Models.Pledges
{

    public class PledgeContributors
    {

        public enum PledgeContribuionStatus
        {
            Pending,
            Completed
        }

        public PledgeContribuionStatus Status { get; set; }

        public int ID { get; set; }

        [Range(1, 9999, ErrorMessage = "The amount must be between 1 and 9999")]
        public decimal Amount { get; set; }
        public BusinessLogic.CurrencyLogic.CurrencyEnum Currency { get; set; }

        public bool IsOriginator { get; set; }

        [ForeignKey("SinnerID")]
        public virtual ApplicationUser Sinner { get; set; }

        [Required]
        [Display(Name = "Sinner")]
        public string SinnerID { get; set; }


        [ForeignKey("PledgeID")]
        public virtual Pledge Pledge { get; set; }

        [Required]
        [Display(Name = "Pledge")]
        public Guid PledgeID { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }

        [Display(Name = "Keep Pledged Amount Anonymous")]
        public bool AmountAnonymous { get; set; }

        [Display(Name = "Keep Your Identity Anonymous")]
        public bool UserAnonymous { get; set; }

        public string ThirdPartyRef { get; set; }

    }

    public class PledgeContributorVM
    {
        public PledgeContributors PC { get; set; }
        public bool IsThisUser { get; set; }
    }
}