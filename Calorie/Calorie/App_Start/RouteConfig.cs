using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Calorie
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //          name: "Root",
            //            url: "{action}",
            //            defaults: new { controller = "Root", action = "Index" },
            //        constraints: new { IsRootAction = new IsRootActionConstraint() }  // Route Constraint
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



        }
    }

    public class IsRootActionConstraint : IRouteConstraint
    {
        private Dictionary<string, Type> _controllers;

        public IsRootActionConstraint()
        {
            _controllers = Assembly
                                .GetCallingAssembly()
                                .GetTypes()
                                .Where(type => type.IsSubclassOf(typeof(Controller)))
                                .ToDictionary(key => key.Name.Replace("Controller", "").ToLower());
        }

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return !_controllers.Keys.Contains((values["action"] as string).ToLower());
        }

        #endregion
    }
}
