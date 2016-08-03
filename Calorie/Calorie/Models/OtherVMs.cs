using System.Collections.Generic;
using static Calorie.BusinessLogic.ChartLogic;

namespace Calorie.Models.Misc
{

    public class HelpAlert
    {
        public string ID { get; set; }
        public string Message { get; set; }
    }
    public class SideBarVM
    {

        public List<Teams.Team> Teams { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Pledges.Pledge> Pledges { get; set; }

    }

    public class CharityPartialVM
    {
        public string CharityName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DonutChartVM
    {

        public chartData  Data { get; set; }    
        public bool centerLabel { get; set; }
        public int Size { get; set; }
        public bool ShowLabels { get; set; }
        public bool Animate { get; set; }


        public DonutChartVM(chartData _data, bool _centerLabel)
        {
            Data = _data;           
        
            centerLabel = _centerLabel;
            Animate = true;
        }

        public DonutChartVM() {}

    }

    public class LineChartVM
    {

        public chartData Data { get; set; }
        public bool Animate { get; set; }

      
    }

    public class BarChartVM
    {

        public chartData Data { get; set; }
        public bool Animate { get; set; }

    }    

    public class FilterVM
    {
        public string Callback { get; set; }
        public List<string> Options { get; set; }
    }

    public class FaqVM
    {
        public string ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public bool ShowSocial { get; set; }
    }

    public class FAQSVM : List<FaqVM>
    {        
    }
}