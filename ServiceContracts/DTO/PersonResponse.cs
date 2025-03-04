using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that ios used as return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Age { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse Object to compare</param>
        /// <returns>True or false, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return PersonID == person.PersonID && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId && Address == person.Address && ReceiveNewsLetter == person.ReceiveNewsLetter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth?.ToString("dd MMM yyyy")}, Gender: {Gender}, Country ID: {CountryId}, Country: {Country}, Address: {Address}, Receive News Letters: {ReceiveNewsLetter}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest() { PersonID = PersonID, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true), Address = Address, CountryID = CountryId, ReceiveNewsLetters = ReceiveNewsLetter };
        }
    }
        public static class PersonExtensions
        {
            /// <summary>
            /// An extension method to convert an object of Person class into PersonResponse class
            /// </summary>
            /// <param name="person">The Person object to convert</param>
            /// /// <returns>Returns the converted PersonResponse object</returns>
            public static PersonResponse ToPersonResponse(this Person person)
            {
                //person => convert => PersonResponse
                return new PersonResponse()
                {
                    PersonID = person.PersonID,
                    PersonName = person.PersonName,
                    Email = person.Email,
                    DateOfBirth = person.DateOfBirth,
                    ReceiveNewsLetter = person.ReceiveNewsLetter,
                    Address = person.Address,
                    CountryId = person.CountryID,
                    Gender = person.Gender,
                    Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
                };
            }
        }
    }


