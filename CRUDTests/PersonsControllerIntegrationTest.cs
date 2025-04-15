using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest
    {
        #region Index
        [Fact]
        public void Index_ToReturnView()
        {
            //Arrange

            //Act
           HttpResponseMessage response = _client.Getasync("/Persons/Index");

            //Assert
            response.Should().BeSuccessful();
        }
    }
    #endregion
}
