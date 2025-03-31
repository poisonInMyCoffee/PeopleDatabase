using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        //Incase of updation this method will excecute
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
            CREATE PROCEDURE [dbo].[GetAllPersons]
            AS BEGIN 
            SELECT PersonID,PersonName,Email,DateOfbirth,Gender,CountryID,Address,ReceiveNewsletters from [dbo].[Persons]
            END
";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        /// incase the procedure gets overriden or taken back, this method excecutes.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllpersons]";

            migrationBuilder.Sql(sp_GetAllPersons);

        }
    }
}
