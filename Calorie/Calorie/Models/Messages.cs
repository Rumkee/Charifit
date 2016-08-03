using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models
{
    public class Message
    {

        public enum LevelEnum
        {
            alert_success,
            alert_info,
            alert_warning,
            alert_danger
        }

        [Flags]
        public enum TypeEnum
        {
            TemporaryAlert,
            StickyAlert,
            Push,
            Email
        }

        public enum StatusEnum
        {
            Unread,
            Read
        }

        public StatusEnum Status { get; set; }

        public int ID { get; set; }

        public LevelEnum  Level{ get; set; }

        public TypeEnum Type{ get; set; }
        public string MessageBody { get; set; }

        [InverseProperty("Messages")]
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    //    [Column(TypeName = "NVARCHAR")]
     //   [StringLength(4000)]
        public string UserID { get; set; }


        //public static bool Add(Alert.AlertType _Type, String _Message,List<Alert> CurrentAlerts)
        //{
        //    if (CurrentAlerts.Where(a => a.Message == _Message && a.Type == _Type).Count() == 0)
        //    {
        //        CurrentAlerts.Add(new Alert() { Message = _Message, Type = _Type });
        //        return true;
        //    }
        //    return false;
        //}
    }
}