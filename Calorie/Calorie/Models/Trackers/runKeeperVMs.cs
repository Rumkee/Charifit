using System;
using System.Collections.Generic;
using Calorie.BusinessLogic.Trackers;

namespace Calorie.Models.Trackers
{


    public class runKeeperSelectorVM
    {
        public List<runKeeperVM> Available { get; set; }
        public List<runKeeperVM> UnAvailable { get; set; }
        public List<runKeeperVM> Used { get; set; }

        public runKeeperSelectorVM()
        {
            Available = new List<runKeeperVM>();
            UnAvailable= new List<runKeeperVM>();
            Used= new List<runKeeperVM>();
        }
    }


    public class runKeeperVM
    {

        public runKeeperVM()
        {
            ShowButtons = true;
        }

        public runKeeperVM(string JSONBlob,ApplicationUser usr=null)
        {
            jsonblob = JSONBlob;
            JSONObj = System.Web.Helpers.Json.Decode(JSONBlob);

            JSONObj.duration_mins = JSONObj.duration / 60.0m;
            JSONObj.total_kilometers = (((decimal) JSONObj.total_distance)/1000.0m).ToString("0.00");
            JSONObj.duration_hours = (((decimal) JSONObj.duration_mins)/60.0m).ToString("0.00");
            
            JSONObj.logoPath = RunKeeper.LogoURL;

            var rnd = new Random();
            JSONObj.mapId = "map" + rnd.Next(1,9999).ToString();
            JSONObj.galleryID = "gallery" + rnd.Next(1, 9999).ToString();
            User = usr;
            ShowButtons = true;
        }

        public dynamic JSONObj { get; set; }

        public string jsonblob { get; set; }

        public ApplicationUser User { get; set; }

        public bool ShowButtons { get; set; }

        //public string activityImgSrc { get; set; }
        //public string type { get; set; }
        //public string start_time { get; set; }


        //public string uri { get; set; }
        //public bool showLogo { get; set; }
        //public string logoURL { get; set; }
        //public string total_distanceFormatted { get; set; }
        //public string duration_minsFormatted { get; set; }
        //public string total_calories { get; set; }
        //public string mapId { get; set; }
        //public string galleryID { get; set; }
        //public List<runKeeperImage> Images  { get; set; }
        //public string notes { get; set; }

    }

    


}