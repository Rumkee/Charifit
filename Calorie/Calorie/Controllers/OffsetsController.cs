using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Calorie.Models;
using Microsoft.AspNet.Identity;

namespace Calorie.Controllers
{
    public class OffsetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

      
        // GET: Offsets/Create
          [Authorize] 
        public ActionResult Create(string pledgeID)
        {
 
            return View(new Offset() {OffsetterID =CurrentUser().Id,
                                                    Offsetter =CurrentUser(),
                                                    PledgeID = new Guid(pledgeID),
                                                    Pledge=db.OpenPledges.Include("Activity_Types").FirstOrDefault(p => p.PledgeID.ToString() == pledgeID)});
        }

        // POST: Offsets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
          [Authorize] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Offset offset)
        {
            if (ModelState.IsValid)
            {
                db.Offsets.Add(offset);
                BusinessLogic.Messaging.Add(Message.LevelEnum.alert_success, "Thank you. your activity has been logged", Message.TypeEnum.StickyAlert, CurrentUser());
                
            }
            else
            {                
                BusinessLogic.Messaging.Add(Message.LevelEnum.alert_danger , "There was a problem trying to save your activity. Sorry.", Message.TypeEnum.TemporaryAlert , CurrentUser());
            }

            db.SaveChanges();
            return RedirectToAction("Details", "Pledges", new { id = offset.PledgeID });

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
