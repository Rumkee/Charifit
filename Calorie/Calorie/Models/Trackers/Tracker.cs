using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models.Trackers
{
    public class Tracker
    {

        public enum TrackerType
        {
            RunKeeper,
            Fitbit,
            MicrosoftHealth
        }

        public int ID { get; set; }

        public TrackerType Type { get; set; }

        public string AuthToken { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? AccessTokenExpiry { get; set; }
        
        public string ThirdPartyUserID { get; set; }


        [InverseProperty("Trackers")]
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

     //   [Column(TypeName = "NVARCHAR")]
     //   [StringLength(4000)]
        public string UserID { get; set; }


    }


    public class AddTrackerVM
    {

       public BusinessLogic.Trackers.ITracker ITracker { get; set; }
        public Tracker UserTracker { get; set; }


    }
}