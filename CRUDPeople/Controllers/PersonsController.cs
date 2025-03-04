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
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsController(IPersonsService personsService,ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }   


        [Route("persons/Index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy=nameof(PersonResponse.PersonName),
            sortOrderOptions sortOrder=sortOrderOptions.ASC)
        {

            //filter
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

        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.countries = countries;
            return View();
        }
    }
}
