using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models
{
    public class UserAlert
    {

        public enum UserAlertType
        {
            Something,
            PledgeQuery
        };
        
        [Key]
        public int ID { get; set; }

        //[InverseProperty("Saints")]
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public string UserID { get; set; }

        public UserAlertType Type { get; set; }

        public string Data { get; set; }


    }
}