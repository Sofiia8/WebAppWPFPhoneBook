using WebApplicationPhoneBook.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPhoneBook.Data
{
    public class RepositoryTestData /*: IRepository<Person>*/
    {
        private List<Person> _persons;
        public RepositoryTestData()
        {
            _persons = new List<Person> {
            new Person
            {
                ID = 1,
                Surname = "Васильев",
                Name = "В",
                Secondname = "Вич",
                Phonenum = "989",
                Address = "р",
                Description = "90"
            },
            new Person
            {
                ID = 2,
                Surname = "Васильева",
                Name = "Василиса",
                Secondname = "Премудрая",
                Phonenum = "999",
                Address = "д",
                Description = "98"
            }};
        }
        public async Task DeleteRecord(int id)
        {
            var item = await GetItemById(id);
            _persons.Remove(item);
        }

        public async Task EditRecord(int id, string surname, string name, string secondname, string phonenum, string address, string description)
        {
            var item = await GetItemById(id);
            item.Surname = surname;
            item.Name = name;
            item.Secondname = secondname;
            item.Phonenum = phonenum;
            item.Address = address;
            item.Description = description;
        }

        public async Task<Person> GetItemById(int id)
        {
            return _persons.Where(i => i.ID == id).First();
        }
        public async Task<IEnumerable<Person>> GetItems()
        {
            return _persons;
        }

        public async Task SaveNewData(Person person)
        {
            _persons.Add(person);
        }
    }
}
