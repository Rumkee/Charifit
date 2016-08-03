using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using Calorie.Models;
using Calorie.Models.Charities;
using Calorie.Models.Pledges;
using Calorie.Models.Social;
using Calorie.Models.Teams;

namespace Calorie.BusinessLogic.Social
{
    public class OpenGraph
    {
        public static OpenGraphVM GetOpenGraphVMForSite(HttpRequestBase Request,UrlHelper Url)
        {           
            return new OpenGraphVM()
            {
                type = "website",
                title = "Help Yourself, Helping Others",
                description = "Use your fitness tracking data to help fulfill charitable pledges",
                image = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~/Images/Photos/FB_SiteImage1.jpg")}"
            };
        }

        public static OpenGraphVM GetOpenGraphVMForPledge(Pledge pledge, HttpRequestBase Request, UrlHelper Url)
        {

            var amt = CurrencyLogic.ToCurrency(pledge.Contributors, pledge.Originator.Currency).ToString("0.00");
            var currencyPrefix = CurrencyLogic.GetCurrencyPrefix(pledge.Originator.Currency);
            
            return new OpenGraphVM()
            {
                type = "article",
                title = "Help Yourself, Helping Others",
                description = $"{currencyPrefix}{amt} Pledged to {pledge.Charity.Name}",
                image = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~/Images/Photos/FB_SiteImage1.jpg")}"
            };
        }

        public static OpenGraphVM GetOpenGraphVMForTeam(Team t, HttpRequestBase Request, UrlHelper Url)
        {

            var ImageURL = Url.Action("GetImage", "Image", new { ImageID = t.ImageID}, protocol: Request.Url.Scheme);
            
            return new OpenGraphVM()
            {
                type = "article",
                title = $"{t.Name} on ChariFit",
                description = $"{t.Name} {t.Description}",
                image = ImageURL
            };
        }

        public static OpenGraphVM GetOpenGraphVMForCharity(Charity C, HttpRequestBase Request, UrlHelper Url)
        {
            return new OpenGraphVM()
            {
                type = "article",
                title = $"{C.Name} on ChariFit",
                description = C.Description,
                image = C.JustGivingCharityImageURL
            };
        }

        public static OpenGraphVM GetOpenGraphVMForCorporate(ApplicationUser U, HttpRequestBase Request, UrlHelper Url)
        {

            string ImageURL ;

            if (U.ProfilePictureID.HasValue)
                ImageURL = Url.Action("GetImage", "Image", new { ImageID = U.ProfilePictureID.Value}, protocol: Request.Url.Scheme);
            else
                ImageURL = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~/Images/Photos/FB_SiteImage1.jpg")}";
            
            

            return new OpenGraphVM()
            {
                type = "article",
                title = $"{U.UserName} on ChariFit",
                description = U.CompanyDescription,
                image = ImageURL
            };
        }

    }
}