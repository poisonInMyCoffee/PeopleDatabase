using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions options): base(options) 
        {
            
        }

        //DbSet represents 2 tables Country and person
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            //this class Connects the table from sql to EF(Model binding) 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to countries
            string countriesJson = System.IO.File.ReadAllText("Countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries) {
                modelBuilder.Entity<Country>().HasData(country);
            } 
            
            //Seed to persons
            string personsJson = System.IO.File.ReadAllText("Persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons) {
                modelBuilder.Entity<Person>().HasData(person);

                //FluentAPI[used to change the datatype or variable name in method]
                modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                    .HasColumnName("TaxIdentificationNumber")
                    .HasColumnType("varchar(8)")
                    .HasDefaultValue("ABC12345");

                //Adding constraint using fluent API(Not applying unique value constraint coz we have used default value if no value is entered)

                //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

                modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber])=8");

                //Table relations
                //modelBuilder.Entity<Person>(entity =>
                //{
                //    entity.HasOne<Country>(c => c.Country).WithMany(p => p.Persons).HasForeignKey(p=>p.CountryID);
                //});
            }
        }
        public List <Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter("@PersonID",person.PersonID),
                new SqlParameter("@PersonName",person.PersonName),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender",person.Gender),
                new SqlParameter("@CountryID",person.CountryID),
                new SqlParameter("@Address",person.Address),
                new SqlParameter("@ReceiveNewsLetters",person.ReceiveNewsLetters)
            };
            return Database.ExecuteSqlRaw("EXCECUTE [dbo].[InsertPerson] @PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryID,@Address,@ReceiveNewsLetters",parameters);
        }
    }
}
