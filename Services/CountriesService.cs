using ServiceContracts.DTO;
using ServiceContracts;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {
                                                                                                                                                                                                    
        private readonly List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
                    new Country() { CountryID = Guid.Parse("21576240-84EB-4D8D-A7E7-00C40A0115B7"), CountryName = "USA" },
                    new Country() { CountryID = Guid.Parse("B01F65E8-56AB-4D3B-8374-30A29DBA1A23"), CountryName = "UK" },
                    new Country() { CountryID = Guid.Parse("97D669DE-8883-44F3-B473-13AE8ACBC7E2"), CountryName = "Canada" },
                    new Country() { CountryID = Guid.Parse("8E8D69B9-B709-4649-A55E-6DF9E19728BD"), CountryName = "India" },
                    new Country() { CountryID = Guid.Parse("6D23C4F8-D0DD-4425-989C-79F5F0E8C13A"), CountryName = "Australia" },
                });
            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Validation
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Countryname can't be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
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

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryID)
        {
            if (countryID == null)
                return null;
            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);
            if (country_response_from_list == null) { return null; }

            return country_response_from_list.ToCountryResponse() ?? null;

        }
    }
}
