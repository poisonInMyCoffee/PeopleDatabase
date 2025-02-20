using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as DTO for inserting new person
    /// </summary>
    public class PersonAddRequest
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Creates the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns></returns>

        public Person ToPerson()
        {
            return new Person() { PersonName=PersonName, Email=Email,DateOfBirth=DateOfBirth,Gender=Gender.ToString(),
            Address=Address,CountryID=CountryID,ReceiveNewsLetter=ReceiveNewsLetter};
        }
    }
}
