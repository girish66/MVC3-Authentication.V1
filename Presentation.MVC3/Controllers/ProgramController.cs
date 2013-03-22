using System.Web.Mvc;

namespace Presentation.MVC3.Controllers
{
    public class ProgramController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}