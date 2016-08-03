using System;
using System.Collections.Generic;
using Calorie.BusinessLogic;
using Calorie.BusinessLogic.Trackers;

namespace Calorie.Models.Trackers
{

    

    public class FitbitSelectorVM
    {
        public List<FitbitVM> Available { get; set; }
        public List<FitbitVM> UnAvailable { get; set; }
        public List<FitbitVM> Used { get; set; }

        public FitbitSelectorVM()
        {
            Available = new List<FitbitVM>();
            UnAvailable = new List<FitbitVM>();
            Used = new List<FitbitVM>();
        }
    }


    public class FitbitVM
    {

        public FitbitVM()
        {
            ShowButtons = true;
        }

        public FitbitVM(string JSONBlob, DateTime _ActivityDate, ApplicationUser usr = null)
        {
            /*
            "activityId":51007,
       "activityParentId":90019,
       "calories":230,
       "description":"7mph",
       "distance":2.04,
       "duration":1097053,
       "hasStartTime":true,
       "isFavorite":true,
       "logId":1154701,
       "name":"Treadmill, 0% Incline",
       "startTime":"00:25",
       "steps":3783
           */
            

             jsonblob = JSONBlob;
            JSONObj = System.Web.Helpers.Json.Decode(JSONBlob);

            var dtstr = _ActivityDate.ToString("dd MMMM yyyy");
            if (!string.IsNullOrEmpty(JSONObj.startTime))
                dtstr += " " + JSONObj.startTime;

            var tryDate = GenericLogic.GetDateTime(dtstr);
            JSONObj.ActivityDate = tryDate ?? _ActivityDate;




            //JSONObj.duration_mins = 
            //JSONObj.total_kilometers = (((decimal)JSONObj.total_distance) / 1000.0m).ToString("0.00");
            //JSONObj.duration_hours = (((decimal)JSONObj.duration_mins) / 60.0m).ToString("0.00");

            JSONObj.logoPath = FitBit.LogoURL;

            //var rnd = new Random();
            //JSONObj.mapId = "map" + rnd.Next(1, 9999).ToString();
            //JSONObj.galleryID = "gallery" + rnd.Next(1, 9999).ToString();
            User = usr;
            ShowButtons = true;
        }

     

        public dynamic JSONObj { get; set; }

        public string jsonblob { get; set; }

        public ApplicationUser User { get; set; }

        public bool ShowButtons { get; set; }

 

    }


}