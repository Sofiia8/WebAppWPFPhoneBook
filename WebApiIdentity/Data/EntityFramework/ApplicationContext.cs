using WebApiIdentity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiIdentity
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
