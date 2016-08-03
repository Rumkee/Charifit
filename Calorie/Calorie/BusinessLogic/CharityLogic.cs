using System.Collections.Generic;
using System.Linq;

namespace Calorie.BusinessLogic
{
    public class CharityLogic
    {

        public static List<Models.Charities.Charity> GetTopCharities()
        {

            var db = new Models.ApplicationDbContext();
            return db.Charities.OrderByDescending(c => c.Pledges.Count()).Take(5).ToList();

        }

        public static List<Models.Charities.Charity> GetTopCharitiesByAmountRaised()
        {
            var db = new Models.ApplicationDbContext();
            return db.Charities.ToList().OrderByDescending(c => c.Pledges.ToList().Sum(p => p.Contributors.ToList().Sum(con => CurrencyLogic.ToBase(con.Currency, con.Amount)))).Take(5).ToList();
                        
        }

        public static ChartLogic.chartData getUserPledgeContributionsForCharity(Models.Charities.Charity C)
        {

            var Amounts = new List<ChartLogic.chartData.chartDataItem>();
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Ledgends = new List<ChartLogic.chartData.chartDataItem>();

            var db = new Models.ApplicationDbContext();
            var AllCharityPledgeContributions = db.PledgeContributors.Where(PC => PC.Pledge.Charity.ID == C.ID ).ToList();
            var GroupedPledgeContributions = AllCharityPledgeContributions.GroupBy(pc => pc.Sinner).Select(g => new { g,Sum = CurrencyLogic.ToBase(g.ToList()) }).ToList();

            foreach (var gPC in GroupedPledgeContributions.OrderByDescending(a => a.Sum))
            {                
                Amounts.Add(new ChartLogic.chartData.chartDataItem(gPC.Sum.ToString("0.00")));
                Labels.Add(new ChartLogic.chartData.chartDataItem(gPC.Sum.ToString("0.00")));
                Ledgends.Add(new ChartLogic.chartData.chartDataItem(gPC.g.Key.UserName));
            }

            return new ChartLogic.chartData(Ledgends, Amounts, Labels, GenericLogic.HTML.USER_HTML + "&nbsp;Users",true);
            
        }

        public static ChartLogic.chartData getTeamPledgeContributionsForCharity(Models.Charities.Charity C)
        {

            var Amounts = new List<ChartLogic.chartData.chartDataItem>();
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Ledgends = new List<ChartLogic.chartData.chartDataItem>();

            var db = new Models.ApplicationDbContext();
            var AllCharityPledgeContributions = db.PledgeContributors.Where(PC => PC.Pledge.Charity.ID == C.ID).ToList();
            var GroupedPledgeContributions = AllCharityPledgeContributions.GroupBy(pc => pc.Sinner.Team).Select(pc => new {pc, Sum=CurrencyLogic.ToBase(pc.ToList())});

            foreach (var gPC in GroupedPledgeContributions.OrderByDescending(a => a.Sum))
            {
                Amounts.Add(new ChartLogic.chartData.chartDataItem(gPC.Sum.ToString("0.00")));

                Labels.Add(new ChartLogic.chartData.chartDataItem(gPC.Sum.ToString("0.00")));
                Ledgends.Add(gPC.pc.Key == null
                    ? new ChartLogic.chartData.chartDataItem("Unaffiliated")
                    : new ChartLogic.chartData.chartDataItem(gPC.pc.Key.Name));
            }

            return new ChartLogic.chartData(Ledgends, Amounts, Labels, GenericLogic.HTML.TEAM_HTML+ "&nbsp;Teams", true);

        }


    }
}