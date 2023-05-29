using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApplicationPhoneBook.Data
{
    public interface IRepository<Person>
    {
        Task<IEnumerable<Person>> GetItems();
        Task<Person> GetItemById(int id);
        Task<HttpStatusCode> SaveNewData(Person person, string jwt);
        Task<HttpStatusCode> DeleteRecord(int id, string jwt);
        Task<HttpStatusCode> EditRecord(int id, string surname, string name, string secondname, string phonenum, string address, string description, string jwt);
    }
}
