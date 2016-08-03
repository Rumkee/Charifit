using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models.Teams
{
    public class TeamJoinRequests
    {

        public int ID { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("TeamJoinRequests")]
        public virtual ApplicationUser User { get; set; }

       // [Column(TypeName = "NVARCHAR")]
        //[StringLength(4000)]
        public string UserID { get; set; }

        [ForeignKey("TeamID")]
        public virtual Teams.Team Team { get; set; }
        public int TeamID { get; set; }


    }
}