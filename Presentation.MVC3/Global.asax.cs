using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ClassLibrary.Data;
using Presentation.MVC3.Filters;

namespace Presentation.MVC3
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogAuthorizePrincipalAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookie = FormsAuthentication.FormsCookieName;
            HttpCookie httpCookie = Context.Request.Cookies[cookie];
            if (httpCookie == null) return;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired) return;
            var identity = new FormsIdentity(ticket);
            var authorizePrinciplaRoles = AuthorizePrincipalRole.CreateAuthorizePrincipalRoles(ticket.UserData);
            var principal = new AuthorizePrincipal(identity, authorizePrinciplaRoles);
            Context.User = principal;
        }
    }
}