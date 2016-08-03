using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Calorie.Models;
using System.Text;
using System.Threading.Tasks;
using Calorie.Models.Trackers;

namespace Calorie.BusinessLogic.Trackers
{
    public class MicrosoftHealth : ITracker
    {

        private TrackerLogic ParentTracker { get; set; }
        public MicrosoftHealth(TrackerLogic _Parent)
        {
            ParentTracker = _Parent;
        }

        public string name => "Microsoft Health";

        string ITracker.LogoURL => LogoURL;

        public static string LogoURL => "ThirdPartyLogos/MSHealth.png";

        string ITracker.ManageURL => ManageURL;
        public static string ManageURL => "https://account.live.com/consent/Manage";

        public Tracker.TrackerType Type => Tracker.TrackerType.MicrosoftHealth;

        public string AuthenticateActionName => "MicrosoftHealthAuth";

        public string DisassociateActionName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string AuthenticateStart()
        {

            var u = ConfigurationManager.AppSettings["MicrosoftHealthAuthorizationURL"];

            u += "?client_id=" + ConfigurationManager.AppSettings["MicrosoftHealthClientID"] ;
            u += "&scope=" + "mshealth.ReadActivityHistory mshealth.ReadActivityLocation offline_access mshealth.ReadProfile";
            u += "&response_type=code";
            u += "&redirect_uri=" + HttpUtility.UrlEncode(ParentTracker.Url.Action("MicrosoftHealthAuthCompleted", "Trackers", null, ParentTracker.RequestScheme));                                       

            return u;
        }

        public async Task<string> GetAccessCode(ApplicationDbContext db,Tracker t)
        {

            if (t == null)
                  return "No Authorization Code";
            

            if (!string.IsNullOrEmpty(t.AccessToken) && (t.AccessTokenExpiry > DateTime.Now))
                return t.AccessToken;

            if (!string.IsNullOrEmpty(t.RefreshToken))
            {

                var Headers = new List<KeyValuePair<string, string>>();

                var Data = new List<KeyValuePair<string, string>>();
                Data.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["MicrosoftHealthClientID"]));
                Data.Add(new KeyValuePair<string, string>("redirect_uri", ParentTracker.Url.Action("MicrosoftHealthAuthCompleted", "Trackers", null, ParentTracker.RequestScheme)));

                Data.Add(new KeyValuePair<string, string>("client_secret", ConfigurationManager.AppSettings["MicrosoftHealthClientSecret"]));
                Data.Add(new KeyValuePair<string, string>("refresh_token", t.RefreshToken));
                Data.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));

                var u = ConfigurationManager.AppSettings["MicrosoftHealthTokenURL"];

                try
                {
                    var result = await GenericLogic.HttpPost(Headers, Data, u);

                    dynamic jsonresponse = System.Web.Helpers.Json.Decode(result);

                    string accesstoken = jsonresponse?.access_token;

                    if (!string.IsNullOrEmpty(accesstoken))
                    {
                        t.AccessToken = accesstoken;
                        t.RefreshToken = jsonresponse?.refresh_token;
                        t.AccessTokenExpiry = DateTime.Now.AddSeconds((jsonresponse?.expires_in * 0.9));
                        t.ThirdPartyUserID = jsonresponse?.user_id;
                        db.SaveChanges();
                        return t.AccessToken;
                    }

                    //there was a problem.
                    Messaging.Add(Message.LevelEnum.alert_danger, "Oops! something went wrong trying to request information from Microsoft Health", Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                    db.SaveChanges();
                    return string.Empty;
                }
                catch
                {
                    Messaging.Add(Message.LevelEnum.alert_danger, "Oops! something went wrong trying to request information from Microsoft Health", Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                    db.SaveChanges();
                    return string.Empty;
                }

                   
            }
            else
            {

                var Headers = new List<KeyValuePair<string, string>>();
                            
                var Data = new List<KeyValuePair<string, string>>();
                Data.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["MicrosoftHealthClientID"]));
                Data.Add(new KeyValuePair<string, string>("redirect_uri", ParentTracker.Url.Action("MicrosoftHealthAuthCompleted", "Trackers", null, ParentTracker.RequestScheme)));
               
                Data.Add(new KeyValuePair<string, string>("client_secret", ConfigurationManager.AppSettings["MicrosoftHealthClientSecret"]));
                Data.Add(new KeyValuePair<string, string>("code", t.AuthToken));
                Data.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                    
                var u = ConfigurationManager.AppSettings["MicrosoftHealthTokenURL"];


                try {
                    var result = await GenericLogic.HttpPost(Headers, Data, u);

                    dynamic jsonresponse = System.Web.Helpers.Json.Decode(result);

                    string accesstoken = jsonresponse?.access_token;

                    if (!string.IsNullOrEmpty(accesstoken))
                    {
                        t.AccessToken = accesstoken;
                        t.RefreshToken = jsonresponse?.refresh_token;
                        t.AccessTokenExpiry = DateTime.Now.AddSeconds((jsonresponse?.expires_in * 0.9));
                        t.ThirdPartyUserID = jsonresponse?.user_id;
                        db.SaveChanges();
                        return t.AccessToken;
                    }
                    //there was a problem.
                    Messaging.Add(Message.LevelEnum.alert_danger, "Oops! something went wrong trying to request information from Microsoft Health", Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                    db.SaveChanges();
                    return string.Empty;
                }
                catch
                {
                    Messaging.Add(Message.LevelEnum.alert_danger, "Oops! something went wrong trying to request information from Microsoft Health", Message.TypeEnum.TemporaryAlert, ParentTracker.User);
                    db.SaveChanges();
                    return string.Empty;
                }
                   
                  
            }
        }

    
        public bool AuthenticateComplete(string userID, string code,ApplicationDbContext db)
        {
            
            var User = db.Users.FirstOrDefault(u => u.Id == userID);
            
            if (User != null && User.Id == userID && !string.IsNullOrEmpty(code))
            {

                User.Trackers.Add(new Tracker() { AuthToken = code, Type = Tracker.TrackerType.MicrosoftHealth  });
                
                Messaging.Add(Message.LevelEnum.alert_success, "You have successfully linked your account to Microsoft Health", Message.TypeEnum.StickyAlert , User);
                db.SaveChanges();
                return true;
              
            }


            //if we get here, something went wrong :(    
            Messaging.Add(Message.LevelEnum.alert_danger, "Oops! something went wrong trying to link your account to Microsoft Health", Message.TypeEnum.TemporaryAlert, User);
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
    }
}