using System.Linq;
using System.Net;
using System.Web.Mvc;
using Calorie.Models;
using Microsoft.AspNet.Identity;
using Calorie.BusinessLogic;
using System.Net.Mime;

namespace Calorie.Controllers
{
    public class AlertsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }
        
       
        public ActionResult GetAlerts()
        {

            var user = CurrentUser();

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("");
            }

            var response = new System.Text.StringBuilder();

            //var BadgesObj = new Badges();
            var DoSave = false;

            foreach (var m in user.Messages.Where(m => m.Status==Message.StatusEnum.Unread))
            {
                response.Append("<div class='alert " + m.Level.ToString().Replace("_", "-") + " alert-dismissible fade in' role='alert' data-alertid='" + m.ID + "'>" +
                        "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                            m.MessageBody +
                    "</div>");

                if (m.Type==Message.TypeEnum.TemporaryAlert)
                {
                    DoSave = true;
                    m.Status = Message.StatusEnum.Read;
                }
            }
            if (DoSave)
                db.SaveChanges();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(response.ToString());
          

        }

        [Authorize]
        public ActionResult DeleteAlert(string _AlertID)
        {
            var MessageID = GenericLogic.GetInt(_AlertID);

            var ThisUserID = string.Empty;
            if (CurrentUser() != null)
                ThisUserID = CurrentUser().Id;
            

            var Msg = db.Messages.FirstOrDefault(m => m.ID == MessageID && m.UserID == ThisUserID);

            if (Msg != null)
            {
                //we can delete it.
                Msg.Status = Message.StatusEnum.Read;
                db.SaveChanges();

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("OK", MediaTypeNames.Text.Plain);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("Couldn't find message or not permitted", MediaTypeNames.Text.Plain);
           
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
