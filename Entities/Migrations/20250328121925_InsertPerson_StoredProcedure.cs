using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        //Incase of updation this method will excecute
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPersons = @"
            CREATE PROCEDURE [dbo].[InsertPerson]
            (@PersonID uniqueIdentifier, @PersonName nvarchar(40),@Email nvarchar(50),@DateOfBirth nvarchar(27),@Gender nvarchar(10),@CountryID uniqueidentifier,
            @Address nvarchar(1000),ReceiveNewsLetters bit)
            AS BEGIN 
            INSERT INTO [dbo].[Persons](PersonID,PersonName,Email,,DateOfBirth,,Gender,CountryId,Address,ReceiveNewsLetters) VALUES 
            (@PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryID,@ReceiveNewsLetters)
            END
            ";
            migrationBuilder.Sql(sp_InsertPersons);
        }

        /// <inheritdoc />
        /// incase the procedure gets overriden or taken back, this method excecutes.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPersons = @"DROP PROCEDURE [dbo].[InsertPerson]";

            migrationBuilder.Sql(sp_InsertPersons);

        }
    }
}

