using System.Web.Mvc;
using ClassLibrary.Data;
using Presentation.MVC3.Filters;

namespace Presentation.MVC3.Controllers
{
    [AuthorizePrincipal(Module = Modules.Program, Permission = Permissions.Write | Permissions.Read | Permissions.Delete, Operators = Operators.All)]
    public class SuperController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}