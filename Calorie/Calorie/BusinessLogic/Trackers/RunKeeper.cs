using System;
using System.Linq;
using Calorie.Models;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using Calorie.Models.Trackers;
using RestSharp;


namespace Calorie.BusinessLogic.Trackers
{
    public class RunKeeper : ITracker
    {

        private TrackerLogic ParentTracker { get; set; }
        public RunKeeper(TrackerLogic _Parent)
        {
            ParentTracker = _Parent;
        }

        public string name => "RunKeeper";

        public Tracker.TrackerType Type => Tracker.TrackerType.RunKeeper;

        public string AuthenticateActionName => "RunKeeperAuth";

        public string DisassociateActionName
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        string ITracker.LogoURL => LogoURL;

        public static string LogoURL => "ThirdPartyLogos/runKeeper.png";

        string ITracker.ManageURL => ManageURL;
        public static string ManageURL => "https://runkeeper.com/settings/apps";

        public string AuthenticateStart()
        {
            var u = ConfigurationManager.AppSettings["RunKeeperAuthorizationURL"];
            u += "?client_id=" + ConfigurationManager.AppSettings["RunKeeperClientID"];
            u += "&redirect_uri=" + ParentTracker.Url.Action("RunKeeperAuthComplete", "Trackers", new { userID = ParentTracker.User.Id }, ParentTracker.RequestScheme );
            u += "&response_type=code";

            return u;
        }

        public bool AuthenticateComplete(string userID, string code,ApplicationDbContext db)
        {

            var User = db.Users.FirstOrDefault(u => u.Id == userID);

            
            if (User != null && User.Id == userID && !string.IsNullOrEmpty(code))
            {

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["grant_type"] = "authorization_code";
                    data["code"] = code;
                    data["client_id"] = ConfigurationManager.AppSettings["RunKeeperClientID"];
                    data["client_secret"] = ConfigurationManager.AppSettings["RunKeeperClientSecret"];
                    data["redirect_uri"] = ParentTracker.Url.Action("RunKeeperAuthComplete", "Trackers", new { userID = ParentTracker.User.Id }, ParentTracker.RequestScheme);

                    var response = wb.UploadValues(ConfigurationManager.AppSettings["RunKeeperAccessTokenURL"], "POST", data);
                    dynamic jsonresponse = System.Web.Helpers.Json.Decode(Encoding.UTF8.GetString(response));

                    if (!string.IsNullOrEmpty(jsonresponse?.access_token))
                    {

                        //success, we've got an access token
                        User.Trackers.Add(new Tracker() { AuthToken = jsonresponse.access_token, Type = Tracker.TrackerType.RunKeeper });
                        Messaging.Add(Models.Message.LevelEnum.alert_success, "You have successfully linked your account to RunKeeper!",Message.TypeEnum.StickyAlert , User);
                        db.SaveChanges();

                        return true;
                    }
                }
            }


            //if we get here, something went wrong :(    
            Messaging.Add(Models.Message.LevelEnum.alert_danger, "Oops! something went wrong trying to link your account to RunKeeper",Message.TypeEnum.TemporaryAlert, User);

            db.SaveChanges();
            return false;
        }


        public string DisassociateStart()
        {
            throw new NotImplementedException();
        }

        public string DisassociateComplete()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAccessCode(ApplicationDbContext db,Tracker t)
        {
           
            if (!string.IsNullOrEmpty(t?.AccessToken) && (t.AccessTokenExpiry > DateTime.Now))
                    return Task.Delay(0).ContinueWith(tsk => t.AccessToken);                             
           

            return Task.Delay(0).ContinueWith(tsk => "Runkeeper doesn't use access code");
        }


        #region "API CALLS"

        public static runKeeperSelectorVM getPastActivityData(ApplicationUser User, ApplicationDbContext db,Models.Pledges.Pledge pledge)
        {

            var results = new runKeeperSelectorVM();

            var client = new RestClient(ConfigurationManager.AppSettings["RunKeeperAPIURL"]);

            var request = new RestRequest("fitnessActivities", Method.GET);
            
            var Tracker = db.Trackers.FirstOrDefault(t => t.UserID == User.Id && t.Type == Models.Trackers.Tracker.TrackerType.RunKeeper);
            if (Tracker == null) return results;

            var AccessToken = Tracker.AuthToken;

            request.AddHeader("Accept", "application/vnd.com.runkeeper.FitnessActivityFeed+json");
            request.AddHeader("Authorization", "Bearer " + AccessToken);
            
            
            // execute the request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return results;
            var resultJSON = System.Web.Helpers.Json.Decode(response.Content);
            foreach (var itm in resultJSON.items)
            {
                string ident = itm.uri;

                if (User.Offsetters.Any(o => o.ThirdPartyIdentifier == ident))
                {
                    results.Used.Add(new runKeeperVM(System.Web.Helpers.Json.Encode(itm), User));
                }
                else if (pledge.Activity_Types.Count == 0 ||
                         pledge.Activity_Types.Any(at => at.Activity.ToString().Replace("_", "") == itm.type))
                {
                    results.Available.Add(new runKeeperVM(System.Web.Helpers.Json.Encode(itm), User));
                }
                else
                {
                    results.UnAvailable.Add(new runKeeperVM(System.Web.Helpers.Json.Encode(itm), User));
                }
            }

            return results;

        }

        public static runKeeperVM getSpecificPastActivityData(string Ident, ApplicationUser User, ApplicationDbContext db)
        {

            var result = new runKeeperVM();
            var client = new RestClient(ConfigurationManager.AppSettings["RunKeeperAPIURL"]);
            var request = new RestRequest(Ident, Method.GET);

            var Tracker = db.Trackers.FirstOrDefault(t => t.UserID == User.Id && t.Type == Models.Trackers.Tracker.TrackerType.RunKeeper);
            if (Tracker == null) return result;

            var AccessToken = Tracker.AuthToken;

            request.AddHeader("Accept", "application/vnd.com.runkeeper.FitnessActivity+json");
            request.AddHeader("Authorization", "Bearer " + AccessToken);


            // execute the request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return result;

            //var resultJSON = System.Web.Helpers.Json.Decode(response.Content);
            return new runKeeperVM(response.Content, User);
            

        }
        
#endregion

  }
}