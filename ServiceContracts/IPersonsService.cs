using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Perosn entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Addds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);


        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns persons object based on given person id
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        PersonResponse GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns alll person object with given search field  `   
        /// </summary>
        /// <param name="SearchBy"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>

        List<PersonResponse> GetFilteredPersons(string SearchBy, string? searchString);

        /// <summary>
        /// 
        ///Name of property based on which persons list should be sorted asc or desc
        /// </summary>
        /// <param name="allPersons"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy
            , SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person response object after updation</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes person based on the given person id
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>

        bool DeletePerson(Guid? personID);


    }
}