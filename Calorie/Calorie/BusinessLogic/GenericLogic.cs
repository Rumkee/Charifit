using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Calorie.Models.Misc;

namespace Calorie.BusinessLogic
{
    public class GenericLogic
    {

        public class HTML
        {
            
        public static string CALORIE_HTML = "<span class='glyphicon glyphicon-fire'></span>";
        public static string HOURS_HTML = "<span class='glyphicon glyphicon-time'></span>";
        public static string MILES_HTML = "<span class='glyphicon glyphicon-road'></span>";
        public static string SESSIONS_HTML = " <span class='glyphicon glyphicon-heart'></span>";
        public static string SPONSORED_HTML = "<i class='fa fa-money'></i>";
        public static string RAISED_HTML = "<i class='fa fa-money'></i>";
        public static string TEAM_HTML = "<span class='glyphicon glyphicon-flag'></span>";
        public static string USER_HTML = "<span class='glyphicon glyphicon-user'></span>";
            

        }

        public static int? GetInt(string input)
        {

            int r;
            if (int.TryParse(input, out r))
                    return r;
            
            return null;      

        }

        public static DateTime? GetDateTime(string inputDate)
        {

            DateTime r ;
            if (DateTime.TryParse(inputDate, out r))
                    return r;
            
            return null;

        }


        public static async Task<string>  HttpPost(List<KeyValuePair<string,string>> Headers, List<KeyValuePair<string, string>> FormData,string URL)
    {

        HttpClient httpClient = new HttpClient();
        
        foreach(var Header in Headers)
        {
            httpClient.DefaultRequestHeaders.Add(Header.Key , Header.Value );
        }               

        var formContent = new FormUrlEncodedContent(FormData);
        
        HttpResponseMessage response = await httpClient.PostAsync(URL, formContent);
        return await response.Content.ReadAsStringAsync();
   }

        public static async Task<HttpResponseMessage> HttpPut(object body, string URL)
        {

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
            return await httpClient.PutAsJsonAsync(URL, body);
            
        }


        public static FAQSVM GetFAQs(UrlHelper Url)
        {
            var RegisterURL = Url.Action("Register", "Account");
            var CreateURL = Url.Action("Create", "Pledges");
            var RegisterJustGiving = Url.Action("RegisterJustGiving", "Account");

            var FAQ1 = new FaqVM
            {
                Question = "Who or what is a “ChariF<span class='calorie-green-color'>Ώ</span>t” ?",
                Answer = "ChariFit is a portmanteau of the words Charity and Fitness.<br><br>" +
                            "We are a non profit organisation whose purpose is to raise funds for charities and encourage healthy lifestyles.",

                ID = "1"
            };

            var FAQ2 = new FaqVM
            {
                Question = "So you’re a charity ?",
                Answer = "No we are a u.k. company limited by guarantee (a non profit org) as such we do not have shareholders and do not distribute profits.<br><br>" +
                        "The members and trustee’s of the company are charged with building a socially responsible organization which aims to deliver as much fund raising for charities as is possible.",

                ID = "2"
            };
            var FAQ3 = new FaqVM
            {
                Question = "What’s you cut of the action/slice of the cake ?",
                Answer = "Currently? Nothing.<br><br>JustGiving will take a small proportion of the contribution amount, see <a href='https://www.justgiving.com/fees/'>JustGiving fees</a>.",
                ID = "3"
            };

            var FAQ41 = new FaqVM
            {
                Question = "How does this work ?",
                Answer = $"<h4>If you have a fitness tracker:-</h4><a href='{RegisterURL}'>Create an account</a>, hook up your fitness tracker's account to ChariFit and start logging your workout activities against pledges, to help reach their goals. It’s that simple!" +
                          "<BR><BR>" +
                          "<h4>If you're making a donation:-</h4>" +
                          $"<a href='{RegisterURL}'>Create an account</a>," +
                          $" then start your first pledge.<br><br>" +
                          "You choose the charity, the deadline, the type of activities and the quantity of activities.<br>" +
                          "Once the ChariFit community has logged sufficient activities to reach the goal you specified within the deadline your pledge payment will be taken.<br><br>" +
                          $"Payments are handled entirely by JustGiving so you might want us to <a href='{RegisterJustGiving}'>create a JustGiving account for you </a> if you don't have one already.",
                ID = "41"
            };

            var FAQ4 = new FaqVM
            {
                Question = "I have a fitness tracker, how does this work ?",
                Answer = "<a href='" + RegisterURL + "'>Create an account</a>, hook up your fitness tracker account to ChariFit and start logging your workout activities against pledges. It’s that simple!",
                ID = "4"
            };




            var FAQ5 = new FaqVM
            {
                Question = "I want to donate to charity, how does this work ?",
                Answer = "<a href='" + RegisterURL + "'>Create an account</a>," +
                         " then use the \"<a href='" + CreateURL + "'>Add Pledge</a>\" option to set up a new pledge.<br><br>" +
                         "You choose the charity, the deadline, the type of activities and the quantity of activities.<br>" +
                         "Once the ChariFit community has logged all the activities you specified within the deadline you’ll be required to make the payment.<br><br>" +
                         "Payments are handled entirely by JustGiving so you might want us to <a href='" + RegisterJustGiving + "'>create a JustGiving account for you </a> if you don't have one already.",

                ID = "5"
            };

            var FAQ6 = new FaqVM
            {
                Question = "Why can’t i find any pledges that are suitable for me to log my activities against ?",
                Answer = "It’s early days for us, there may not always be many pledges to choose from. We need to spread the word, so if you can please like and share us on social media.",
                ShowSocial = true,
                ID = "6"
            };

            var FAQ7 = new FaqVM
            {
                Question = "I represent a company, why should i set up a corporate account ?",
                Answer = "Corporate accounts are a great way to have a relationship with the charities you care about and to motivate your employees or other communities to live active lives.<br><br>" +
                            "It costs nothing to setup and run an account and it can provide fantastic social media exposure.<br><br>" +
                            "Company accounts are also a great way to execute on your corporate social responsibility goals.",
                ID = "7"
            };

            var FAQ8 = new FaqVM
            {
                Question = "I represent a Charity. Why are we already on ChariFit ? ",
                Answer = "You’re not really \"on\" ChariFit, your charity details are visible here because you are registered on JustGiving and we display the information from there through JustGiving’s API’s.<br><br>" +
                        "We hope to funnel additional fund raising to you without taking anything away financially, but if you’d still rather not appear on ChariFit or would like a direct relationship with us, please get in contact and let us know.",
                ID = "8"
            };

            var FAQ9 = new FaqVM
            {
                Question = "I represent a Charity, how do i get listed on ChariFit ?",
                Answer = "<a href='http://pages.justgiving.com/causes.html'>Head on over to JustGiving and register with them</a> <br><br>" +
                "You’ll then appear on ChariFit at: https://Charifit.com/Charity/YOUR_CHARITIES_NAME <br><br>" +
                "We would like to build direct relationship with Charities, if your interested please get in contact.",
                ID = "9"
            };

            var FAQ10 = new FaqVM
            {
                Question = "How much wood would a woodchuck chuck if a woodchuck could chuck wood?",
                Answer = "A woodchuck would chuck as much wood as a woodchuck could chuck if a woodchuck could chuck wood.",
                ID = "10"
            };

            var FAQ11 = new FaqVM
            {
                Question = "What is the most frequently asked frequently asked question?",
                Answer = "The most frequently asked frequently asked question is “What is the most frequently asked frequently asked question?",
                ID = "11"
            };

            var FAQ12 = new FaqVM
            {
                Question = "What is the purpose of a Team ?",
                Answer = "Creators of pledges can choose to limit their pledges to members of a certain team or teams, such as a pledge from a company being limited to the employees team.<br><br>" +
                            "Anyone can create a team and as administrator you choose if the team is public such that anyone can join or private such that you must approve new team members.",
                ID = "12"
            };

            return new FAQSVM { FAQ1, FAQ41,FAQ2, FAQ3,  FAQ12, FAQ6, FAQ7, FAQ8, FAQ9, FAQ10, FAQ11 };
            
        }

    }
}