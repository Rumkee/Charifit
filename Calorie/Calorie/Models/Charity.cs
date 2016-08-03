using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models.Charities
{


    public class Charity {

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string JustGivingCharityID { get; set; }        
        public string JustGivingCharityBlob { get; set; }
        public string JustGivingCharityImageURL { get; set; }
        public string Description { get; set; }
        public string CharityURL { get; set; }
        public virtual List<Pledges.Pledge> Pledges{ get; set; }

        public string JustGivingRegistrationNumber { get; set; }

    }

    public class PreferredCharity
    {
        public int ID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public string UserID { get; set; }

        [ForeignKey("CharityID")]
        public virtual Charity Charity { get; set; }
        public int CharityID { get; set; }

    }

    
    public class CharityVM
    {
        public Charity Charity{ get; set; }
        public bool ShowSelector { get; set; }
        public bool ShowRemoveSelector { get; set; }

        public bool ShowSocial { get; set; }
        public bool ShowJustGivingLink { get; set; } = false;
    }

    public class CharityIndexVM
    {
        public List<Charity> Charities { get; set; }

    }

    public class CharityDetailsVM
    {

        public Charity Charity{ get; set; }
        public List<Pledges.Pledge> CurrentPledges{ get; set; }
        public decimal TotalPledged { get; set; }
        public int NoOfPledges { get; set; }    
        public decimal TotalRaised { get; set; }
        public int NoOfRaised { get; set; }
        public BusinessLogic.ChartLogic.chartData UserPledgedChartData { get; set; }
        public BusinessLogic.ChartLogic.chartData TeamPledgedChartData { get; set; }
        

    }

    public class CharityListVM
    {
        public List<Charity> Charities{ get; set; }
        public bool ShowSelector { get; set; }

        public bool ShowSocial { get; set; }
        public bool  ShowJustGivingLink { get; set; }
    }
}