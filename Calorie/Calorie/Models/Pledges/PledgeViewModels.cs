using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Calorie.Models.Pledges
{


  

    [NotMapped]
    public class EditPledgeVM
    {
        

        public List<String> GalleryIDs { get; set; }
        public List<String> Activities { get; set; }
        public Pledge Pledge { get; set; }

        public string PledgeID { get; set; }

        [AllowHtml]
        public string Story { get; set; }

        public DateTime ExpiryDate { get; set; }


    }

    [NotMapped]
    public class CreatePledgeVM
    {

        public string JustGivingCharityID { get; set; }
        public List<String> TeamIDs { get; set; }

        public List<String> GalleryIDs { get; set; }

        public List<String> ActivityIDs { get; set; }

        public Pledge Pledge { get; set; }
        public string CurrencySymbol { get; set; }
        public CreatePledgeVM() : base()
        {
            Pledge = new Pledge();


        }
    }


    public class PledgeSearchVM
    {
        public List<Pledge> Pledges{ get; set; }
        public string SearchString { get; set; }

        public bool LoggedIn { get; set; }
    }
        
    public class PledgesIndexVM
    {

        public ApplicationUser User { get; set; }

        public List<Pledge> NewPledges { get; set; }
    }

    public class PledgesDetailsVM
    {
        public ApplicationUser User { get; set; }

        public Pledge Pledge { get; set; }
    }

    public class PledgePartialListVM
    {

        public List<Pledge> Pledges { get; set; }
        public bool Animate { get; set; }
    }

}