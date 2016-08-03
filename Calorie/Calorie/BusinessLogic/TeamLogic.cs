using System;
using System.Collections.Generic;
using System.Linq;
using Calorie.Models;
using Calorie.Models.Pledges;
using Calorie.Models.Teams;

namespace Calorie.BusinessLogic.TeamLogic
{

    public class TeamActivityStats
    {
        public decimal Calories { get; set; }
        public decimal Hours { get; set; }
        public decimal Miles { get; set; }
        public decimal Sessions { get; set; }
        public decimal BaseCurrencySponsoredTotal { get; set; }
        public decimal BaseCurrencyRaisedTotal { get; set; }
    }
    public class TeamLogic
    {

        public static ChartLogic.chartData getChartDataForSponsorshipPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var UsersWithSponsorship = T.Members.Where(m => m.Contributions.Any());
            foreach(var user in UsersWithSponsorship)
            {
                var Amt = user.Contributions.Sum(c => CurrencyLogic.ToBase(c.Currency, c.Amount)).ToString("0.00");
                Labels.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
            }
                        
            return new ChartLogic.chartData(Legends, Series, Labels, GenericLogic.HTML.SPONSORED_HTML + "&nbsp;Sponsored",true);

        }

        public static ChartLogic.chartData getChartDataForRaisedPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var db = new ApplicationDbContext();
          
            var UsersWithOffsets = T.Members.Where(m => m.Offsetters.Any());
            foreach (var user in UsersWithOffsets)
            {

                var Amt = CurrencyLogic.ToBase(db.PledgeContributors.ToList().Where(c => c.Pledge.Offsets.Exists(o => o.Offsetter == user)).ToList()).ToString("0.00");
                
                Labels.Add( new ChartLogic.chartData.chartDataItem(Amt ));
                Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
            }

            return new ChartLogic.chartData(Legends,  Series, Labels, GenericLogic.HTML.RAISED_HTML + "&nbsp;Raised", true);

        }


        public static ChartLogic.chartData getChartDataForSessionsPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var UsersWithOffsets = T.Members.Where(m => m.Offsetters.Any());
            foreach (var user in UsersWithOffsets)
            {
                var Amt = user.Offsetters.Count().ToString();
                Labels.Add( new ChartLogic.chartData.chartDataItem(Amt));
                Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
            }

            return new ChartLogic.chartData(Legends, Series,Labels,  GenericLogic.HTML.SESSIONS_HTML + "&nbsp;Sessions");

        }


        public static ChartLogic.chartData getChartDataForHoursPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var UsersWithOffsets = T.Members.Where(m => m.Offsetters.Any());
            foreach (var user in UsersWithOffsets)
            {

                var HoursTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Hours).Sum(o => o.OffsetAmount);
                var MinutesTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Minutes).Sum(o => o.OffsetAmount);                               
                
                HoursTotal += (MinutesTotal / 60m);
                if (HoursTotal > 0)
                {
                    var Amt = HoursTotal.ToString("0.00");
                    Labels.Add(new ChartLogic.chartData.chartDataItem(Amt));
                    Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                    Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
                }

                
            }

            return new ChartLogic.chartData(Legends, Series,Labels,  GenericLogic.HTML.HOURS_HTML + "&nbsp;Hours");
        }

        public static ChartLogic.chartData getChartDataForMilesPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var UsersWithOffsets = T.Members.Where(m => m.Offsetters.Any());
            foreach (var user in UsersWithOffsets)
            {

                var MilesTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Miles).Sum(o => o.OffsetAmount);
                var MetersTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Meters).Sum(o => o.OffsetAmount);
                var KmetersTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Kilometers).Sum(o => o.OffsetAmount);                              
                
                KmetersTotal += (MetersTotal / 1000.00m);
                MilesTotal += (KmetersTotal * 0.621m);
                                
                if (MilesTotal > 0)
                {
                    var Amt = MilesTotal.ToString("0.00");
                    Labels.Add(new ChartLogic.chartData.chartDataItem(Amt));
                    Series.Add(new ChartLogic.chartData.chartDataItem(Amt));
                    Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
                }


            }

            return new ChartLogic.chartData(Legends, Series,Labels, GenericLogic.HTML.MILES_HTML + "&nbsp;Miles");
        }


        public static ChartLogic.chartData getChartDataForCaloriesPerUser(Team T)
        {
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Series = new List<ChartLogic.chartData.chartDataItem>();
            var Legends = new List<ChartLogic.chartData.chartDataItem>();

            var UsersWithOffsets = T.Members.Where(m => m.Offsetters.Any());
            foreach (var user in UsersWithOffsets)
            {

                var CalTotal = user.Offsetters.Where(o => o.Pledge.Activity_Units == PledgeActivity.ActivityUnits.Calories).Sum(o => o.OffsetAmount);
                                
                if (CalTotal > 0)
                {                    
                    Labels.Add(new ChartLogic.chartData.chartDataItem(CalTotal.ToString()));
                    Series.Add(new ChartLogic.chartData.chartDataItem(CalTotal.ToString()));
                    Legends.Add(new ChartLogic.chartData.chartDataItem(user.UserName));
                }


            }

            return new ChartLogic.chartData(Legends, Series,Labels, GenericLogic.HTML.CALORIE_HTML + "&nbsp;Calories");
        }



        public static int? getStarsForTeam(Team T)
        {
            var sumRating = T.Members.Sum(UserLogic.GetStarsForUser);
            int? TeamStarRating = null;
            if (T.Members.Any())
                    TeamStarRating= sumRating / T.Members.Count();
            
            return TeamStarRating;
        }
        
        public static string getStarsHTMLForTeam(Team T)
        {
            var r = getStarsForTeam(T);
            return r.HasValue ? Utilities.StarsHTML(r.Value) : string.Empty;
        }

        public static TeamActivityStats getActivityStatsForTeam(Team T)
        {
            var stats = new TeamActivityStats();

            var db = new ApplicationDbContext();

            var teamOffsets = db.Offsets.Where(o => o.Offsetter.Team.ID == T.ID).ToList();
            var teamGroupedActivities = teamOffsets.GroupBy(o => o.Pledge.Activity_Units).ToList();

            var calTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Calories)?.Sum(a => a.OffsetAmount);
            var HoursTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Hours)?.Sum(a => a.OffsetAmount);
            var MinutesTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Minutes)?.Sum(a => a.OffsetAmount);
            var KmetersTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Kilometers)?.Sum(a => a.OffsetAmount);
            var MetersTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Meters)?.Sum(a => a.OffsetAmount);
            var MilesTotal = teamGroupedActivities.FirstOrDefault(g => g.Key == PledgeActivity.ActivityUnits.Miles)?.Sum(a => a.OffsetAmount);
            var SessionsTotal = teamOffsets.Count();

            stats.BaseCurrencySponsoredTotal = Math.Round(CurrencyLogic.ToBase(db.PledgeContributors.Where(pc => pc.Sinner.Team.ID == T.ID).ToList()),2);
            stats.BaseCurrencyRaisedTotal = Math.Round(CurrencyLogic.ToBase(db.PledgeContributors.ToList().Where(pc => pc.Pledge.Offsets.Exists(o => o.Offsetter.Team?.ID == T.ID)).ToList()), 2);

            MetersTotal = MetersTotal.HasValue ? MetersTotal : 0;
            KmetersTotal = KmetersTotal.HasValue ? KmetersTotal : 0;
            MinutesTotal = MinutesTotal.HasValue ? MinutesTotal : 0;
            MilesTotal = MilesTotal.HasValue ? MilesTotal : 0;
            HoursTotal = HoursTotal.HasValue ? HoursTotal : 0;

            HoursTotal += (MinutesTotal / 60m);
            KmetersTotal += (MetersTotal / 1000.00m);
            MilesTotal += (KmetersTotal * 0.621m);
          

            stats.Calories = calTotal.HasValue ? Math.Round(calTotal.Value,2) : 0;
            stats.Hours= HoursTotal.HasValue ? Math.Round(HoursTotal.Value,2) : 0;
            stats.Miles= MilesTotal.HasValue ? Math.Round(MilesTotal.Value,2) : 0;
            stats.Sessions = SessionsTotal;
            
            return stats;
        }
    }
}