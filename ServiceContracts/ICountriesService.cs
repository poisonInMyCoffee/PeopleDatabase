using ServiceContracts.DTO;
namespace ServiceContracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries
        /// </summary>
        /// <returns>all countries from the list as List of countryResponse</CountryResponse> </returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns a country object based on given country id
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns></returns>
        CountryResponse? GetCountryByCountryId(Guid? countryID);
    }   
}
