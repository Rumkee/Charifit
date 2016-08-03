using System.Web.Optimization;

namespace Calorie
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/External/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/External/jquery.validate*"));

            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/External/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/calorie").Include(
                  "~/Scripts/Calorie/calorie.js",
                  "~/Scripts/Calorie/helpers.js",
                  "~/Scripts/Calorie/Chart/charts.js",
                  "~/Scripts/Calorie/Chart/barChart.js",
                  "~/Scripts/Calorie/Chart/donutChart.js",
                  "~/Scripts/Calorie/Chart/miniDonutChart.js",
                  "~/Scripts/Calorie/Chart/lineChart.js",
                  "~/Scripts/Calorie/Shared/textEditor.js",
                  "~/Scripts/Calorie/currency.js",
                  "~/Scripts/Calorie/Account/account.js",
                  "~/Scripts/Calorie/user.js",
                  "~/Scripts/Calorie/images.js",
                  "~/Scripts/Calorie/Social/social.js",
                  "~/Scripts/Calorie/alerts.js",
                  "~/Scripts/Calorie/location.js",
                  "~/Scripts/Calorie/Teams/teams.js",
                  "~/Scripts/Calorie/Trackers/fitbit.js",
                  "~/Scripts/Calorie/Trackers/runKeeper.js"
                  ));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/External/chartist.min.js",
                        "~/Scripts/External/chartist-plugin-legend.js",
                        "~/Scripts/External/js.cookie.js",
                        "~/Scripts/External/handlebars.js",                                                                       
                        "~/Scripts/External/bigtext.js",
                        "~/Scripts/External/respond.js",
                        "~/Scripts/External/jquery.magnific-popup.min.js",
                        "~/Scripts/External/flipclock.min.js",
                        "~/Scripts/External/accounting.min.js",
                        "~/Scripts/External/jquery.easing.1.3.js",
                        "~/Scripts/External/parallax.min.js"



                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/CSS/chartist.min.css",
                        "~/CSS/magnific-popup.css",
                        "~/CSS/flipclock.css",
                        "~/CSS/BootstrapXL.css",
                        "~/CSS/chartistoverrides.css",
                        "~/CSS/Site.css"));
        }
    }
}
