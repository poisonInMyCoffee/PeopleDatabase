using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Entities;
namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            //Compares current object to another object of country response type and returns true,if both are same otherwise returns false
            if (obj == null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse Country_To_Compare = (CountryResponse)obj;
            return CountryId== Country_To_Compare.CountryId &&  CountryName==Country_To_Compare.CountryName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}   
