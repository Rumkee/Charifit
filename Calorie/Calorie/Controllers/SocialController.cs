using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using Calorie.Models;
using Microsoft.AspNet.Identity;

using Calorie.Models.Social;

namespace Calorie.Controllers
{
    public class SocialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }


#region Likes

        public ActionResult LikesCount(string LinkType, string LinkID)
        {                      
            var Count = db.Likes.Count(L => L.LinkID == LinkID && L.LinkType == LinkType);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(Count.ToString(), MediaTypeNames.Text.Plain);
            
        }

  
        [HttpPost]
        public ActionResult LikesAdd(string LinkType, string LinkID)
          {

            System.Threading.Thread.Sleep(1000);

           
            var ThisUserID = "";
            var user = CurrentUser();
            if (user != null)
            {
                ThisUserID = user.Id;
            }

            db.Likes.Add(new Like() { LinkID = LinkID, LinkType = LinkType, UserID = ThisUserID });
            db.SaveChanges();
            
              var CurrentCount = db.Likes.Count(L => L.LinkID == LinkID && L.LinkType == LinkType);

            Response.StatusCode = (int)HttpStatusCode.OK;

            return Content(CurrentCount.ToString(), MediaTypeNames.Text.Plain);             

        }

 #endregion


    }
}
