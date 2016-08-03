using System.Collections.Generic;
using System.Linq;


namespace Calorie.BusinessLogic
{
    public class CurrencyLogic
    {
        public static Dictionary<int, string> getCurrencyOptions()
        {

            var Dict = new Dictionary<int, string>
            {
                {0, "£ British Pounds"},
                {1, "د.إ. United Arab Emirates Dirhams"},
                {2, "$ Australian Dollars"},
                {3, "$ Canadian Dollars"},
                {4, "€ Euros"},
                {5, "$ Hong Kong Dollars"},
                {6, "$ Singapore Dollars"},
                {7, "$ US Dollars"}
            };

            return Dict;
            
        }

        public static Dictionary<string, string> getCurrencyToCountryOptions()
        {

          
            var Dict = new Dictionary<string, string>
            {
                {"GB", "United Kingdom"},                
                {"AU", "Australia"},
                {"CA", "Canada"},
                {"DE", "Europe"},
                {"HK", "Hong Kong"},
                {"SG", "Singapore"},
                {"AE", "UAE"},
                {"US", "USA"}
            };

            return Dict;

        }


        public enum CurrencyEnum
        {
            GBP,
            AED, 
            AUD, 
            CAD, 
            EUR, 
            HKD, 
            SGD,
            USD
        }

        public static string GetCurrencyPrefix(CurrencyEnum C)
        {

            switch (C)
            {
                case CurrencyEnum.AED:
                    return "د.إ.(AED)";
                case CurrencyEnum.AUD:
                    return "$(AUD)";
                case CurrencyEnum.CAD:
                    return "$(CAD)";
                case CurrencyEnum.EUR:
                    return "€(EUR)";
                case CurrencyEnum.HKD:
                    return "$(HKD)";
                case CurrencyEnum.SGD:
                    return "$(SGD)";
                case CurrencyEnum.USD:
                    return "$(USD)";
                case CurrencyEnum.GBP:
                default:
                    return "£(GBP)";

            }

        }

        public static decimal GetCurrencyCoefficient(CurrencyEnum C)
        {

            switch (C)
            {
                case CurrencyEnum.AED:
                    return (decimal)5.57;
                case CurrencyEnum.AUD:
                    return (decimal)2.18;
                case CurrencyEnum.CAD:
                    return (decimal)2.04;
                case CurrencyEnum.EUR:
                    return (decimal)1.35;
                case CurrencyEnum.HKD:
                    return (decimal)11.76;
                case CurrencyEnum.SGD:
                    return (decimal)2.17;
                case CurrencyEnum.USD:
                    return (decimal)1.52;
                //case CurrencyEnum.GBP:
                default:
                    return (decimal)1.00;

            }

        }
       
        
        public static string GetDisplayAmount(string InputAmount, string InputCurrencyCode, string OutputCountryCode,CurrencyEnum? UserCurrency )
        {
            var OutputCurrency = CurrencyLogic.MapCountryCodeToCurrencyEnum(OutputCountryCode);
            var InputCurrency = CurrencyLogic.MapCurrencyStringToCurrencyEnum(InputCurrencyCode);

            if (UserCurrency.HasValue) {OutputCurrency = UserCurrency.Value;}

            decimal InputAmountDec = 0;
            decimal.TryParse(InputAmount,out InputAmountDec);

            decimal OutputAmount =InputAmountDec;
            if (InputCurrency !=OutputCurrency)
            {
                OutputAmount = ConvertAmount(InputCurrency, OutputCurrency, InputAmountDec);
                return $"~{GetCurrencyPrefix(OutputCurrency)}{OutputAmount.ToString("0.00")}";
            }

            return $"{GetCurrencyPrefix(OutputCurrency)}{OutputAmount.ToString("0.00")}";
        }
            
        public static string GetDisplayAmount(List<Models.Pledges.PledgeContributors> Inputs, string OutputCountryCode, CurrencyEnum? UserCurrency)
        {

            var OutputCurrency = MapCountryCodeToCurrencyEnum(OutputCountryCode);
            if (UserCurrency.HasValue) { OutputCurrency = UserCurrency.Value; }

            decimal Total = Inputs.Sum(PC => ConvertAmount(PC.Currency, OutputCurrency, PC.Amount));
            
            if (Inputs.Any(a => a.Currency != OutputCurrency))
                    return $"~{GetCurrencyPrefix(OutputCurrency)}{Total.ToString("0.00")}";

            return $"{GetCurrencyPrefix(OutputCurrency)}{Total.ToString("0.00")}";
        }

        public static decimal ToBase(List<Models.Pledges.PledgeContributors> Inputs) => Inputs.Sum(PC => ToBase(PC.Currency, PC.Amount));

        public static decimal ToCurrency(List<Models.Pledges.PledgeContributors> Inputs,CurrencyEnum OutputCurrency)
        {
            return Inputs.Sum(PC => ConvertAmount(PC.Currency,OutputCurrency,PC.Amount));
        }


        public static decimal ConvertAmount(CurrencyEnum From, CurrencyEnum To,decimal Amount)
        {
            //convert input to gbp then to output
            var InputAmountGBP = Amount / GetCurrencyCoefficient(From);
            return  InputAmountGBP * GetCurrencyCoefficient(To);
       }

        public static CurrencyEnum MapCountryCodeToCurrencyEnum(string CountryCode)
        {

            switch (CountryCode)
            {

                case "AT":
                case "BE":
                case "CY":
                case "EE":
                case "FI":
                case "FR":
                case "DE":
                case "GL":
                case "IE":
                case "IT":
                case "LV":
                case "LT":
                case "MT":
                case "NL":
                case "PT":
                case "ES":
                case "SI":
                case "SK":
                case "ME":
                    return CurrencyEnum.EUR;

                case "AE":
                    return CurrencyEnum.AED;

                case "AU":
                    return CurrencyEnum.AUD;

                case "CA":
                    return CurrencyEnum.CAD;

                case "GB":
                    return CurrencyEnum.GBP;

                case "HK":
                    return CurrencyEnum.HKD;

                case "SG":
                    return CurrencyEnum.SGD ;
              
                default:
                    return CurrencyEnum.USD;

            }

        }

        public static CurrencyEnum MapCurrencyStringToCurrencyEnum(string CurrencyString)
        {

            switch (CurrencyString)
            {

             case "EUR":
                    return CurrencyEnum.EUR;

                case "AED":
                    return CurrencyEnum.AED;

                case "AUD":
                    return CurrencyEnum.AUD;

                case "CAD":
                    return CurrencyEnum.CAD;

                case "GBP":
                    return CurrencyEnum.GBP;

                case "HKD":
                    return CurrencyEnum.HKD;

                case "SGD":
                    return CurrencyEnum.SGD;

                default:
                    return CurrencyEnum.USD;
                    
            }

        }

        public static decimal ToBase(CurrencyEnum From,decimal amount) => ConvertAmount(From, CurrencyEnum.GBP, amount);
    }

   

}