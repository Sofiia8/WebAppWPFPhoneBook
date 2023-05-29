using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace ClassLibraryLogic.Data
{
    public interface IRepository<Person>
    {
        Task<ObservableCollection<Person>> GetItems();
        Task<Person> GetItemById(int id);
        Task<HttpStatusCode> SaveNewData(Person person, string jwt);
        Task<HttpStatusCode> DeleteRecord(int id, string jwt);
        Task<HttpStatusCode> EditRecord(int id, string surname, string name, string secondname, string phonenum, string address, string description, string jwt);
    }
}
