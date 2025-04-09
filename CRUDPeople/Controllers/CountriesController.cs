using Microsoft.AspNetCore.Mvc;

namespace CRUDPeople.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
    }
}
