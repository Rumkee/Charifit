using System.Linq;
using System.Web.Mvc;
using Calorie.BusinessLogic;
using Calorie.Models;
using Calorie.Models.Misc;
using Microsoft.AspNet.Identity;

// For extension methods.


namespace Calorie.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        
        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }


        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var model = new SideBarVM
            {
                Teams = db.Teams.ToList(),
                Users = db.Users.ToList(),
                Pledges = db.OpenPledges.ToList()
            };

            return PartialView("_Sidebar",model);
        }
        
        public ActionResult Index()
        {

            var IndexVM = new IndexViewModel { LoggedInUser = CurrentUser() };
            return View(IndexVM);
        }


        public ActionResult FAQ() => View(GenericLogic.GetFAQs(Url));

        public ActionResult BrowserNotSupported() => View();

        public ActionResult TandC() => View();
       
        public ActionResult Top() => View();

        public ActionResult Cookies() => View();
      
        public ActionResult About() => View();

        public ActionResult Contact() => View();

        public ActionResult Blog() => View();
    }
}