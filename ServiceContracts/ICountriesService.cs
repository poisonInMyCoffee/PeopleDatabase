using ServiceContracts.DTO;
namespace ServiceContracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
    }
}
