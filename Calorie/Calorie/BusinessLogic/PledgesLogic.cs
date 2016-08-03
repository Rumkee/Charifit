using System;
using System.Collections.Generic;
using System.Linq;
using Calorie.Models;
using Calorie.Models.Pledges;
using Calorie.Models.Teams;

namespace Calorie.BusinessLogic
{
    public static class PledgesLogic
    {

        public enum PledgeStatus
        {
            None,
            Open,
            Canceled,
            Completed,
            Expired
        }

        public static void EditPledge(EditPledgeVM VM, Pledge pledge, ApplicationUser user,ApplicationDbContext db){

            if (VM.Activities != null)
            {
            foreach(var newA in VM.Activities)
            {
                var thisNewType = (PledgeActivity.ActivityTypes)Enum.Parse(typeof(PledgeActivity.ActivityTypes), newA);
                if (!pledge.Activity_Types.Exists(at => at.Activity == thisNewType)){
                    pledge.Activity_Types.Add(new PledgeActivity { Activity=thisNewType});
                }
            }
            }

            if (VM.GalleryIDs != null)
            {

            //new images
            foreach (var newG in VM.GalleryIDs)
            {
                var newGInt = GenericLogic.GetInt(newG);
                if (!pledge.Gallery.Exists(g => g.CalorieImageID  == newGInt))
                {                    
                    pledge.Gallery.Add(db.Images.FirstOrDefault(t => t.CalorieImageID == newGInt));
                }
            }
            
            //deleted images
            foreach (var old in pledge.Gallery.ToList())
            {
                if (!VM.GalleryIDs.Exists(g => GenericLogic.GetInt(g) == old.CalorieImageID)){
                    pledge.Gallery.Remove(old);
                }
            }
            }
            pledge.ExpiryDate = VM.ExpiryDate;
            pledge.Story = VM.Story;

            Messaging.Add(Message.LevelEnum.alert_success, "Your Pledge has been updated.", Message.TypeEnum.StickyAlert, user);


        }

        public static ChartLogic.chartData getProgressChartData(Pledge P)
        {        
            var remaining = P.Activity_Amount - P.TotalOffsetAmount;
            var lbl = getPledgeProgressDescription(P.TotalOffsetPercent, true);
            var labels = new List<ChartLogic.chartData.chartDataItem> {new ChartLogic.chartData.chartDataItem(lbl)};

            var seriesA = new ChartLogic.chartData.chartDataItem(P.TotalOffsetAmount.ToString());
            var seriesB = new ChartLogic.chartData.chartDataItem("{ className: 'ct-series ct-series-remainder',value: " + remaining + "}",
                                                                                 ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder, remaining);
            var series = new List<ChartLogic.chartData.chartDataItem>();
            series.Add(seriesA);
            series.Add(seriesB);

            return new ChartLogic.chartData(null, series, labels);
        }

        public static ChartLogic.chartData getMiniProgressChartData(Pledge P)
        {
            var remaining = P.Activity_Amount - P.TotalOffsetAmount;
            var lbl = getPledgeProgressDescription(P.TotalOffsetPercent, true);
            var labels = new List<ChartLogic.chartData.chartDataItem> {new ChartLogic.chartData.chartDataItem(lbl)};

            var seriesA = new ChartLogic.chartData.chartDataItem("{ className: 'mini-donut ct-series  ct-series-a',value: " + P.TotalOffsetAmount + "}",_value:P.TotalOffsetAmount);
            var seriesB = new ChartLogic.chartData.chartDataItem("{ className: 'mini-donut ct-series ct-series-remainder',value: " + remaining + "}",
                                                                                 ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder, remaining);
            var series = new List<ChartLogic.chartData.chartDataItem> {seriesA, seriesB};

            return new ChartLogic.chartData(null, series, labels);
        }
        
        public static bool getCompletePledgeFromCreatePledgeVM(CreatePledgeVM pledgeVM, ApplicationDbContext db, ApplicationUser user)
        {

            pledgeVM.Pledge.Contributors[0].IsOriginator = true;
            pledgeVM.Pledge.Contributors[0].Currency = user.Currency;

            if (pledgeVM.Pledge.Teams == null)
                pledgeVM.Pledge.Teams = new List<Team>();
            

            if (pledgeVM.TeamIDs != null)
            {
                foreach (var tId in pledgeVM.TeamIDs)
                {
                    int tIdInt = 0;
                    int.TryParse(tId, out tIdInt);
                    pledgeVM.Pledge.Teams.Add(db.Teams.FirstOrDefault(t => t.ID == tIdInt));
                }
            }

            if (pledgeVM.GalleryIDs != null)
            {
                foreach (var gIdStr in pledgeVM.GalleryIDs)
                {
                    var gID = GenericLogic.GetInt(gIdStr);
                    pledgeVM.Pledge.Gallery.Add(db.Images.FirstOrDefault(t => t.CalorieImageID == gID));
                }
            }

            if (pledgeVM.ActivityIDs != null)
            {
                if (pledgeVM.Pledge.Activity_Types == null)
                        pledgeVM.Pledge.Activity_Types = new List<PledgeActivity>();
                
                foreach (var ActivityID in pledgeVM.ActivityIDs)
                {
                    pledgeVM.Pledge.Activity_Types.Add(new PledgeActivity { Activity = (PledgeActivity.ActivityTypes)Enum.Parse(typeof(PledgeActivity.ActivityTypes), ActivityID) });
                }
            }

           if (!string.IsNullOrEmpty(pledgeVM.JustGivingCharityID))
            {
                var SelectedCharity = JustGivingLogic.getOrCreateCharityRecordFromJustGivingCharityID(pledgeVM.JustGivingCharityID);
                if (SelectedCharity == null)
                        return false;
                
                pledgeVM.Pledge.CharityID = SelectedCharity.ID;
            }

            return true;

        }

        public static string getPledgeProgressDescription(Pledge p) => getPledgeProgressDescription(p.TotalOffsetPercent, false);

        public static string getPledgeProgressDescription(int percent, bool includeBreak)
        {

            var breakChar = " - ";
            if (includeBreak)
            {
                breakChar = "<BR>";
            }
            if (percent == 0)
            {
                return "0% " + breakChar + " Not Started !";
            }
            if (percent < 25)
            {
                return $"{percent}%{breakChar}Just Started";
            }
            if (percent < 50)
            {
                return $"{percent}%{breakChar}";
            }
            if (percent < 75)
            {
                return $"{percent}%{breakChar}Nearly Done !";
            }
            if (percent < 99)
            {
                return $"{percent}%{breakChar}Nearly Done !";
            }
            return $"100%{breakChar}Finished!";
        }

        public static ChartLogic.chartData getUserOffsetList(Pledge P)
        {

            var Amounts = new List<ChartLogic.chartData.chartDataItem>();
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Ledgends = new List<ChartLogic.chartData.chartDataItem>();

            var GroupedOffsets = P.Offsets.GroupBy(off => off.Offsetter).ToList();

            foreach (var go in GroupedOffsets)
            {
                var sum = go.Sum(a => a.OffsetAmount);
                Amounts.Add(new ChartLogic.chartData.chartDataItem(sum.ToString("0.00")));

                Labels.Add(new ChartLogic.chartData.chartDataItem(sum.ToString("0.00")));
                Ledgends.Add(new ChartLogic.chartData.chartDataItem(go.Key.UserName ));
            }

            //add reminder....
            var diff = P.Activity_Amount - P.TotalOffsetAmount;

            Amounts.Add(new ChartLogic.chartData.chartDataItem("{className: \'ct-series ct-series-remainder\'," + "value: " + diff +   "}",
                                                                ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder,diff));

            Labels.Add(new ChartLogic.chartData.chartDataItem(diff.ToString(),ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder, diff));
            Ledgends.Add(new ChartLogic.chartData.chartDataItem("Remaining",ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder));
            
            return new ChartLogic.chartData(Ledgends,Amounts, Labels, GenericLogic.HTML.USER_HTML + "&nbsp;Users");


        }

        public static ChartLogic.chartData getTeamsOffsetList(Pledge P)
        {

            var Amounts = new List<ChartLogic.chartData.chartDataItem>();
            var Labels = new List<ChartLogic.chartData.chartDataItem>();
            var Ledgends = new List<ChartLogic.chartData.chartDataItem>();

            var GroupedOffsets = P.Offsets.GroupBy(off => off.Offsetter.Team).ToList();

            foreach (var go in GroupedOffsets)
            {
                var TeamSum = go.Sum(o => o.OffsetAmount);
                Amounts.Add(new ChartLogic.chartData.chartDataItem(TeamSum.ToString("0.00")));
                if (go.Key != null)
                {
                    Labels.Add(new ChartLogic.chartData.chartDataItem(TeamSum.ToString("0.00")));
                    Ledgends.Add(new ChartLogic.chartData.chartDataItem(go.Key.Name ));
                }
                else
                {
                    Labels.Add(new ChartLogic.chartData.chartDataItem(TeamSum.ToString("0.00")));
                    Ledgends.Add(new ChartLogic.chartData.chartDataItem("Unaffiliated"));
                }

            }

            //add reminder....
            var diff = P.Activity_Amount - P.TotalOffsetAmount;

            Amounts.Add(new ChartLogic.chartData.chartDataItem("{className: \'ct-series ct-series-remainder\'," + "value: " + diff.ToString("0.00") +"}"
                                                        ,ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder, diff));

            Labels.Add(new ChartLogic.chartData.chartDataItem(diff.ToString("0.00"),ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder,diff));
            Ledgends .Add(new ChartLogic.chartData.chartDataItem("Remaining",ChartLogic.chartData.chartDataItem.chartDataItemFlags.IsRemainder));
            
            return new ChartLogic.chartData(Ledgends, Amounts, Labels, GenericLogic.HTML.TEAM_HTML + "&nbsp;Teams");        

        }
        
        public static ChartLogic.chartData getBurndownChartData(Pledge P)
        {

            var labels = new List<ChartLogic.chartData.chartDataItem >();
            var target = new List<ChartLogic.chartData.chartDataItem>();
            var actual = new List<ChartLogic.chartData.chartDataItem>();

            //calculate burn down rate per day
            var timespanDiff = P.ExpiryDate - P.CreatedUTC;
            var perDay = P.Activity_Amount / timespanDiff.Days;
            var DayAmount = P.Activity_Amount;
            var DayActual = P.Activity_Amount;
            var stepRate = timespanDiff.Days/ 10;

            for (var dt = P.CreatedUTC; dt < P.ExpiryDate; dt = dt.AddDays(stepRate))
            {
                DayActual -= P.Offsets.Where(o => o.CreatedUTC>dt & o.CreatedUTC < dt.AddDays(stepRate)).Sum(o => o.OffsetAmount);
                
                labels.Add(new ChartLogic.chartData.chartDataItem (dt.ToString("dd MMMM yyyy")));                
                target.Add(new ChartLogic.chartData.chartDataItem (DayAmount.ToString("0.00")));
                DayAmount -= perDay * stepRate;

                if (dt < DateTime.UtcNow)
                    actual.Add(new ChartLogic.chartData.chartDataItem (DayActual.ToString("0.00")));
                
            }

            var series = new List<List<ChartLogic.chartData.chartDataItem>> {target, actual};

            var Legend = new List<ChartLogic.chartData.chartDataItem>
            {
                new ChartLogic.chartData.chartDataItem("Target"),
                new ChartLogic.chartData.chartDataItem("Actual")
            }; // (new string[] { "Target", "Actual" });

            return new ChartLogic.chartData(Legend,series, labels,"Burn Down");
        }

        public static PledgeStatus GetPledgeStatus(Pledge P)
        {
            if (P.Closed){
                return PledgeStatus.Canceled;
            }
            if (P.ExpiryDate < DateTime.UtcNow){
                return PledgeStatus.Expired;
            }
            if(P.TotalOffsetPercent == 100){
                return PledgeStatus.Completed;
            }
            return PledgeStatus.Open;
        }

        public static string GetPledgeStatusHTML(Pledge p)
        {
            switch (GetPledgeStatus(p)){
                case PledgeStatus.Canceled:
                    return "<text style='color:red; font-size:small; font-weight:normal'><i class='fa fa-ban'></i>&nbsp;Canceled</text>";

                case PledgeStatus.Completed:
                    return "<text style='color:#18bc9c;font-size:x-small;font-weight:normal'><span class='glyphicon glyphicon-ok'></span>&nbsp;Completed</text>";

                case PledgeStatus.Expired:
                    return "<text style='color:red; font-size:small; font-weight:normal'><i class='fa fa-ban'></i>&nbsp;Expired</text>";

                case PledgeStatus.Open:
                    return "<text style='color:#18bc9c; font-size:x-small;font-weight:normal'><span class='glyphicon glyphicon-ok'></span>&nbsp;Open</text>";
                
            }

            return string.Empty;
        }



    }

}