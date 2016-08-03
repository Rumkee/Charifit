using System;
using System.Linq;
using System.Web.Mvc;
using Calorie.Models;
using Calorie.Models.Pledges;
using Microsoft.AspNet.Identity;



namespace Calorie.Controllers
{
    public class PledgeContributorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

       

        // GET: PledgeContributors/Create
        [Authorize]
        public ActionResult Create(string Pledgeid)
        {

            var _Pledge = db.OpenPledges.FirstOrDefault(p => p.PledgeID.ToString() == Pledgeid);
            if (_Pledge == null)
            {
                return HttpNotFound();
            }

            return View(new PledgeContributors() {
                PledgeID = new Guid(Pledgeid),
                Pledge = _Pledge,
                Sinner = CurrentUser(),
                SinnerID = CurrentUser().Id,
                Currency = CurrentUser().Currency,
                IsOriginator = false
            });

        }

        // POST: PledgeContributors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PledgeContributors pledgeContributors)
        {

            pledgeContributors.Sinner = CurrentUser();
            pledgeContributors.SinnerID = CurrentUser().Id;
            ModelState.Clear();
            TryValidateModel(pledgeContributors);

            var UserMessage = "";
            var ErrorMessage = "";
            if (pledgeContributors.IsOriginator)
            {
                UserMessage = "Excellent! Thank you. Your new pledge has been created.";
                ErrorMessage = "There was a problem trying to create your new pledge. Sorry.";
            }
            else
            {
                UserMessage = "Excellent! Thank you. you have been added as co-sponsor to this pledge.";
                ErrorMessage = "There was a problem trying to add your co-sponsorship to this pledge. Sorry.";
            }

            if (ModelState.IsValid)
            {
                db.PledgeContributors.Add(pledgeContributors);
                BusinessLogic.Messaging.Add(Message.LevelEnum.alert_success, UserMessage, Message.TypeEnum.StickyAlert, CurrentUser());                
            }
            else
            {                
                BusinessLogic.Messaging.Add(Message.LevelEnum.alert_danger , ErrorMessage, Message.TypeEnum.TemporaryAlert , CurrentUser());
            }

            db.SaveChanges();
            return RedirectToAction("Details", "Pledges", new { id = pledgeContributors.PledgeID });
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
