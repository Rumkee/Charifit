using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calorie.Models;
using Calorie.Models.Charities;
using Calorie.Models.Pledges;
using Calorie.Models.Teams;

namespace Calorie.BusinessLogic
{
    public class UserLogic
    {

        public class UserStats
        {

            public decimal? calTotal { get; set; }
            public decimal? HoursTotal { get; set; }
            public decimal? KmetersTotal { get; set; }
            public decimal? MetersTotal { get; set; }
            public decimal? MilesTotal { get; set; }
            public decimal? MinutesTotal { get; set; }
            public decimal? SessionsTotal { get; set; }
            public decimal PledgedTotal { get; set; }
            
        }
  

        public static ChartLogic.chartData getChartDataForCharities(ApplicationUser user)
        {
            
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var CharityTotals = user.Contributions.GroupBy(c => c.Pledge.Charity?.Name).Select(g => new {g.First().Pledge.Charity?.Name, Total = CurrencyLogic.ToBase(g.ToList())}).OrderByDescending(a => a.Total).ToList();
            
            foreach (var T in CharityTotals)
            {
                var Amt = T.Total.ToString("0.00");
                Labels.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Legends.Add(new ChartLogic.chartData.chartDataItem(T.Name));
            }

            return new ChartLogic.chartData(Legends, Series, Labels, "Charities", true);

        }

        public static async Task<bool> JoinTeamRequestApproved(ApplicationUser user, Team team,GenericEmailViewModel EmailVM,string teamURL)
        {

            Messaging.Add(Message.LevelEnum.alert_success, string.Format("Your membership of team '{0}' has been approved.", team.Name), Message.TypeEnum.StickyAlert, user);

            if (!user.RecieveFirstPartyEmails)
                return true;

            EmailVM.Name = user.UserName;
            EmailVM.HTMLContent = "<p>Congratulations! <br/>" + string.Format("<p>Your request to join team '{0}' has been approved by the Administrator",team.Name);
            EmailVM.PlaintextMessage = EmailVM.HTMLContent.Replace("<br/>", Environment.NewLine);
            EmailVM.Buttons.Add(new GenericEmailButtonViewModel {Text = "Go To Team",URL = teamURL});
   
            return await Messaging.SendEmail("Membership Approved", EmailVM.PlaintextMessage, Utilities.RenderGenericEmalHTML(EmailVM), user.Email);
            

        }
        private static bool checkAddUserToTeam(int? TeamID, ApplicationUser User,ApplicationDbContext db)
        {
            if (!TeamID.HasValue)
                return false;

            if (User.TeamID.HasValue)
                return false;


            var TID = TeamID.Value;

            var Team = db.Teams.Find(TID);

            if (Team == null)
                return false;

            if (Team.Availability == Team.Access.Private)
            {
                var Req = new TeamJoinRequests { TeamID = TID, UserID = User.Id };
                db.TeamJoinRequests.Add(Req);
                Messaging.Add(Message.LevelEnum.alert_warning, string.Format("A request to join the private team '{0}' has been made. The administrator will review the request shortly.", Team.Name), Message.TypeEnum.StickyAlert, User);
            }
            else
            {
                User.TeamID = TID;
                Messaging.Add(Message.LevelEnum.alert_success, string.Format("You have successfully joined team '{0}'", Team.Name), Message.TypeEnum.StickyAlert, User);
            }

            return true;
            
        }

        public static bool MergeVewModelToUser(EditViewModel EditVM, ApplicationUser User, ApplicationDbContext db)
        {

            var changed = false;

            User.UserName = SetProperty(User.UserName, EditVM.User.UserName, ref changed);
            User.ProfilePictureID = SetProperty(User.ProfilePictureID, EditVM.User.ProfilePictureID, ref changed);
            User.RecieveFirstPartyEmails = SetProperty(User.RecieveFirstPartyEmails, EditVM.User.RecieveFirstPartyEmails, ref changed);
            User.RecieveThirdPartyEmails = SetProperty(User.RecieveThirdPartyEmails, EditVM.User.RecieveThirdPartyEmails, ref changed);
            User.IsSponsor = SetProperty(User.IsSponsor, EditVM.User.IsSponsor, ref changed);
            User.IsExercisor = SetProperty(User.IsExercisor, EditVM.User.IsExercisor, ref changed);

            User.IsCompany = SetProperty(User.IsCompany, EditVM.User.IsCompany, ref changed);
            User.CompanyDescription = SetProperty(User.CompanyDescription, EditVM.User.CompanyDescription, ref changed);
            User.CompanyURL= SetProperty(User.CompanyURL, EditVM.User.CompanyURL, ref changed);

            User.Currency= SetProperty(User.Currency, EditVM.User.Currency, ref changed);
            if (checkAddUserToTeam(EditVM.User.TeamID, User,db))
                changed = true;

            
            
            if (EditVM.PreferredActivities?.Any() ?? false)
            {
                User.PreferredActivities?.Clear();
                changed = true;
                foreach (var pa in EditVM.PreferredActivities)
                {
                    User.PreferredActivities?.Add(new PreferredActivity
                    {
                        Activity = (PledgeActivity.ActivityTypes)Enum.Parse(typeof(PledgeActivity.ActivityTypes), pa)
                    });
                }

            }

            if (EditVM.PreferredjustGivingCharities?.Any() ?? false)
            {
                User.PreferredCharities?.Clear();
                changed = true;
                foreach (var pJustGivCharity in EditVM.PreferredjustGivingCharities)
                {
                    var c = JustGivingLogic.getOrCreateCharityRecordFromJustGivingCharityID(pJustGivCharity);
                    if (c != null)
                        User.PreferredCharities?.Add(new PreferredCharity {CharityID=c.ID});
                                        
                }

            }
            
            return changed;

        }

        private static T SetProperty<T>(T Original, T New, ref bool changedFlag)
        {
            if (Original==null && New == null)
            {
                return Original;
            }

            if ((Original == null && New != null) || New !=null && (New.GetHashCode() != Original.GetHashCode()) )
            {
                changedFlag = true;
                return New;
            }

            return Original;
        }

       

        public static async Task<bool> ConfigureNewUser(ApplicationUser User,GenericEmailViewModel EmailVM, string ConfirmEmailURL)
        {

            var Msg = "Thanks for singing up!" + Environment.NewLine + Environment.NewLine +
                "Please check your email for a link to confirm your email address." ;

            Messaging.Add(Message.LevelEnum.alert_success, Msg, Message.TypeEnum.StickyAlert, User);

            EmailVM.Name = User.UserName;
            EmailVM.HTMLContent ="Welcome to ChariFit, thanks for signing up.<br/><br/>Please confirm your email address by clicking below";
            EmailVM.PlaintextMessage = $"Welcome to ChariFit, thanks for signing up. Please confirm your email address by going to this address: {ConfirmEmailURL}";
            EmailVM.Buttons.Add(new GenericEmailButtonViewModel {Text = "Confirm Email Address",URL = ConfirmEmailURL });

            return await Messaging.SendEmail("Welcome To ChariFit", EmailVM.PlaintextMessage, Utilities.RenderGenericEmalHTML(EmailVM), User.Email);

        }

        public static async Task<bool> ConfigureNewJustGivingAccount(ApplicationUser User, GenericEmailViewModel JustGivingSignUpEmailVM, string Password)
        {

            JustGivingSignUpEmailVM.Name = User.UserName;
            JustGivingSignUpEmailVM.HTMLContent =
                "We have created a JustGiving Account for you with the following details:-<br/><br/>" +
                $"Email Address: {User.Email}<br/>" + $"Password: {Password}<br/><br/><br/>" +
                "Please note that ChariFit will not keep a record of your password." + "<br/>" +
                "We strongly suggest you change this password now.";

            JustGivingSignUpEmailVM.PlaintextMessage = JustGivingSignUpEmailVM.HTMLContent.Replace("<br/>",Environment.NewLine);

            JustGivingSignUpEmailVM.Buttons.Add(new GenericEmailButtonViewModel {Text = "Change Password", URL = "https://www.justgiving.com/account/your-details#update-your-password" });
            

            await Messaging.SendEmail("JustGiving Account Details", JustGivingSignUpEmailVM.PlaintextMessage, Utilities.RenderGenericEmalHTML(JustGivingSignUpEmailVM), User.Email);


            var Msg = "A JustGiving account has been created for you, check your email for details.";
            Messaging.Add(Message.LevelEnum.alert_success, Msg, Message.TypeEnum.StickyAlert, User);
            return true;
            
        }

        

        public static async Task<bool> ResetPassword( GenericEmailViewModel ResetPasswordVM,ApplicationUser user,string CallbackURL)
        {

            ResetPasswordVM.Name = user.UserName;
            ResetPasswordVM.HTMLContent = "A password reset has been requested on your account";
            ResetPasswordVM.PlaintextMessage = ResetPasswordVM.HTMLContent;
            ResetPasswordVM.Buttons.Add(new GenericEmailButtonViewModel {Text = "Reset Password",URL = CallbackURL});


            return await Messaging.SendEmail("Account Password Reset", ResetPasswordVM.PlaintextMessage, Utilities.RenderGenericEmalHTML(ResetPasswordVM), user.Email);
            
        }

        public static int getStarsForPledgedTotal(decimal amount)
        {
            if (amount == 0)
            {
                return 0;
            }
            if (amount < 10)
            {
                return 1;
            }
            if (amount < 100)
            {
                return 2;
            }
            if (amount < 200)
            {
                return 3;
            }
            if (amount < 300)
            {
                return 4;
            }

            return 5;
        }

        public static string GetStarsHTMLForPledgedTotal(decimal amount)
        {
            return Utilities.StarsHTML(getStarsForPledgedTotal(amount));
        }


        public static string GetStarsHTMLForActivity(PledgeActivity.ActivityUnits units, decimal quantity )
        {

            return Utilities.StarsHTML(GetStarsForActivity(units, quantity));
            

        }

        public static int GetStarsForActivity(PledgeActivity.ActivityUnits units, decimal quantity)
        {

            switch (units)
            {

                case PledgeActivity.ActivityUnits.Calories:
                    if (quantity == 0)
                            return 0;
                    
                    if (quantity < 200)
                            return 1;
                    
                    if (quantity < 400)
                            return 2;
                    
                    if (quantity < 600)
                            return 3;
                    
                    if (quantity < 800)
                            return 4;
                    
                    
                        return 5;
                    

                case PledgeActivity.ActivityUnits.Hours:
                    if (quantity == 0)
                        return 0;
                    if (quantity < 5)
                        return 1;
                    if (quantity < 10)
                        return 2;
                    if (quantity < 15)
                        return 3;
                    if (quantity < 20)
                        return 4;

                    return 5;

                case PledgeActivity.ActivityUnits.Kilometers:
                    if (quantity == 0)
                        return 0;
                    if (quantity < 5)
                        return 1;
                    if (quantity < 10)
                        return 2;
                    if (quantity < 15)
                        return 3;
                    if (quantity < 20)
                        return 4;

                    return 5;
                case PledgeActivity.ActivityUnits.Miles:
                    if (quantity == 0)
                        return 0;
                    if (quantity < 5)
                        return 1;
                    if (quantity < 10)
                        return 2;
                    if (quantity < 15)
                        return 3;
                    if (quantity < 20)
                        return 4;

                    return 5;
                case PledgeActivity.ActivityUnits.Sessions:
                    if (quantity == 0)
                        return 0;
                    if (quantity < 1)
                        return 1;
                    if (quantity < 2)
                        return 2;
                    if (quantity < 3)
                        return 3;
                    if (quantity < 4)
                        return 4;

                    return 5;
            }

            return 0;

        }

        public static string getStarsHTMLForUser(ApplicationUser user)
        {
            if (user.IsSuperAdmin)
            {
                return Utilities.StarsHTMLGold(GetStarsForUser(user));
            }
            return Utilities.StarsHTML(GetStarsForUser(user));
        }

        public static UserStats getStatsForUser(ApplicationUser user)
        {

            var stats = new UserStats();

            var groupedActivities = user.Offsetters.GroupBy(o => o.Pledge.Activity_Units);
            stats.calTotal= groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Calories)?.Sum(a => a.OffsetAmount);
            stats.HoursTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Hours)?.Sum(a => a.OffsetAmount);
            stats.KmetersTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Kilometers)?.Sum(a => a.OffsetAmount);
            stats.MetersTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Meters)?.Sum(a => a.OffsetAmount);
            stats.MilesTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Miles)?.Sum(a => a.OffsetAmount);
            stats.MinutesTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Minutes)?.Sum(a => a.OffsetAmount);
            stats.SessionsTotal = groupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Sessions)?.Sum(a => a.OffsetAmount);

            stats.MetersTotal = stats.MetersTotal.HasValue ? stats.MetersTotal : 0;
            stats.KmetersTotal = stats.KmetersTotal.HasValue ? stats.KmetersTotal : 0;
            stats.MinutesTotal = stats.MinutesTotal.HasValue ? stats.MinutesTotal : 0;
            stats.MilesTotal = stats.MilesTotal.HasValue ? stats.MilesTotal : 0;
            stats.HoursTotal = stats.HoursTotal.HasValue ? stats.HoursTotal : 0;

            stats.KmetersTotal += (stats.MetersTotal / 1000.00m);
            stats.MilesTotal += (stats.KmetersTotal * 0.621m);
            stats.HoursTotal += (stats.MinutesTotal / 60m);
            stats.PledgedTotal = CurrencyLogic.ToBase(user.Contributions);
            return stats;
        }
   

        public static int GetStarsForUser(ApplicationUser user)
        {
            var stats = getStatsForUser(user);

            var a = GetStarsForActivity(PledgeActivity.ActivityUnits.Calories, stats.calTotal ?? 0);
            var b = GetStarsForActivity(PledgeActivity.ActivityUnits.Hours, stats.HoursTotal ?? 0);
            var c = GetStarsForActivity(PledgeActivity.ActivityUnits.Kilometers, stats.KmetersTotal ?? 0);
            var d = GetStarsForActivity(PledgeActivity.ActivityUnits.Sessions, stats.SessionsTotal?? 0);
            var e = getStarsForPledgedTotal(stats.PledgedTotal);
            
            int[] List = {a, b, c, d, e};
            return List.Max();
         
        }

        
    }
}