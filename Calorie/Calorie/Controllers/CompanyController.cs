using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Calorie.Models;
using Microsoft.AspNet.Identity;
using Calorie.BusinessLogic;
using System.Net.Mime;
using Calorie.Models.Companys;
    

namespace Calorie.Controllers
{
    public class CompanyController: Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        

        [Route("co/{companyname}")]
        public ActionResult Details(string companyname)
        {

            var Company = db.Users.FirstOrDefault(u => u.UserName.ToLower().Replace(" ", "") == companyname);

                if (Company != null && Company.IsCompany)
            {

                var VM = new CompanyDetailsVM
                {
                    Company = Company,
                    IsAdmin = CurrentUser() == Company,
                    CurrentPledges = db.OpenPledges.ToList().Where(p => p.Contributors.Exists(c => c.Sinner == Company)).ToList(),
                    CompletedPledges = db.Pledges.ToList().Where(p => PledgesLogic.GetPledgeStatus(p) == PledgesLogic.PledgeStatus.Completed && p.Contributors.Exists(c => c.Sinner == Company)).ToList()
                };

                return View(VM);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("No such company as " + companyname, MediaTypeNames.Text.Plain);
        }

  
        public ActionResult Index() => View(new AllCompanysVM() { Companys = db.Users.Where(u => u.IsCompany).ToList() });

        [HttpPost]
        public ActionResult Filter(string count, string type)
        {

            var countInt = GenericLogic.GetInt(count);
            var typeInt = GenericLogic.GetInt(type);

            var returnRecords = new List<ApplicationUser>();

            var Companies = db.Users.Where(u => u.IsCompany).ToList();
            
            if (typeInt.HasValue && countInt.HasValue)
            {
                switch (typeInt)
                {
                    case 1:
                        returnRecords = Companies.OrderByDescending(UserLogic.GetStarsForUser).Take(countInt.Value).ToList();
                        break;

                    case 2:
                        returnRecords = Companies.OrderByDescending(u => CurrencyLogic.ToBase(u.Contributions)).Take(countInt.Value).ToList();
                        break;

                    //case 3:
                    //default:

                    //    break;

                }
                

            }

            
            return PartialView("_CorporatePartials", new Models.Companys.CompanySearchResultsVM() { CorporateResults = returnRecords ,ShowSelector = true});

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
