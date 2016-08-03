using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models.Pledges
{

    public class PledgeActivity
    {
        public int ID { get; set; }

        [ForeignKey("Pledge_PledgeID")]
        public virtual Pledge Pledge { get; set; }

        public Guid Pledge_PledgeID { get; set; }

        public ActivityTypes Activity { get; set; }

        public enum ActivityUnits
        {
            Sessions,
            Hours,
            Minutes,
            Meters,
            Kilometers,
            Miles,
            Calories
        }

        public enum ActivityTypes
        {
            Running,
            Cycling,
            Mountain_Biking,
            Walking,
            Hiking,
            Downhill_Skiing,
            Cross_Country_Skiing,
            Snowboarding,
            Skating,
            Swimming,
            Wheelchair,
            Rowing,
            Elliptical,
            Yoga,
            Pilates,
            //CrossFit,??
            Spinning,
            //Zumba,?
            //Barre,?
            //Group_Workout,?
            Dance,
            //Bootcamp,?
            Boxing_MMA,
            Meditation,
            Strength_Training,
            Circuit_Training,
            Core_strengthening,
            //Arc_Trainer,?
            Stairmaster_Stepwell,
            //Sports,?
            //Nordic_Walking?
        }
    }

    public class PreferredActivity
    {

        public int ID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        public string UserID { get; set; }
        
        public PledgeActivity.ActivityTypes Activity { get; set; }
        
    }

}