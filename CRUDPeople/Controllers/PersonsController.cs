using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDPeople.Controllers
{
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsService _personsService;

        //constructor
        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }   


        [Route("persons/Index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy=nameof(PersonResponse.PersonName),
            sortOrderOptions sortOrder=sortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.DateOfBirth),"DateOfBirth" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.Country),"Country" },
                { nameof(PersonResponse.Address),"Address" }
            };
            List<PersonResponse> persons= _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentsearchBy = searchBy;
            ViewBag.currentsearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons =  _personsService.GetSortedPersons(persons,sortBy,sortOrder);
            ViewBag.currentSortBy=sortBy;
            ViewBag.currentSortOrder=sortOrder;


            return View(sortedPersons);
        }
    }
}
