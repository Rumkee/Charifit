using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Calorie.Models;
using Microsoft.AspNet.Identity;
using Calorie.BusinessLogic;
using System.Threading.Tasks;
using Calorie.Models.Trackers;

namespace Calorie.Controllers
{
    public class TrackersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BusinessLogic.Trackers.TrackerLogic TrackerLogic = new BusinessLogic.Trackers.TrackerLogic() ;

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

      


        #region MicrosoftHealth

        [Authorize]
        public ActionResult MicrosoftHealthAuth()
        {
            Session["ReferingURL"] = Request.UrlReferrer;

            TrackerLogic.setRequestProps(Url, CurrentUser(), this.Request?.Url?.Scheme);
            var msHealth = TrackerLogic.MicrosoftHealth;

            return Redirect(msHealth.AuthenticateStart());

        }

        public ActionResult MicrosoftHealthAuthCompleted(string code)
        {

            var user = CurrentUser();
            if (user== null)
            {
                //todo
            }
            
            var msHealth= TrackerLogic.MicrosoftHealth;

             msHealth.AuthenticateComplete(user.Id, code, db);

            if (!string.IsNullOrEmpty(Session["ReferingURL"].ToString())){
                return Redirect(Session["ReferingURL"].ToString());
            }
            else
            {
                return RedirectToAction("Edit", "Account");
            }
            

        }

        #endregion

        #region Fitbit

        [Authorize]
        public ActionResult FitbitAuth()
        {
            Session["ReferingURL"] = Request.UrlReferrer;
            TrackerLogic.setRequestProps(Url, CurrentUser(), this.Request?.Url?.Scheme);
            var fitbit = TrackerLogic.Fitbit;

            return Redirect(fitbit.AuthenticateStart());

        }

        public async Task<ActionResult> FitbitAuthCompleted(string state, string code)
        {

            TrackerLogic.setRequestProps(Url, CurrentUser(), this.Request?.Url?.Scheme);
            var fitbit = TrackerLogic.Fitbit;

            await fitbit.AuthenticateComplete(state, code, db);

            if (!string.IsNullOrEmpty(Session["ReferingURL"].ToString()))
            {
                return Redirect(Session["ReferingURL"].ToString());
            }
            else
            {
                return RedirectToAction("Edit", "Account");
            }

        }

        [Authorize]
        public async Task<ActionResult> FitBitGetDataForDate(string pledgeID, string inputDate)
        {

            var DateToSearch = GenericLogic.GetDateTime(inputDate);
            var pledge = db.OpenPledges.FirstOrDefault(p => p.PledgeID.ToString() == pledgeID);
            if (pledge == null || !DateToSearch.HasValue)
                return null;
            
            
            TrackerLogic.setRequestProps(Url,CurrentUser(), this.Request?.Url?.Scheme);
            var results = await TrackerLogic.Fitbit.getFitBitDataForDate(DateToSearch.Value,CurrentUser());
            
            return PartialView("~/Views/offsets/Fitbit/_FitbitActivityPartialSummaryList.cshtml", results);

        }

        #endregion

       
        #region Runkeeper
        
        [Authorize]
        public ActionResult RunKeeperAuth()
        {
            Session["ReferingURL"] = Request.UrlReferrer;

            TrackerLogic.setRequestProps(Url, CurrentUser(), this.Request?.Url?.Scheme);
            var runKeeper = TrackerLogic.RunKeeper;

            return Redirect(runKeeper.AuthenticateStart());

        }


        public ActionResult RunKeeperAuthComplete(string userID,string code )
        {

            TrackerLogic.setRequestProps(Url, CurrentUser(), this.Request?.Url?.Scheme);
            var runKeeper = TrackerLogic.RunKeeper;

            runKeeper.AuthenticateComplete(userID, code, db);

            if (!string.IsNullOrEmpty(Session["ReferingURL"].ToString()))
            {
                return Redirect(Session["ReferingURL"].ToString());
            }
            else
            {
                return RedirectToAction("Edit", "Account");
            }

        }

        [Authorize]
        public ActionResult RunKeeperGetPastActivities(string pledgeID)
        {

            var pledge = db.OpenPledges.FirstOrDefault(p => p.PledgeID.ToString() == pledgeID);
            if (pledge == null)
                return null;
            

            var results = BusinessLogic.Trackers.RunKeeper.getPastActivityData(CurrentUser(), db, pledge);

            return PartialView("~/Views/offsets/runKeeper/_runKeeperActivityPartialSummaryList.cshtml", results);            

        }

        [Authorize]
        public ActionResult RunKeeperGetSpecificActivity(string Ident)
        {

            var results = BusinessLogic.Trackers.RunKeeper.getSpecificPastActivityData(Ident,CurrentUser(), db);

            return PartialView("~/Views/offsets/runKeeper/_runKeeperActivityPartial.cshtml", results);

        }
        
        #endregion

     
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

