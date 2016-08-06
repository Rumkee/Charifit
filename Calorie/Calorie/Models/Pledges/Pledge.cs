using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Calorie.Models.Pledges
{

    //public class PledgeTeam
    //{
    //    public int ID { get; set; }

    //    [ForeignKey("TeamID")]
    //    public virtual Team Team { get; set; }
    //    public int TeamID { get; set; }

    //    [ForeignKey("PledgeID")]
    //    public virtual Pledge Pledge{ get; set; }
    //    public int PledgeID { get; set; }


    //}
    public class Pledge
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PledgeID { get; set; }


        public bool Closed { get; set; }


        [Display(Name = "Amount of activity required")]
        public decimal Activity_Amount { get; set; }

        public PledgeActivity.ActivityUnits Activity_Units { get; set; }

        [Display(Name = "Select the applicable types of activity, or leave blank for all.")]
        public List<PledgeActivity> Activity_Types { get; set; } = new List<PledgeActivity>();


        [Display(Name = "Tell us the story behind this donation")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Story { get; set; }

        [Display(Name = "Allow others to add to this pledge")]
        public bool OpenToOtherContributors { get; set; }
        

      
        [Display(Name = "Charity")]
        public string JustGivingCharityID { get; set; }

        public string CharityImageURL { get; set; }

        public string CharityName { get; set; }
        
        public virtual List<Offset> Offsets { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        
        public decimal  TotalOffsetAmount { 
        get
        {
            if (Offsets != null)
                    return Offsets.Sum(o => o.OffsetAmount);

            return 0M;

        }
        }           

        public int TotalOffsetPercent 
        { 
            get
            {
                if (Activity_Amount!=0)
                    return (int)(Math.Min((decimal)100.00, (TotalOffsetAmount /Activity_Amount) * (decimal)100.00));
                
                return 0;
            }
        }

        public virtual ICollection<Teams.Team> Teams { get; set; }

        public virtual List<PledgeContributors> Contributors { get; set; } = new List<PledgeContributors>();

        public PledgeContributors Originator{
            get {
                return Contributors.FirstOrDefault(c => c.IsOriginator = true);
            }
        }

        public virtual List<CalorieImage> Gallery { get; set; } = new List<CalorieImage>();

        public virtual List<PledgeBookmark> Bookmarks { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUTC { get; set; }


        [ForeignKey("CharityID")]
        public virtual Charities.Charity Charity{ get; set; }

        public Nullable<int> CharityID { get; set; }

    }

   

  


}
