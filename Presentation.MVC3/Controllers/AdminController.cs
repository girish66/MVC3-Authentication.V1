using System.Web.Mvc;
using ClassLibrary.Data;
using Presentation.MVC3.Filters;

namespace Presentation.MVC3.Controllers
{
    public class AdminController : Controller
    {
        [AuthorizePrincipal(Module = Modules.Program, Permission = Permissions.Write | Permissions.Delete, Operators = Operators.Or)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizePrincipal(Module = Modules.Program, Permission = Permissions.Write | Permissions.Read | Permissions.Delete, Operators = Operators.All)]
        public ActionResult All()
        {
            return View();
        }

        [AuthorizePrincipal(Module = Modules.Program, Permission = Permissions.Write | Permissions.Read | Permissions.Delete, Operators = Operators.Or)]
        public ActionResult Or()
        {
            return View();
        }

        [AuthorizePrincipal()]
        public ActionResult None()
        {
            return View();
        }

        public ActionResult Free()
        {
            return View();
        }
    }
}