using Calorie.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Calorie.BusinessLogic.Trackers
{
    public class TrackerLogic
    {

        public UrlHelper Url{ get; set; }
        public ApplicationUser User { get; set; }
        public string RequestScheme { get; set; }

        public List<ITracker> Trackers { get; set; }

        public RunKeeper RunKeeper { get; set; }
        public FitBit Fitbit { get; set; }
        public MicrosoftHealth MicrosoftHealth { get; set; }


        
        public TrackerLogic()
        {

            RunKeeper = new RunKeeper(this);
            Fitbit = new FitBit(this);
            MicrosoftHealth = new MicrosoftHealth(this);

            Trackers = new List<ITracker> {RunKeeper, Fitbit, MicrosoftHealth};


        }

        public void setRequestProps(UrlHelper _Url,ApplicationUser _User,string _RequestScheme)
        {
            Url = _Url;
            User = _User;
            RequestScheme = _RequestScheme;
        }

    }


}