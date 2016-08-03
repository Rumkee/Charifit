using System.Web;
using RazorEngine;
using RazorEngine.Templating;

namespace Calorie.BusinessLogic
{
    public class Utilities
    {

        public static string RenderHTML<T>(string ViewPath, T Data)
        {
            

            string renderedHTML;

            if (Engine.Razor.IsTemplateCached(ViewPath, typeof(T)))
                renderedHTML = Engine.Razor.Run(ViewPath, typeof(T), Data);
            else
            {
                string template = System.IO.File.ReadAllText(ViewPath);
                renderedHTML = Engine.Razor.RunCompile(template, ViewPath, typeof(T), Data);
            }

            return renderedHTML;

        }

        public static string RenderGenericEmalHTML(Models.GenericEmailViewModel EmailVM)
        {

            var ViewPath = HttpContext.Current.Server.MapPath("~/Views/Emails/_GenericEmail.cshtml");

            if (Engine.Razor.IsTemplateCached(ViewPath, typeof(Models.GenericEmailViewModel)))
            {
                return Engine.Razor.Run(ViewPath, typeof(Models.GenericEmailViewModel), EmailVM);
            }

            var template = System.IO.File.ReadAllText(ViewPath);
            return Engine.Razor.RunCompile(template, ViewPath, typeof(Models.GenericEmailViewModel), EmailVM);
            
        }

        public static string RenderGenericEmalHTML(string _HTMLMessage , string _UserName, string _RootURL)
        {

          var EmailVM = new Models.GenericEmailViewModel()
            {
                HTMLContent = _HTMLMessage,
                Name = _UserName,
                RootURL = _RootURL
            };

            return RenderGenericEmalHTML(EmailVM);

        }

        public static string StarsHTML(int no)
        {
            
            var StarsHTML = "<i class='fa fa-star calorie-green-color' aria-hidden='true'></i>";
            var NoStarsHTML = "<i class='fa fa-star-o calorie-green-color' aria-hidden='true'></i>";

            var OutputHTML = string.Empty;

            for (var i = 1; i <= 5; i++)
            {
                if (i <= no)
                    OutputHTML += StarsHTML;
                else
                    OutputHTML += NoStarsHTML;
            }

            return OutputHTML;
        }

        public static string StarsHTMLGold(int no)
        {
            
            var StarsHTML = "<i class='fa fa-star calorie-gold-color ' aria-hidden='true'></i>";//fa-spin fa-spin-slow
            var NoStarsHTML = "<i class='fa fa-star-o calorie-gold-color' aria-hidden='true'></i>";

            var OutputHTML = string.Empty;

            for (var i = 1; i <= 5; i++)
            {
                if (i <= no)
                    OutputHTML += StarsHTML;
                else
                    OutputHTML += NoStarsHTML;
            }

            return OutputHTML;
        }


    }
}