using System;
using System.Collections.Generic;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using static System.Collections.Specialized.BitVector32;
using RepositoryContracts;
using Moq;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        //private fields
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)  //It is the class which lets us create dummy object
        {
            _fixture = new Fixture(); 
            _personsRepositoryMock= new Mock<IPersonsRepository>(); //Using this method we can mock any method of IPersonsRepository 


            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            //Craete mock for DbContext
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
              new DbContextOptionsBuilder<ApplicationDbContext>().Options
             );

            //Access Mock DbContext object
            ApplicationDbContext dbContext = dbContextMock.Object;

            //Create mocks for DbSets(because in this service test we wiull be using both the db sets
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            //Create services based on mocked DbContext object
            _countriesService = new CountriesService(null);

            _personService = new PersonsService(null);

            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson 

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };
              
            await action.Should().ThrowAsync<ArgumentNullException>();

        }


        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(TemplateMethodQuery => TemplateMethodQuery.PersonName, null as string)
                .Create();

            //Act
            Func<Task> action=(async () =>
            {
                await _personService.AddPerson(personAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        
        }

        //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
           
            //PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person name...", Email = "person@example.com", Address = "sample address", CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01"), ReceiveNewsLetters = true };
            //Using autofixture this work is done by following method
            //PersonAddRequest? personAddRequest = _fixture.Create<PersonAddRequest>(); We do Not use this because it gives random data which isn't valid'
            //Instead we use build
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp=>temp.Email, "random@gmail.com"). //Lets us override the value
                Create();

            _personsRepositoryMock

            //Await
            PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = await _personService.GetAllPersons();

            //Assert
            // Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
           // Assert.Contains(person_response_from_add, persons_list);
            persons_list.Should().Contain(person_response_from_add); 
        }

        #endregion


        #region GetPersonByPersonID

        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

            //Assert
           // Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }


        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "sample@gmail.com").Create();
            PersonResponse person_response_from_add = await _personService.AddPerson(person_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
           // Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_add.Should().Be(person_response_from_add);
        }

        #endregion


        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

            //Assert
            //Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }


        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone121@gmail.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone123121@gmail.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone12121@gmail.com").Create();
           

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = await _personService.GetAllPersons();

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
           
               persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
            
        }
        #endregion


        #region GetFilteredPersons

        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone121@gmail.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone123121@gmail.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone12121@gmail.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
         
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }


        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp=>temp.CountryID,country_response_1.CountryID)
                .With(temp=>temp.PersonName,"Arjun")
                .With(temp => temp.Email, "someone121@gmail.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .With(temp => temp.PersonName, "Madhur")
                .With(temp => temp.Email, "someone123121@gmail.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Aditya")
                .With(temp => temp.Email, "someone12121@gmail.com").Create();


            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            
                    persons_list_from_search.Should().OnlyContain(temp=>temp.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase));
                
            
        }

        #endregion


        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Shubham")
                .With(temp => temp.Email, "someone121@gmail.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .With(temp => temp.PersonName, "Madhur")
                .With(temp => temp.Email, "someone123121@gmail.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Aditya")
                .With(temp => temp.Email, "someone12121@gmail.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert            
           // persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);
            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion


        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            Func<Task> action=async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>().Create();

            //Assert
            Func<Task> action=async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }


        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
           

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Shubham")
                .With(temp => temp.Email, "someone121@gmail.com").Create();
           
            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            //Assert
            var action=async () =>
            {
                //Act
                await _personService.UpdatePerson(person_update_request);
            };
           await action.Should().ThrowAsync<ArgumentException>();

        }


        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();


            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);


            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Shubham")
                .With(temp => temp.Email, "someone121@gmail.com").Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().BeEquivalentTo(person_response_from_get);

        }

        #endregion


        #region DeletePerson

        //If you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();


            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);


            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Shubham")
                .With(temp => temp.Email, "someone121@gmail.com").Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);


            //Act
            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }


        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();

        }

        #endregion
    }
}