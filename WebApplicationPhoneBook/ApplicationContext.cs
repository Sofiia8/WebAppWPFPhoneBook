using WebApplicationPhoneBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplicationPhoneBook
{
    public class ApplicationContext: IdentityDbContext<User>
    {
        public DbSet<Person> Phonebook { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options):
            base(options)
        {
            var a = Database.EnsureCreated();
        }
        
    }
}
