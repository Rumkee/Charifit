using System.Collections.Generic;
using System.Linq;

namespace Calorie.BusinessLogic
{

    public static class ChartLogic
    {

        public class chartData
        {

            //internal class
            public class chartDataItem
            {

                public chartDataItem(string _valueJsonObject,chartDataItemFlags _options = chartDataItemFlags.None, decimal _value = 0)
                {
                    ValueJsonObject = _valueJsonObject;
                    Flags = _options;
                    Value = _value;

                    if (Value == 0)
                    {
                        decimal a ;
                        decimal.TryParse(ValueJsonObject, out a);
                        Value = a;
                    }
                }
                public enum chartDataItemFlags
                {
                    None,
                    IsRemainder
                }
                public string ValueJsonObject { get; set; }
                public decimal Value { get; set; }

                public chartDataItemFlags Flags { get; set; }

                
            }

            //public props
            public List<chartDataItem> legendsRaw { get; set; }
            public List<List<chartDataItem>> seriesRaw { get; set; }
            public List<chartDataItem> labelsRaw { get; set; }
            public string Name { get; set; }
            public bool IsCurrency { get; set; }

            //constructors
            public chartData(List<chartDataItem> _legends, List<chartDataItem> _series, List<chartDataItem> _labels, string _name="",bool _isCurrency= false)
            {
                IsCurrency = _isCurrency;
                Name = _name;
                legendsRaw = _legends;
                seriesRaw = new List<List<chartDataItem>> {_series};

                labelsRaw = _labels;

            }

            public chartData(List<chartDataItem> _legends, List<List<chartDataItem>> _series, List<chartDataItem> _labels, string _name = "",bool _isCurrency = false)
            {
                IsCurrency = _isCurrency;
                Name = _name;
                legendsRaw = _legends;
                seriesRaw = _series;
                labelsRaw = _labels;
                
            }

            //public functions
            public string getLegend(bool includeRemaining)
            {
                if (legendsRaw == null)
                {
                    return string.Empty;
                }
                var o = new List<string>();
                foreach (var itm in legendsRaw)
                {
                    if ((itm.Flags ==chartDataItem.chartDataItemFlags.IsRemainder && includeRemaining) | (itm.Flags ==chartDataItem.chartDataItemFlags.None) )
                            o.Add(itm.ValueJsonObject);
                    
                }

                return ChartLogic.GetArrayString(o, true);
            
            }
            public string getSeries(bool includeRemaining)
            {
                var a = new List<string>();

                foreach(var List in seriesRaw)
                {
                    
                var o = new List<string>();
                foreach (var itm in List)
                {
                    if ((itm.Flags == chartDataItem.chartDataItemFlags.IsRemainder && includeRemaining) | (itm.Flags == chartDataItem.chartDataItemFlags.None))
                            o.Add(itm.ValueJsonObject);
                    
                }

                    a.Add(ChartLogic.GetArrayString(o, false));
                
                }
                return a.Count() > 1 ? ChartLogic.stringifySeries(a) : a.ElementAt(0);
                

            }
            public string getLabels(bool includeRemaining)
            {
                var o = new List<string>();
                foreach (var itm in labelsRaw)
                {
                    if ((itm.Flags == chartDataItem.chartDataItemFlags.IsRemainder && includeRemaining) | (itm.Flags == chartDataItem.chartDataItemFlags.None))
                            o.Add(itm.ValueJsonObject);
                    
                }

                return ChartLogic.GetArrayString(o, true);

            }
            public decimal getTotal(bool IncludeRemainder)
            {
                var t = 0M;

                foreach (var lst in seriesRaw)
                {
                    foreach (var obj in lst)
                    {
                        if (IncludeRemainder | obj.Flags==chartDataItem.chartDataItemFlags.None)
                            t = t + obj.Value;
                    }
                }

                return t;
            }
          
            public string getSeriesRotated90(int seriesIndex,bool IncludeRemaining)
            {

                var thisList = seriesRaw.ElementAtOrDefault(seriesIndex);
                if (thisList == null)
                {
                    return string.Empty;
                }

                var NewList = new List<string>();
                foreach (var itm in thisList)
                {
                    if (itm.Flags ==chartDataItem.chartDataItemFlags.None | IncludeRemaining)
                    {

                        var TmpList = new List<string> {itm.ValueJsonObject};
                        NewList.Add(ChartLogic.GetArrayString(TmpList, true));
                    }
                }

                return ChartLogic.stringifySeries(NewList);

                //[248.15,20.00]
                //[[3],[5]]

            }

        }



        public static string stringifySeries(List<string> series)
        {
            var output = new System.Text.StringBuilder();
            output.Append("[");

            foreach (var str in series)
            {
                output.Append(str + ",");
            }

            output.Append("]");

            return output.ToString();
        }

        public static string GetArrayString(List<string> input,bool wrapInQuotes  )
        {
            if (input == null)
            {
                return string.Empty;
            }
            var output = new System.Text.StringBuilder();

            output.Append("[");

            for(int i=0; i<=input.Count()-1;i++)
            {
                if (i == input.Count() - 1)
                {
                    output.Append( checkWrapInQuotes(input[i],wrapInQuotes) );
                }
                else
                {
                    output.Append(checkWrapInQuotes(input[i],wrapInQuotes) + ",");
                }

            }

            output.Append("]");
            return output.ToString();

        }

        private static string checkWrapInQuotes(string input,bool wrapInQuotes)
        {
            if (input == null)
                    return null;
            

            if (wrapInQuotes && !input.Contains("{"))
                    return $"'{input}'";
            
            return input;
        }
               

    }
}

