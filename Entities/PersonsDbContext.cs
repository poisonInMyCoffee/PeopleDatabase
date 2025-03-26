using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            }
        }
        public List <Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXCECUTE [dbo].[GetAllPersons]").ToList();
        }
    }
}
