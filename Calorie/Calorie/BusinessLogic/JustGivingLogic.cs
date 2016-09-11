using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Calorie.Models;
using Calorie.Models.Charities;
using Calorie.Models.Pledges;
using RestSharp;

namespace Calorie.BusinessLogic
{
    public class JustGivingLogic
    {

        public static string getPoweredByJustGivingHTML() => "<a href ='https://www.justgiving.com'><img class='justGivingPoweredByImage' src='data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAxNi4wLjMsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+DQo8c3ZnIHZlcnNpb249IjEuMSIgaWQ9IkxheWVyXzEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiIHg9IjBweCIgeT0iMHB4Ig0KCSB3aWR0aD0iMTg1cHgiIGhlaWdodD0iNDkuOTg3cHgiIHZpZXdCb3g9IjAgMCAxODUgNDkuOTg3IiBlbmFibGUtYmFja2dyb3VuZD0ibmV3IDAgMCAxODUgNDkuOTg3IiB4bWw6c3BhY2U9InByZXNlcnZlIj4NCjxnPg0KCTxyZWN0IHg9Ijk3LjciIHk9IjE3LjI1MiIgZmlsbD0iI0FEMjlCNiIgd2lkdGg9IjUuNzExIiBoZWlnaHQ9IjE3LjI5MSIvPg0KCTxwYXRoIGZpbGw9IiNBRDI5QjYiIGQ9Ik05NS44NzgsMjAuOTZoLTguMDcybC00LjcyMyw0LjcxM2g2LjQ4NGMtMC44NDYsMS44OTUtMi43NSwzLjIxNy00Ljk1OSwzLjIxNw0KCQljLTIuOTk3LDAtNS40MjgtMi40MzItNS40MjgtNS40MjdjMC0zLDIuNDMxLTUuNDM1LDUuNDI4LTUuNDM1YzEuMDg1LDAsMi4xLDAuMzE4LDIuOTQ3LDAuODY4bDQuMzczLTQuMzY5DQoJCWMtMS45OTgtMS42NC00LjU1MS0yLjYyNC03LjMzNy0yLjYyNGMtNi4zODEsMC0xMS41NTMsNS4xNzMtMTEuNTUzLDExLjU2YzAsNi4zNzgsNS4xNzIsMTEuNTUsMTEuNTUzLDExLjU1DQoJCWM2LjM4NywwLDExLjU2Mi01LjE3MiwxMS41NjItMTEuNTVDOTYuMTUzLDIyLjYwMiw5Ni4wNTYsMjEuNzYyLDk1Ljg3OCwyMC45NiIvPg0KCTxyZWN0IHg9Ijk3LjciIHk9IjEyLjMwMiIgZmlsbD0iI0FEMjlCNiIgd2lkdGg9IjUuNzExIiBoZWlnaHQ9IjMuNDg2Ii8+DQoJPHBvbHlnb24gZmlsbD0iI0FEMjlCNiIgcG9pbnRzPSIxMTYuMjgsMzQuNjY0IDExMS4zNDcsMzQuNjY0IDExMS4zMDEsMzQuNTQ0IDEwNC41NDEsMTcuMjQzIDExMC42MzUsMTcuMjQzIDExMy44NDksMjYuODU4IA0KCQkxMTcuMDg5LDE3LjI0MyAxMjMuMDksMTcuMjQzIAkiLz4NCgk8cmVjdCB4PSIxMjQuMTM4IiB5PSIxNy4yNTIiIGZpbGw9IiNBRDI5QjYiIHdpZHRoPSI1LjcxNyIgaGVpZ2h0PSIxNy4yOTEiLz4NCgk8cmVjdCB4PSIxMjQuMTM4IiB5PSIxMi4zMDIiIGZpbGw9IiNBRDI5QjYiIHdpZHRoPSI1LjcxNyIgaGVpZ2h0PSIzLjQ4NiIvPg0KCTxwYXRoIGZpbGw9IiNBRDI5QjYiIGQ9Ik0xNDcuNTkxLDM0LjU0M2gtNS43MDd2LTkuNTg5YzAtMS44LTAuNzQ2LTIuNzE4LTIuMjE1LTIuNzE4Yy0xLjkwNCwwLTIuMjk2LDEuNDgyLTIuMjk2LDIuNzE4djkuNTg5DQoJCWgtNS43MTZWMTcuMjUyaDMuOTU4bDEuNTY5LDEuNTU5YzEuMjM4LTEuMjU1LDIuNjc0LTEuODY0LDQuMzU3LTEuODY0YzMuNzMyLDAsNi4wNDksMi41MTQsNi4wNDksNi41NjZWMzQuNTQzeiIvPg0KCTxwYXRoIGZpbGw9IiNBRDI5QjYiIGQ9Ik0xNy42MzEsMzQuODQyYy0yLjk4MywwLTUuNDYtMS4xMjktNy4zMzktMy4zNjdMOS45NzQsMzEuMWwzLjg3MS0zLjg2OWwwLjIyMywwLjI0Mg0KCQljMS4yMjUsMS4zNDYsMi4yNTMsMS45MjYsMy40NDIsMS45MjZjMS41NDQsMCwyLjI3MS0wLjkzMiwyLjI3MS0yLjkzNFYxMi4zMDJoNS44OTlWMjYuNjgNCgkJQzI1LjY4LDMxLjg2NSwyMi43NDQsMzQuODQyLDE3LjYzMSwzNC44NDIiLz4NCgk8cGF0aCBmaWxsPSIjQUQyOUI2IiBkPSJNMTYzLjEyLDE3LjI1MmwtMS40NTMsMS40NjNjLTEuMzczLTEuMjEtMi45MTYtMS43NjgtNC44My0xLjc2OGMtMy44NzYsMC03LjgwMywyLjgwNC03LjgwMyw4LjE2MnYwLjA2NA0KCQljMCw1LjM2MiwzLjkyNyw4LjE2Nyw3LjgwMyw4LjE2N2MxLjgzOCwwLDMuMjc5LTAuNTE2LDQuNjA3LTEuNjU3Yy0wLjI0NiwyLjA0Mi0xLjQ3OSwzLjQwMS0zLjk0NSwzLjQwMQ0KCQljLTEuNDg2LDAtMi43NjYtMC4yNjgtNC4wOTgtMC44NjRsLTMuNzM2LDMuNzM3bDAuNTE2LDAuMjdjMi4xNzUsMS4xNTEsNC43NDYsMS43NjQsNy40MzksMS43NjRjNi41ODQsMCw5LjY0Ni0zLjM0OCw5LjY0Ni05LjYNCgkJVjE3LjI1MkgxNjMuMTJ6IE0xNjEuODQxLDI1LjE3NmMwLDEuOTk1LTEuNTQ3LDMuNDQtMy42OCwzLjQ0Yy0yLjE0NSwwLTMuNjM5LTEuNDEzLTMuNjM5LTMuNDR2LTAuMDY3DQoJCWMwLTEuOTk1LDEuNTMxLTMuNDQsMy42MzktMy40NGMyLjEzMywwLDMuNjgsMS40NDUsMy42OCwzLjQ0VjI1LjE3NnoiLz4NCgk8cGF0aCBmaWxsPSIjQUQyOUI2IiBkPSJNMzMuNjQ1LDM0Ljg0MmMtMy43MzMsMC02LjA1LTIuNTE1LTYuMDUtNi41NjdWMTcuMjUyaDUuNzF2OS41NzVjMCwxLjgwOCwwLjc0NSwyLjcyMiwyLjIwNSwyLjcyMg0KCQljMS45MDcsMCwyLjMwMi0xLjQ4MiwyLjMwMi0yLjcyMnYtOS41NzVoNS43MjV2MTcuMjkxaC0zLjk2N2wtMS41NjYtMS41NjRDMzYuNzYzLDM0LjIzNCwzNS4zMzUsMzQuODQyLDMzLjY0NSwzNC44NDIiLz4NCgk8cGF0aCBmaWxsPSIjQUQyOUI2IiBkPSJNNjcuMTY1LDM0LjgxNGMtMy41NDQsMC01LjI3MS0xLjc3Mi01LjI3MS01LjQyNnYtNy4wODhoLTEuOTI3di01LjA0OWgxLjkyN3YtNC45NWg1LjcxNHY0Ljk1aDMuNzk3djUuMDQ5DQoJCWgtMy43OTd2Ni4zMzhjMCwwLjc2OCwwLjIxMSwxLDAuOTE2LDFjMC43MDQsMCwxLjM4LTAuMTY2LDEuOTg2LTAuNTA2bDAuODM3LTAuNDUzdjQuOTU3bC0wLjI3MywwLjE2DQoJCUM2OS45MDcsMzQuNDkzLDY4LjY3MSwzNC44MTQsNjcuMTY1LDM0LjgxNCIvPg0KCTxwYXRoIGZpbGw9IiNBRDI5QjYiIGQ9Ik01My44MzEsMjMuOWwtMC41My0wLjE3NmMtMS4zMzMtMC40NTItMi41OTgtMC44NzctMi41OTgtMS40NDJ2LTAuMDYxYzAtMC41NjIsMC42OTktMC43NjMsMS4zNTQtMC43NjMNCgkJYzAuOTE2LDAsMi4xNTMsMC4zODMsMy40OCwxLjA1NmwzLjI5Ni0zLjI5NWwtMC4yNjItMC4xNzNjLTEuODU0LTEuMjQ4LTQuMjU0LTEuOTkyLTYuNDItMS45OTJjLTMuODc0LDAtNi40NzQsMi4yNzYtNi40NzQsNS42NjMNCgkJdjAuMDY1YzAsMy4zNzMsMi42NTIsNC42MDYsNS4yNCw1LjM2M2MwLjIxNywwLjA2OSwwLjQyOCwwLjEzLDAuNjM0LDAuMTk0YzEuMzYxLDAuNDAxLDIuNTM4LDAuNzQ3LDIuNTM4LDEuMzYydjAuMDY0DQoJCWMwLDAuNzAxLTAuODY4LDAuODUyLTEuNiwwLjg1MmMtMS4zOTIsMC0zLjAzNC0wLjU4Ny00LjU5Mi0xLjYyM2wtMy4xNDQsMy4xNDlsLTAuMDQ1LDAuMDY0bDAuMjc5LDAuMjE1DQoJCWMyLjEyNSwxLjY3OSw0Ljc0NywyLjYwMyw3LjM4MSwyLjYwM2M0LjIyMywwLDYuNzQ4LTIuMTQ2LDYuNzQ4LTUuNzU3di0wLjA2NkM1OS4xMTYsMjUuNjY0LDU1LjY3MywyNC41Miw1My44MzEsMjMuOSIvPg0KCTxwb2x5Z29uIGZpbGw9IiNBRDI5QjYiIHBvaW50cz0iMTY4LjQ1LDE4LjAzNSAxNjkuMjY5LDE4LjAzNSAxNjkuMjY5LDIwLjAwNyAxNzAuMTg4LDIwLjAwNyAxNzAuMTg4LDE4LjAzNSAxNzAuOTk3LDE4LjAzNSANCgkJMTcwLjk5NywxNy4yNTIgMTY4LjQ1LDE3LjI1MiAJIi8+DQoJPHBvbHlnb24gZmlsbD0iI0FEMjlCNiIgcG9pbnRzPSIxNzMuNDQ1LDE3LjI1MiAxNzIuODQ5LDE4LjIxNiAxNzIuMjUzLDE3LjI1MiAxNzEuMjk4LDE3LjI1MiAxNzEuMjk4LDIwLjAwNyAxNzIuMjAyLDIwLjAwNyANCgkJMTcyLjIwMiwxOC42MzUgMTcyLjYxNywxOS4yNzcgMTczLjA2NSwxOS4yNzcgMTczLjQ4NSwxOC42MzUgMTczLjQ4NSwyMC4wMDcgMTc0LjQxMSwyMC4wMDcgMTc0LjQxMSwxNy4yNTIgCSIvPg0KPC9nPg0KPC9zdmc+DQo='/></a><div class='justGivingPoweredByText'>*Details From </div>";

        public static Charity getOrCreateCharityRecordFromJustGivingCharityID(string JustGivingID)
        {

            JustGivingID = JustGivingID.Replace(" ", "");

            var db = new ApplicationDbContext();

            var charity = db.Charities.FirstOrDefault(c => c.JustGivingCharityID == JustGivingID);

            if (string.IsNullOrEmpty(charity?.JustGivingCharityImageURL))
                    ReloadCharityData(charity, db);
            
            if (charity != null)
                return charity;

            var result = GetCharityDetailsByJustGivingID(JustGivingID);
            if (string.IsNullOrEmpty(result))
                return null;

            var resultJSON = Json.Decode(result);

            var newCharity = new Charity
            {
                JustGivingCharityBlob = result,
                JustGivingCharityID = JustGivingID,
                JustGivingCharityImageURL = resultJSON.logoAbsoluteUrl,
                JustGivingRegistrationNumber = resultJSON.registrationNumber,
                Name = resultJSON.name,
                CharityURL = resultJSON.websiteUrl,
                Description = resultJSON.description
            };

            db.Charities.Add(newCharity);
            db.SaveChanges();
            return newCharity;
        }

        
        public static bool RecacheJustGivingCharities()
        {
            var db = new ApplicationDbContext();

            var ToUpdate = db.Charities.Where(c => !string.IsNullOrEmpty(c.JustGivingCharityID)).ToList();
            
            foreach (var _char in ToUpdate)
                    ReloadCharityData(_char,db);

            return true;
        }

        private static bool ReloadCharityData(Charity _Charity,ApplicationDbContext db)
        {

        if (_Charity==null) throw new ArgumentNullException(nameof(_Charity));
        if (db == null) throw new ArgumentNullException(nameof(db));


        var reloadedData = GetCharityDetailsByJustGivingID(_Charity.JustGivingCharityID);
        var relatedDataJSON = Json.Decode(reloadedData);

        _Charity.JustGivingCharityBlob = reloadedData;
        _Charity.JustGivingCharityImageURL = relatedDataJSON.logoAbsoluteUrl;
        _Charity.JustGivingRegistrationNumber = relatedDataJSON.registrationNumber;
        _Charity.Name = relatedDataJSON.name;
        _Charity.CharityURL = relatedDataJSON.websiteUrl;
        _Charity.Description = relatedDataJSON.description;

        db.SaveChanges();
            return true;

        }


        public static string GetCharityDetailsByJustGivingID(string JustGivingID)
        {


            var client = new RestClient(ConfigurationManager.AppSettings["JustGivingAPIURL"]);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("{appID}/v1/charity/{charityID}", Method.GET);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method

            request.AddUrlSegment("appID", ConfigurationManager.AppSettings["JustGivingAppId"]); // replaces matching token in request.Resource
            request.AddUrlSegment("charityID", JustGivingID); // replaces matching token in request.Resource


            // easily add HTTP Headers
            //request.AddHeader("Content-type", "application/json");
            request.AddHeader("Accept", "application/json");


            // execute the request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content; 
            }
            return string.Empty; //something went wrong.
            
        }

        public static List<Charity> SearchCharities(string SearchTerm)
        {
            
            var client = new RestClient(ConfigurationManager.AppSettings["JustGivingAPIURL"]);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("{appID}/v1/charity/search", Method.GET);
            request.AddParameter("q", SearchTerm); // adds to POST or URL querystring based on Method

            request.AddUrlSegment("appID", ConfigurationManager.AppSettings["JustGivingAppId"]); // replaces matching token in request.Resource


            // easily add HTTP Headers            
            request.AddHeader("Accept", "application/json");

            // execute the request
            IRestResponse response = client.Execute(request);
            //return response.Content; // raw content as string
            var CharList = new List<Charity>();
            //result.charityId,result.logoUrl,result.name,result.description,result.websiteUrl
            foreach(var result in Json.Decode(response.Content).charitySearchResults)
            {
                CharList.Add(new Charity { JustGivingRegistrationNumber=result.registrationNumber, CharityURL = result.websiteUrl, JustGivingCharityID = result.charityId, Name = result.name, Description = result.description, JustGivingCharityImageURL = result.logoUrl });
            }

            return CharList;

        }

        public static Dictionary<string,string> getCountrys()
        {

            var client = new RestClient(ConfigurationManager.AppSettings["JustGivingAPIURL"]);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("{appID}/v1/countries", Method.GET);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method

            request.AddUrlSegment("appID", ConfigurationManager.AppSettings["JustGivingAppId"]); // replaces matching token in request.Resource           


            // easily add HTTP Headers
            //request.AddHeader("Content-type", "application/json");
            request.AddHeader("Accept", "application/json");


            var dict = new Dictionary<string, string>();
            // execute the request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resultJSON = Json.Decode(response.Content);
                
                foreach (var itm in resultJSON)
                {
                    dict.Add(itm.countrycode,itm.name);
                }
                
            }
            return dict;
            
        }

        public static string GetPaymentURL(PledgeContributors contrib, UrlHelper Url, HttpRequestBase Request)
        {

            var BaseURL = ConfigurationManager.AppSettings["JustGivingURL"];
            
            var PayURL = BaseURL + "4w350m3/donation/direct/charity/" + contrib.Pledge.Charity.JustGivingCharityID + "?";
            PayURL += "amount=" + contrib.Amount;
            PayURL += "&currency=" + contrib.Currency ;
            PayURL += "&reference=" + contrib.ID;
            PayURL += "&exitURL=" + Url.Action("PaymentComplete", "Pledges", new {id = contrib.ID}, Request.Url.Scheme);

            return PayURL;                 

        }


    }
}