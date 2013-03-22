using System;
using System.Web;
using System.Web.Mvc;
using ClassLibrary.Data;

namespace Presentation.MVC3.Filters
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizePrincipalAttribute : AuthorizeAttribute
    {
        public Modules Module { get; set; }
        public Permissions Permission { get; set; }
        public Operators Operators { get; set; } 

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var authorizePrincipal = filterContext.HttpContext.User as AuthorizePrincipal;
            if (authorizePrincipal == null)
            {
                CloseConnection(filterContext);
                return;
            }

            if(Module > 0 && Permission > 0)
            if(!authorizePrincipal.IsInRole(Module, Permission, Operators))
                CloseConnection(filterContext);
        }

        /// <summary>
        /// Closes the connection and sets the status code to Unauthorized
        /// </summary>
        /// <param name="filterContext">The AuthorizationContext</param>
        private void CloseConnection(AuthorizationContext filterContext)
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