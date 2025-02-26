using Microsoft.AspNetCore.Mvc;

namespace CRUDPeople.Controllers
{
    public class PersonsController : Controller
    {
        [Route("persons/Index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
