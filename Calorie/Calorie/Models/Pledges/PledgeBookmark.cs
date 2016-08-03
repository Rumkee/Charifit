using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models.Pledges
{

    public class PledgeBookmark
    {
        public int ID { get; set; }

        [ForeignKey("PledgeID")]
        public virtual Pledge Pledge { get; set; }

        public Guid PledgeID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        public string UserID { get; set; }


    }

  
}