using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Calorie.Models;
using System.Text;
using System.Threading.Tasks;
using Calorie.Models.Trackers;
using RestSharp;

namespace Calorie.BusinessLogic.Trackers
{
    public class FitBit : ITracker
    {

        private TrackerLogic ParentTracker { get; set; }

        public FitBit(TrackerLogic _Parent) 
        {
            ParentTracker = _Parent;
        }

        public string name { get; } = "Fitbit";

        string ITracker.LogoURL => LogoURL;
        public static string LogoURL => "ThirdPartyLogos/fitbit.svg";


        string ITracker.ManageURL=> ManageURL;
        public static string ManageURL => "https://www.fitbit.com/user/profile/apps";


        public Tracker.TrackerType Type { get; } = Tracker.TrackerType.Fitbit;

        public string AuthenticateActionName { get; } = "FitbitAuth";

        public string DisassociateActionName
        {
            get { throw new NotSupportedException(); }
        }

        public string AuthenticateStart()
        {

            var u = ConfigurationManager.AppSettings["FitbitAuthorizationUrl"];
            u += "?response_type=code";
            u += "&client_id=" + ConfigurationManager.AppSettings["FitbitClientID"];
            u += "&redirect_uri=" +
                 HttpUtility.UrlEncode(ParentTracker.Url.Action("FitbitAuthCompleted", "Trackers", null,
                     ParentTracker.RequestScheme));
            u += "&state=" + Uri.EscapeUriString(ParentTracker.User.Id);
            u += "&scope=" + Uri.EscapeUriString("activity location");
            
            return u;
        }

        public  async Task<string> GetAccessCode(ApplicationDbContext db,Tracker t)
        {

            if (t == null)
                return "No Authorization Code";
            


            if (!string.IsNullOrEmpty(t.AccessToken) && (t.AccessTokenExpiry > DateTime.Now))
                return t.AccessToken;

            if (!string.IsNullOrEmpty(t.RefreshToken))
            {
                var Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Authorization", "Basic " + GetAuthorizationCode())
                };

                var Data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("refresh_token", t.RefreshToken),
                    new KeyValuePair<string, string>("grant_type", "refresh_token")
                };

                var result =
                    await GenericLogic.HttpPost(Headers, Data, ConfigurationManager.AppSettings["FitbitTokenURL"]);
                dynamic jsonresponse = System.Web.Helpers.Json.Decode(result);

                string accesstoken = jsonresponse?.access_token;
                if (!string.IsNullOrEmpty(accesstoken))
                {
                    t.AccessToken = accesstoken;
                    t.RefreshToken = jsonresponse?.refresh_token;
                    t.AccessTokenExpiry = DateTime.Now.AddSeconds((jsonresponse?.expires_in*0.9));
                    t.ThirdPartyUserID = jsonresponse?.user_id;
                    db.SaveChanges();
                    return t.AccessToken;
                }

                Messaging.Add(Message.LevelEnum.alert_danger,
                    "Oops! something went wrong trying to request information from Fitbit",
                    Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                db.SaveChanges();
                return string.Empty;
            }
            else
            {

                var Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Authorization", "Basic " + GetAuthorizationCode())
                };

                var Data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", t.AuthToken),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["FitbitClientID"]),
                    new KeyValuePair<string, string>("redirect_uri",
                        ParentTracker.Url.Action("FitbitAuthCompleted", "Trackers", null,
                            ParentTracker.RequestScheme))
                };

                var result =
                    await GenericLogic.HttpPost(Headers, Data, ConfigurationManager.AppSettings["FitbitTokenURL"])
                    ;

                dynamic jsonresponse = System.Web.Helpers.Json.Decode(result);

                string accesstoken = jsonresponse?.access_token;

                if (!string.IsNullOrEmpty(accesstoken))
                {
                    t.AccessToken = accesstoken;
                    t.RefreshToken = jsonresponse?.refresh_token;
                    t.AccessTokenExpiry = DateTime.Now.AddSeconds((jsonresponse?.expires_in*0.9));
                    t.ThirdPartyUserID = jsonresponse?.user_id;
                    db.SaveChanges();
                    return t.AccessToken;
                }
                else
                {
                    //there was a problem.
                    Messaging.Add(Message.LevelEnum.alert_danger,
                        "Oops! something went wrong trying to request information from Fitbit",
                        Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                    db.SaveChanges();
                    return string.Empty;
                }

            }
        }

        private string GetAuthorizationCode()
        {
            var codeBytes =
                Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["FitbitClientID"] + ":" +
                                       ConfigurationManager.AppSettings["FitbitClientSecret"]);
            return Convert.ToBase64String(codeBytes);
        }

        public async Task<bool> AuthenticateComplete(string userID, string code, ApplicationDbContext db)
        {

            var User = db.Users.FirstOrDefault(u => u.Id == userID);
            if (User != null && User.Id == userID && !string.IsNullOrEmpty(code))
            {

                var t = new Tracker() {AuthToken = code, Type = Tracker.TrackerType.Fitbit};
                User.Trackers.Add(t);
                await GetAccessCode(db,t);

                Messaging.Add(Message.LevelEnum.alert_success, "You have successfully linked your account to Fitbit",
                    Message.TypeEnum.StickyAlert, User);
                db.SaveChanges();
                return true;

            }


            //if we get here, something went wrong :(    
            Messaging.Add(Message.LevelEnum.alert_danger,
                "Oops! something went wrong trying to link your account to Fitbit", Message.TypeEnum.TemporaryAlert,
                User);
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


        #region "API CALLS"

        public  async Task<FitbitSelectorVM> getFitBitDataForDate(DateTime inputDateTime, ApplicationUser user)

        {

            var results = new FitbitSelectorVM();
            var db = new ApplicationDbContext();

            var Tracker = db.Trackers.FirstOrDefault(t => t.UserID == user.Id && t.Type == Models.Trackers.Tracker.TrackerType.Fitbit);
            if (Tracker == null) return results;

            
            var client = new RestClient(ConfigurationManager.AppSettings["FitbitAPIURL"]);

            var request = new RestRequest("user/{userID}/activities/date/{inputDate}.json", Method.GET);
            request.AddUrlSegment("userID", Tracker.ThirdPartyUserID); 
            request.AddUrlSegment("inputDate", inputDateTime.ToString("yyyy-MM-dd")); 
            

            var AccessToken = await GetAccessCode(db,Tracker);
            if (string.IsNullOrEmpty(AccessToken))
            {
                return null;
            }

            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("Accept-Language", "en_GB");


            // execute the request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return results;
            var resultJSON = System.Web.Helpers.Json.Decode(response.Content);

            foreach (var itm in resultJSON.activities)
                   results.Available.Add(new FitbitVM(System.Web.Helpers.Json.Encode(itm),inputDateTime,user));                       
            

            return results;
        }
        
        #endregion 



    }
}