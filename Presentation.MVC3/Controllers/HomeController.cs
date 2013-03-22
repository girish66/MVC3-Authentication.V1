using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClassLibrary.Data;
using ClassLibrary.Data.Dto;

namespace Presentation.MVC3.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var userDto = new UserDto();
            UserDto user = userDto.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            return View(user);
        }

        public ActionResult Login(string userName)
        {
            var userDto = new UserDto();
            UserDto user = userDto.GetUser(userName);
            DoLogin(user);
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            ExpireCookie(".ASPXAUTH");
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Free", "Admin");
        }

        [NonAction]
        private void DoLogin(UserDto user)
        {
            var authorizePrincipalRoles = (from roleDto in user.Roles from moduleDto in roleDto.Modules from permissionDto in moduleDto.Permissions select new AuthorizePrincipalRole(roleDto.Name, moduleDto.Id, permissionDto.Id)).ToList();
            var authTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, AuthorizePrincipalRole.ToJson(authorizePrincipalRoles));
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var cookieSize = System.Text.Encoding.UTF8.GetByteCount(encryptedTicket);
            Debug.WriteLine(cookieSize, "Cookie Size:");
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [NonAction]
        private void ExpireCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] == null) return;
            var cookie = new HttpCookie(cookieName) {Expires = DateTime.Now.AddDays(-1d)};
            Response.Cookies.Add(cookie);
        }
    }
}