using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDPeople.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To Do: Add after logic here   
            _logger.LogInformation("{FilterName}.{MethodName}method",nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            PersonsController personsController = (PersonsController)context.Controller;

            IDictionary<string, object?>? parameters = (IDictionary<string, object?>)context.HttpContext.Items["arguments"];

            if (parameters != null)
            {
                if (parameters.ContainsKey("searchBy"))
                {
                    personsController.ViewData["searchBy"] = Convert.ToString(parameters["CurrentSearchBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {
                    personsController.ViewData["searchBy"] = Convert.ToString(parameters["searchString"]);
                }
                if (parameters.ContainsKey("sortBy"))
                {
                    personsController.ViewData["sortBy"] = Convert.ToString(parameters["CurrentSortBy"]);
                }
                if (parameters.ContainsKey("sortOrder"))
                {
                    personsController.ViewData["sortOrder"] = Convert.ToString(parameters["CurrentSortOrder"]);
                }


            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //To Do: add before logic here

            //This filter assures that if you give any other parameter for search by other than which are given that by default this filter
            //puts its value to person name


            context.HttpContext.Items["arguments"] = context.ActionArguments;
            //Check the parameter value
            _logger.LogInformation("{FilterName}.{MethodName}method", nameof(PersonsListActionFilter),nameof(OnActionExecuting));
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                //validate searchby parameter value
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                    };

                    //reset the searchBy parameter vallue
                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated Arguments {searchBy}", context.ActionArguments["searchBy"]);

                    }
                }
            }
        }
    }
}
