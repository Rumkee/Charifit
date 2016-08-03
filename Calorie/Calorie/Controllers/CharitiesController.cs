using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Web.Mvc;
using Calorie.BusinessLogic;
using Calorie.Models;
using Calorie.Models.Charities;

namespace Calorie.Controllers
{
    public class CharitiesController : Controller
    {

           private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Charities
        public ActionResult Index()
        {
            return
                View(new CharityIndexVM
                {
                    Charities =db.Charities.ToList().OrderByDescending(c => c.Pledges.Sum(p => CurrencyLogic.ToBase(p.Contributors))).Take(10).ToList()
                });
        
        }


        [Route("Charity/{charityname}")]
        public ActionResult Details(string charityname)
        {

            var charity = db.Charities.FirstOrDefault(c => c.Name.ToLower().Replace(" ", "") == charityname.ToLower());
            if (charity != null)
            {
                var DetailsVM = new CharityDetailsVM
                {
                    CurrentPledges = db.OpenPledges.Where(p => p.CharityID == charity.ID).ToList(),
                    Charity = charity
                };

                var AllPledges = db.PledgeContributors.Where(c => c.Pledge.CharityID == charity.ID).ToList();

                DetailsVM.TotalPledged = CurrencyLogic.ToBase(AllPledges);
                DetailsVM.NoOfPledges = AllPledges.Count();

                var AllRaised = db.PledgeContributors.Where(c => c.Pledge.CharityID == charity.ID).ToList().Where(pc => PledgesLogic.GetPledgeStatus(pc.Pledge) == PledgesLogic.PledgeStatus.Completed).ToList();
                DetailsVM.TotalRaised = CurrencyLogic.ToBase(AllRaised);
                DetailsVM.NoOfRaised = AllRaised.Count();
                DetailsVM.UserPledgedChartData = CharityLogic.getUserPledgeContributionsForCharity(charity);
                DetailsVM.TeamPledgedChartData = CharityLogic.getTeamPledgeContributionsForCharity(charity);

                return View(DetailsVM);
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Content("Charity not found", MediaTypeNames.Text.Plain);
        }

        
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {            
            var searchResults= JustGivingLogic.SearchCharities(searchTerm);            
            return PartialView("_CharityPartials", new CharityListVM { Charities= searchResults ,ShowSelector=true,ShowJustGivingLink = false,ShowSocial = false});
        }


        [HttpPost]
        public ActionResult Filter(string count, string type)
        {

            Thread.Sleep(1000);
            var countInt = GenericLogic.GetInt(count);
            var typeInt = GenericLogic.GetInt(type);

            var returnRecords = new List<Charity>();

            //"Most Raised", "Most Activities", "Most Liked
            if (typeInt.HasValue && countInt.HasValue)
            {
                switch (typeInt)
                {
                    case 1:
                        returnRecords = db.Charities.ToList().OrderByDescending(c => c.Pledges.Sum(p => CurrencyLogic.ToBase(p.Contributors))).Take(countInt.Value).ToList();
                        break;

                    case 2:
                        returnRecords =db.Charities.OrderByDescending(c => c.Pledges.Sum(p => p.Offsets.Count())).Take(countInt.Value).ToList();
                        break;

                    case 3:
                    default:
                        returnRecords =db.Charities.OrderByDescending(c => db.Likes.Count(l => l.LinkType == "Charity" && l.LinkID == c.ID.ToString())).Take(countInt.Value).ToList();
                        break;

                }

                
            }


            return PartialView("_CharityPartials", new CharityListVM { Charities = returnRecords,ShowSelector =false, ShowSocial = true,ShowJustGivingLink =false});

        }


    }
}