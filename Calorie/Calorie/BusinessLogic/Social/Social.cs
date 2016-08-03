using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calorie.Models;
using Calorie.Models.Charities;
using Calorie.Models.Pledges;
using Calorie.Models.Social;
using Calorie.Models.Teams;

namespace Calorie.BusinessLogic.Social
{
    public class Social
    {
        
        public static SocialVM GetSocialVMForSite(HttpRequestBase Request)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.Site,
                LinkID = "1",
                ShareURL = Request.Url.AbsoluteUri
            };

        }

        public static SocialVM GetSocialVMForPledge(Pledge pledge, HttpRequestBase Request, UrlHelper Url)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.Pledge,
                LinkID = pledge.PledgeID.ToString(),
                ShareURL = Url.Action("Details", "Pledges", new {id = pledge.PledgeID}, protocol: Request.Url.Scheme),
                Blurb = $"{pledge.TotalOffsetPercent.ToString("0")}% Complete and {(pledge.ExpiryDate - DateTime.UtcNow).TotalDays.ToString("0")} days remaining to fulfill pledge to {pledge.Charity?.Name}"
            };

        }

        public static SocialVM GetSocialVMForPledgeContribution(PledgeContributors PC, string PCIdent, HttpRequestBase Request, UrlHelper Url)
        {
            string blurb;

            if(PC.AmountAnonymous) 
                    blurb = $"{PC.Sinner.UserName} made a pledge to {PC.Pledge.Charity.Name}";
            else 
                blurb = $"{PC.Sinner.UserName} pledged {CurrencyLogic.GetCurrencyPrefix(PC.Currency)}{PC.Amount} to {PC.Pledge.Charity.Name}";
            
            return new SocialVM()
            {
                Type = SocialVM.SocialType.PledgeContribution,
                LinkID = PC.ID.ToString(),
                ShareURL = Url.Action("Details", "Pledges", new { id = PC.Pledge.PledgeID }, protocol: Request.Url.Scheme) + "#" + PCIdent,
                Blurb = blurb
            };

        }

        public static SocialVM GetSocialVMForPledgeOffSet(Offset offset, string OffsetIdent, HttpRequestBase Request, UrlHelper Url)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.OffSet,
                LinkID = offset.ID.ToString(),
                ShareURL = Url.Action("Details", "Pledges", new {id = offset.Pledge.PledgeID}, protocol: Request.Url.Scheme) +"#" + OffsetIdent,
                Blurb = $"{offset.Offsetter.UserName} logged {offset.OffsetAmount} {offset.Pledge.Activity_Units} to help fulfill a pledge to {offset.Pledge.Charity.Name}"
            };

        }

        public static SocialVM GetSocialVMForCharity(Charity C,  HttpRequestBase Request, UrlHelper Url)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.Charity,
                LinkID = C.ID.ToString(),
                ShareURL = Url.Action("Details", "Charities", new { charityname = C.Name.Replace(" ", "") },protocol: Request.Url.Scheme),
                Blurb = ""
            };

        }

        public static SocialVM GetSocialVMForTeam(Team T, HttpRequestBase Request, UrlHelper Url)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.Team ,
                LinkID = T.ID.ToString(),
                ShareURL = Url.Action("Details", "Teams", new { teamname = T.Name.Replace(" ", "") }, protocol: Request.Url.Scheme),
                Blurb = ""
            };

        }

        public static SocialVM GetSocialVMForCorporate(ApplicationUser U, HttpRequestBase Request, UrlHelper Url)
        {
            return new SocialVM()
            {
                Type = SocialVM.SocialType.Corporate,
                LinkID = U.Id,
                ShareURL = Url.Action("Details", "Company", new { companyname = U.UserName.Replace(" ", "") }, protocol: Request.Url.Scheme),
                Blurb = ""
            };

        }


    }
}