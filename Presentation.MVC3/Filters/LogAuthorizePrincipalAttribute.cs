using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ClassLibrary.Data;
using ClassLibrary.Data.Dto;

namespace Presentation.MVC3.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogAuthorizePrincipalAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (String.Compare(filterContext.RouteData.Values["Controller"].ToString(), "Program", StringComparison.OrdinalIgnoreCase) == 0)
            {
                var user = HttpContext.Current.User;
                if (user.Identity.IsAuthenticated)
                {
                    // Get the module from the database
                    var m = new ModuleDto().GetModule(filterContext.RouteData.Values["Controller"].ToString());
                    if (m == null || m.Permissions.Count <= 0)
                        return;
                    // Construct the permissions demanded by the module
                    var mask = m.Permissions.Aggregate(0, (current, p) => current | (p.Id > 2 ? (int) Math.Pow(2, p.Id) : p.Id));

                    // Check the users permission
                    if (((AuthorizePrincipal) user).IsInRole((Modules)m.Id, (Permissions)mask, Operators.All))
                        Log("OnActionExecuting", filterContext.RouteData);
                    else
                    {
                        CloseConnection(filterContext);
                    }
                }
                else
                {
                    CloseConnection(filterContext);
                }
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    Log("OnActionExecuted", filterContext.RouteData);
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    Log("OnResultExecuting", filterContext.RouteData);
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    Log("OnResultExecuted", filterContext.RouteData);
        //}

        private void Log(string methodName, RouteData routeData)
        {
            object controllerName = routeData.Values["controller"];
            object actionName = routeData.Values["action"];
            string message = string.Format("{0} controller: {1} action: {2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
        }

        /// <summary>
        /// Closes the connection and sets the status code to Unauthorized
        /// </summary>
        /// <param name="filterContext">The ActionExecutingContext</param>
        private void CloseConnection(ActionExecutingContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;
            //Unauthorized status code
            response.StatusCode = 401;
            response.Write("You are not authorized to access this resource.");
            response.Flush();
            response.Close();
            response.End();

            //set the Result in order to prevent any further Filter attributes from executing
            filterContext.Result = new EmptyResult();
        }
    }
}