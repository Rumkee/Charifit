using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Calorie.BusinessLogic;
using Calorie.BusinessLogic.TeamLogic;
using Calorie.Models;
using Calorie.Models.Teams;
using Microsoft.AspNet.Identity;

namespace Calorie.Controllers
{
    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }



        [HttpPost]
        public ActionResult Search(string searchTerm)
        {

            var records = db.Teams.Where(t => t.Name.Contains(searchTerm));                          
            return PartialView("_TeamPartials", new TeamSearchResultsVM { TeamResults = records.ToList() });

        }


        [HttpPost]
        public ActionResult Filter(string count, string type)
        {

            Thread.Sleep(1000);
            var countInt = GenericLogic.GetInt(count);
            var typeInt = GenericLogic.GetInt(type);

            var returnRecords = new List<Team>();
            //1=Most Stars
            //    2=Most Pledges            
            //    3= Most Popular
            if (typeInt.HasValue && countInt.HasValue)
            {
                switch (typeInt)
                {
                    case 1:
                        returnRecords = db.Teams.ToList().OrderByDescending( TeamLogic.getStarsForTeam).Take(countInt.Value).ToList();
                        break;

                    case 2:
                        returnRecords = db.Teams.OrderByDescending(t => t.Pledges.Count()).Take(countInt.Value).ToList();
                        break;

                    case 3:
                    default:
                        returnRecords = db.Teams.OrderByDescending(t => t.Members.Count()).Take(countInt.Value).ToList();

                        break;
                }
                
            }
            
            return PartialView("_TeamPartials", new TeamSearchResultsVM { TeamResults = returnRecords,ShowSelector = false});

        }


        public ActionResult CheckTeamName(string TeamName)
        {
            Thread.Sleep(500);

            var Count = db.Teams.Count(t => t.Name == TeamName);
            var responsetext = "no";
            if (Count == 0)
            {
                responsetext = "yes";
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(responsetext, MediaTypeNames.Text.Plain);

        }

       
        public ActionResult Index()
        {
            var teams = db.Teams.ToList().OrderByDescending(TeamLogic.getStarsForTeam).Take(10).ToList();
            return View(new TeamIndexVM { AllTeams = teams});
        }

        // GET: Teams/Details/5
        [Route("Teams/Details/{teamname}")]
        public ActionResult Details(string teamname)
        {
            if (string.IsNullOrEmpty(teamname))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Team team = db.Teams.FirstOrDefault(t => t.Name.Replace(" ","") == teamname);
            if (team == null)
            {
                return HttpNotFound();
            }

            var TeamDetailsVM = new TeamDetailsVM();
            TeamDetailsVM.Team = team;
            TeamDetailsVM.IsAdmin = false;
            TeamDetailsVM.CurrentSponsorships = db.OpenPledges.Where(p => p.Contributors.FirstOrDefault(c => c.IsOriginator).Sinner.Team!= null && p.Contributors.FirstOrDefault(c => c.IsOriginator).Sinner.Team.ID == team.ID).ToList();
            TeamDetailsVM.ArchivedSponsorships = db.Pledges.Where(p => p.Closed || p.ExpiryDate < DateTime.UtcNow).Where(p => p.Contributors.FirstOrDefault(c => c.IsOriginator).Sinner.Team != null
                                                                                                                                  && p.Contributors.FirstOrDefault(c => c.IsOriginator).Sinner.Team.ID == team.ID).ToList();

            var ThisUser = CurrentUser();
            if (ThisUser != null)
            {
                if (ThisUser.IsTeamAdmin && ThisUser.Team.ID == team.ID)
                        TeamDetailsVM.IsAdmin = true;

            }
            TeamDetailsVM.AdminUser = db.Users.FirstOrDefault(u => u.IsTeamAdmin & u.TeamID == team.ID);

            return View(TeamDetailsVM);
        }

        // GET: Teams/Create
        public ActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Team team)
        {
                        
            if (ModelState.IsValid)
            {

                var user = CurrentUser();
                if (user.TeamID.HasValue)
                {
                    ModelState.AddModelError("", "You can't create a team if you're already a member of another team.");
                    return View(team);

                }

                db.Teams.Add(team);
                
                user.IsTeamAdmin = true;
                user.Team = team;
                Messaging.Add(Message.LevelEnum.alert_success,$"Team '{team.Name}' successfully created. You have been added as the administrator of this team", Message.TypeEnum.StickyAlert, user);
                db.SaveChanges();
                return RedirectToAction("Details", new { teamname=team.Name.Replace(" ","") });
            }
                        
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }

            var VM = new EditTeamVM
            {
                Team = team,
                TeamID = team.ID,
                Availability = team.Availability
            };
            return View(VM);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit( EditTeamVM TeamVM)
        {

            var user = CurrentUser();
            var team = db.Teams.FirstOrDefault(t => t.ID == TeamVM.TeamID);

            if (!ModelState.IsValid)
            {
                TeamVM.Team = team;
                return View(TeamVM);
            }

            if (team == null)
            {
                Messaging.Add(Message.LevelEnum.alert_danger, "something went wrong trying to update the Team.", Message.TypeEnum.TemporaryAlert, user);
            }
            else if (user.IsTeamAdmin && user.TeamID ==TeamVM.TeamID)
            {
                if (!string.IsNullOrEmpty(TeamVM.Description))
                    team.Description = TeamVM.Description;

                if (TeamVM.TeamImageID>0)
                team.ImageID = TeamVM.TeamImageID;

                if (!string.IsNullOrEmpty(TeamVM.Name))
                {
                    team.Name = TeamVM.Name;
                }

                team.Availability = TeamVM.Availability;

                Messaging.Add(Message.LevelEnum.alert_success, "Team details updated", Message.TypeEnum.StickyAlert, user);
                db.SaveChanges();
                return RedirectToAction("Details", new { TeamName = team.Name.Replace(" ","") });
            }
            else
            {
                Messaging.Add(Message.LevelEnum.alert_danger, "You don't have permission to edit this Team.", Message.TypeEnum.TemporaryAlert, user);
            }

            db.SaveChanges();
            //if we get here something went wrong so send back the edit view.
            if (team != null)
            {
                TeamVM.Team = team;
                TeamVM.Availability = team.Availability;
            }            
            return View(TeamVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveJoinRequests(List<string> IDs, int thisTeamID )
        {
                        
            var team = db.Teams.FirstOrDefault(t => t.ID == thisTeamID);
            if (team == null)
            {
                Messaging.Add(Message.LevelEnum.alert_danger , "something went wrong trying to approve users. please try again.", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            if (IDs== null)
            {
                Messaging.Add(Message.LevelEnum.alert_danger, "something went wrong trying to approve users. please try again.", Message.TypeEnum.TemporaryAlert, CurrentUser());
                db.SaveChanges();
                return RedirectToAction("Details", new { teamname = team.Name.Replace(" ", "") });
            }

            foreach (string ID in IDs)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == ID);
                if (user != null)
                {
                    user.TeamID = thisTeamID;
                }
                var TJR = db.TeamJoinRequests.FirstOrDefault(JR => JR.UserID == ID && JR.TeamID == thisTeamID);
                if (TJR != null)
                {
                    db.TeamJoinRequests.Remove(TJR);
                }                                
            }

            var EmailVM = new GenericEmailViewModel {RootURL = GetRootURL()};

            var TeamURL = Url.Action("Details", "Teams", new { teamname = team.Name.Replace(" ", "") }, Request.Url.Scheme);

            foreach (string ID in IDs)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == ID);
                if (user != null)
                {                    
                   await UserLogic.JoinTeamRequestApproved(user, team, EmailVM, TeamURL);                                   
                }
             
            }
                      
            Messaging.Add(Message.LevelEnum.alert_success, string.Format("{0} new members successfully approved", IDs.Count()), Message.TypeEnum.StickyAlert, CurrentUser());
            db.SaveChanges();
            return RedirectToAction("Details", new { teamname = team.Name.Replace(" ", "") });

        }

        private string GetRootURL() => $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}".TrimEnd("/".ToCharArray()[0]);
        

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
