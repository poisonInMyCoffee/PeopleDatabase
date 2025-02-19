using ServiceContracts.DTO;
using ServiceContracts;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {

        private readonly List<Country> _countries;  
        public CountriesService()
        {
            _countries = new List<Country>(); 
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Validation
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Countryname can't be duplicate
            if(_countries.Where(temp=>temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }


                //Convert object from CountryAddRequest to Country type
                Country country = countryAddRequest.ToCountry();

            //generate countryId
            country.CountryID = Guid.NewGuid();

            //Add Country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }
    }
}
