using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Web.Mvc;
using Calorie.BusinessLogic;
using Calorie.Models;
using Calorie.Models.Pledges;
using Microsoft.AspNet.Identity;

namespace Calorie.Controllers
{
    public class PledgesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }


        [Authorize]
        public ActionResult AddBookmark(string pledgeID)
        {
            Thread.Sleep(500);


            if (string.IsNullOrEmpty(pledgeID))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("ID wasn't a guid", MediaTypeNames.Text.Plain);
            }

            var user = CurrentUser();

            var current = user.Bookmarks.FirstOrDefault(b => b.PledgeID.ToString() == pledgeID);
            if (current == null)
            {
                var newPledge = new PledgeBookmark { PledgeID = new Guid(pledgeID)};
                user.Bookmarks.Add(newPledge );
                db.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content(newPledge.ID.ToString(), MediaTypeNames.Text.Plain);
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("already setup", MediaTypeNames.Text.Plain);
        }

        [Authorize]
        public ActionResult RemoveBookmark(string pledgeID)
        {
            Thread.Sleep(500);

         

            if (string.IsNullOrEmpty(pledgeID))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("ID wasn't a number", MediaTypeNames.Text.Plain);
            }

            var user = CurrentUser();

            var current = user.Bookmarks.FirstOrDefault(b => b.PledgeID == new Guid(pledgeID));
            if (current == null)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("Can't find that bookmark for that user", MediaTypeNames.Text.Plain);                
            }
            user.Bookmarks.Remove(current);
            db.SaveChanges();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(1.ToString(), MediaTypeNames.Text.Plain);
        }


        public ActionResult DisplayAmountFromBase(string BaseCurrencyInputAmount,  String OutputCode)
        {


            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(CurrencyLogic.GetDisplayAmount(BaseCurrencyInputAmount,"GBP" , OutputCode, CurrentUser()?.Currency), MediaTypeNames.Text.Html);

        }

        public ActionResult DisplayAmount(string InputAmount,String InputCurrencyCode,String OutputCode)
        {

        
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(CurrencyLogic.GetDisplayAmount(InputAmount, InputCurrencyCode, OutputCode, CurrentUser()?.Currency), MediaTypeNames.Text.Html);
                        
        }

        public ActionResult DisplayAmountTotal(List<PledgeContributors> PCList, String OutputCurrencyCode)
        {
            
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(CurrencyLogic.GetDisplayAmount(PCList,OutputCurrencyCode,CurrentUser()?.Currency), MediaTypeNames.Text.Html);

        }

        [Authorize]
        public ActionResult MakePayment(string ContribID)
        {

            var ContribIDInt = GenericLogic.GetInt(ContribID);
            if (ContribIDInt.HasValue)
            {                           
            var contrib = db.PledgeContributors.FirstOrDefault(pc => pc.ID == ContribIDInt);
            var URL = JustGivingLogic.GetPaymentURL(contrib, Url, Request);            
            return Redirect(URL);

            }

            return null;

        }

        public ActionResult PaymentComplete(string ID)
        {
            try
            {         
                
            var IDInt = GenericLogic.GetInt(ID);

            if (!IDInt.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var contrib = db.PledgeContributors.FirstOrDefault(c => c.ID == IDInt);

            if (contrib == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            string url = ConfigurationManager.AppSettings["JustGivingAPIURL"] + ConfigurationManager.AppSettings["JustGivingAppId"] + "/v1/donation/ref/" + contrib.ID;

            //need to check this...contrib.ThirdPartyRef 
            var i = new Uri(url);

            var request = WebRequest.CreateDefault(i);
            request.Method = "GET";
            request.ContentType = "application/json";

            var response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var requestedText = reader.ReadToEnd();


            dynamic data = System.Web.Helpers.Json.Decode(requestedText);


           // var amount = data?.donations[0]?.amount;
            var thirdPartyReference = data?.donations[0]?.thirdPartyReference;
            var status = data?.donations[0]?.status;//"Accepted"

            if (thirdPartyReference != contrib.ID.ToString())
            {
                throw new Exception();
            }
            
            if (status== "Accepted")
            {              
                contrib.Status = PledgeContributors.PledgeContribuionStatus.Completed;
                db.SaveChanges();
            }
            else
            {
                Messaging.Add(Message.LevelEnum.alert_warning, "Looks like the payment wasn't made. Try making payment again.", Message.TypeEnum.TemporaryAlert, contrib.Sinner);
                    db.SaveChanges();
                    return RedirectToAction("Index");

            }

            Messaging.Add(Message.LevelEnum.alert_success, "Thank You. Your payment has now been processed.", Message.TypeEnum.StickyAlert, contrib.Sinner);
                db.SaveChanges();
                return RedirectToAction("Index");

          }

        catch{
                if (CurrentUser() != null)
                {
                    Messaging.Add(Message.LevelEnum.alert_warning, "OOps! that didn't work. try making payment again.", Message.TypeEnum.TemporaryAlert, CurrentUser());
                    db.SaveChanges();
                }
                    return RedirectToAction("Index");
            }

            
        }

        [HttpPost]
        public ActionResult Filter(string count, string type)
        {

            Thread.Sleep(1000);
            var countInt = GenericLogic.GetInt(count);
            var typeInt = GenericLogic.GetInt(type);

            var returnRecords = new List<Pledge>();
            var source = db.OpenPledges.Include("Activity_Types");

            if (typeInt.HasValue && countInt.HasValue)
            {
                switch (typeInt)
                {
                    case 1://"New"
                        returnRecords = source.OrderByDescending(p => p.CreatedUTC).Take(countInt.Value).ToList();
                        break;

                    case 2://"Trending"
                        returnRecords = source.OrderByDescending(p => p.CreatedUTC).Take(countInt.Value).ToList();
                        break;

                    case 3://"Almost Completed"
                        returnRecords = source.ToList().OrderByDescending(p => p.TotalOffsetPercent).Take(countInt.Value).ToList();
                        break;

                    case 4://"Almost Expired"
                        returnRecords = source.OrderBy(p => p.ExpiryDate).Take(countInt.Value).ToList();
                        break;                        
                }
                
            }
            
            return PartialView("_PledgeSearchResultsPartial", new PledgeSearchVM { Pledges = returnRecords,SearchString = string.Empty,LoggedIn = (CurrentUser()!=null)});

        }

       
        [Authorize]     
        public ActionResult AddPledgeAlert(string SearchString)
        {

            var User = CurrentUser();

            if (User == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Content("");
            }

            User.Alerts.Add(new UserAlert { Data = SearchString, Type = UserAlert.UserAlertType.PledgeQuery });
            db.SaveChanges();
            
            Response.StatusCode = (int)HttpStatusCode.OK;

            var returnHTML = "<div class='alert alert-success alert-dismissible fade in' role='alert'>" +
                     "<button type = 'button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                         "Thanks! Alert has been added." +
                 "</div>";


            return Content(returnHTML);

        }
        
        [HttpPost]
        public ActionResult userSearch(string SearchString)
        {

            Thread.Sleep(500);

            var _SearchString = SearchString.ToLower();
            
            var Pledges = db.OpenPledges.Include("Activity_Types").ToList();


            var teamNames = db.Teams.Where(t => t.Name.Contains(_SearchString)).ToList().ConvertAll(t => t.Name.ToLower());
            var teamPledgesResult = Pledges.Where(p => p.Teams.Any(t => teamNames.Contains(t.Name.ToLower())));

            var actitiesList = Enum.GetNames(typeof(PledgeActivity.ActivityTypes)).Where(a => a.ToLower().Contains(_SearchString));
            var activitesPledgesResult = Pledges.Where(p => p.Activity_Types.Any(at => actitiesList.Contains(at.Activity.ToString())));

            var charitiesPledgesResult = Pledges.Where(p => p.Charity.Name.ToLower().Contains(_SearchString) );


            var result = teamPledgesResult.Concat(activitesPledgesResult).Concat(charitiesPledgesResult);

            bool IsLoggedIn = CurrentUser() != null;

            return PartialView("_PledgeSearchResultsPartial", new PledgeSearchVM { Pledges = result.ToList(), SearchString = _SearchString, LoggedIn=IsLoggedIn });

        }


        [AllowAnonymous]
        public ActionResult userSearchPrompt(string SearchString)
        {

            SearchString = SearchString.ToLower();
            var ImageURL = Url.Action("GetImage", "Image");
            var ActivitiesURL = Url.Content("~/Images/Activities/");
                        
            var result = new
            {
                teams = from t in db.Teams where t.Name.ToLower().Contains(SearchString) select new {t.Name, Url = ImageURL + "?ImageID=" + t.ImageID + "&Thumb=true" },
                activities = from act in Enum.GetNames(typeof(PledgeActivity.ActivityTypes)) where act.ToLower().Contains(SearchString) select new { Name = act.Replace("_", " "), Url = ActivitiesURL + act + ".png" },
                charities = from c in JustGivingLogic.SearchCharities(SearchString) select new {c.Name, Url = c.JustGivingCharityImageURL }
            };


            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        // GET: Pledges
        public ActionResult Index()
        {

            var context = ((IObjectContextAdapter)db).ObjectContext;
            context.Refresh(RefreshMode.StoreWins, db.PledgeContributors);

            var vm = new PledgesIndexVM
            {
                User = CurrentUser(),
                NewPledges = db.OpenPledges.Include("Activity_Types").OrderByDescending(p => p.CreatedUTC).Take(10).ToList()
        
            };
            
            return View(vm);
        }

        // GET: Pledges/Details/5
        
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //don't use open pledges, allow showing historical pledges.
            Pledge pledge = db.Pledges.Include("Activity_Types").FirstOrDefault(p => p.PledgeID.ToString() == id);            
            if (pledge == null)
            {
                return HttpNotFound();
            }
            return View( new PledgesDetailsVM { Pledge = pledge, User = CurrentUser() });
        }

        // GET: Pledges/Create
        [Authorize] 
        public ActionResult Create()
        {
           
            ViewBag.Teams = db.Teams.ToList();
           
            var PledgeVM = new CreatePledgeVM();
            PledgeVM.CurrencySymbol = CurrencyLogic.GetCurrencyPrefix(CurrentUser().Currency);
            PledgeVM.Pledge = new Pledge();
            PledgeVM.Pledge.Contributors.Add(new PledgeContributors { Sinner = CurrentUser(), SinnerID = CurrentUser().Id });

            return View(PledgeVM);
        }

        // POST: Pledges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePledgeVM pledgeVM)
        {
            ModelState.Clear();
            TryValidateModel(pledgeVM.Pledge);

            
            if (ModelState.IsValid)
            {

                var ThisUser = CurrentUser();
                if (!PledgesLogic.getCompletePledgeFromCreatePledgeVM(pledgeVM, db, ThisUser))
                {
                    Messaging.Add(Message.LevelEnum.alert_danger, "Sorry something went wrong trying to create your pledge.", Message.TypeEnum.TemporaryAlert, ThisUser);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                db.Pledges.Add(pledgeVM.Pledge );
                Messaging.Add(Message.LevelEnum.alert_success, "Thank you! your new pledge has been created.", Message.TypeEnum.StickyAlert, ThisUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        // GET: Pledges/Edit/5
        [Authorize] 
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pledge pledge = db.OpenPledges.Include("Activity_Types").FirstOrDefault(p => p.PledgeID.ToString()==id);
            if (pledge == null)
            {
                Messaging.Add(Message.LevelEnum.alert_danger, "OOps! something went wrong trying to find that pledge.", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (pledge.Originator.SinnerID != CurrentUser().Id)
            {
                Messaging.Add(Message.LevelEnum.alert_danger, "You are not the creator of that pledge", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                     
            return View( new EditPledgeVM { Pledge=pledge,PledgeID=pledge.PledgeID.ToString() });

        }

        [Authorize]
        public ActionResult Cancel(string id)
        {
            if (string.IsNullOrEmpty(id) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

     
            Pledge pledge = db.OpenPledges.Include("Activity_Types").FirstOrDefault(p => p.PledgeID.ToString() == id);
            if (pledge == null)
            {
                Messaging.Add(Message.LevelEnum.alert_warning, "OOps! something went wrong trying to find that pledge.", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var user = CurrentUser();
            if (pledge.Originator.SinnerID == user.Id & pledge.Contributors.Count == 1 && pledge.Offsets.Count == 0)
            {
                try
                {
                    pledge.Closed = true;
                    Messaging.Add(Message.LevelEnum.alert_success, "Pledge canceled.", Message.TypeEnum.StickyAlert, user);                
                }
                catch
                {
                    Messaging.Add(Message.LevelEnum.alert_warning, "Sorry, something went wrong trying to cancel that pledge", Message.TypeEnum.TemporaryAlert, CurrentUser());                
                }
                
            }
            else
            {
                Messaging.Add(Message.LevelEnum.alert_warning, "Sorry, you can't cancel that pledge", Message.TypeEnum.TemporaryAlert, CurrentUser());                
            }

            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // POST: Pledges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPledgeVM EditVM)
        {
            if (ModelState.IsValid)
            {
                var pledge = db.OpenPledges.Include("Activity_Types").FirstOrDefault(p => p.PledgeID.ToString() == EditVM.PledgeID);
                PledgesLogic.EditPledge(EditVM, pledge,CurrentUser(),db);
                db.SaveChanges();
                return RedirectToAction("Details", new {id=pledge.PledgeID});
            }
            
         
            return View(EditVM);
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
