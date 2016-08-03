using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models
{
    [NotMapped]
    public class Badge
    {

        public int ID{ get; set; }
        public string ImgName{ get; set; }
        public string Description { get; set; }

    }

    [NotMapped]
    public class Badges: List<Badge>
    {

        public Badges()
        {
            var B1 = new Badge() { ID = 1, Description = "Badge 1 Description", ImgName = "1.png" };
            var B2 = new Badge() { ID = 2, Description = "Badge 2 Description", ImgName = "2.png" };
            var B3 = new Badge() { ID = 3, Description = "Badge 3 Description", ImgName = "3.png" };
            var B4 = new Badge() { ID = 4, Description = "Badge 4 Description", ImgName = "4.png" };
            var B5 = new Badge() { ID = 5, Description = "Badge 5 Description", ImgName = "5.png" };
            var B6 = new Badge() { ID = 6, Description = "Badge 6 Description", ImgName = "6.png" };
            var B7 = new Badge() { ID = 7, Description = "Badge 7 Description", ImgName = "7.png" };
            var B8 = new Badge() { ID = 8, Description = "Badge 8 Description", ImgName = "8.png" };
            var B9 = new Badge() { ID = 9, Description = "Badge 9 Description", ImgName = "9.png" };
            var B10 = new Badge() { ID = 10, Description = "Badge 10 Description", ImgName = "10.png" };

            this.Add(B1);
            this.Add(B2);
            this.Add(B3);
            this.Add(B4);
            this.Add(B5);
            this.Add(B6);
            this.Add(B7);
            this.Add(B8);
            this.Add(B9);
            this.Add(B10);


        }

    }


    
    public class BadgeAward
    {
        public int ID { get; set; }

        public int BadgeID { get; set; }


        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public string UserID { get; set; }

        public DateTime Awarded { get; set; }



    }
}