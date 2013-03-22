using System.Web.Mvc;
using Presentation.MVC3.Filters;

namespace Presentation.MVC3.Controllers
{
    [LogAuthorizePrincipal()]
    public class BaseController : Controller
    {
    }
}