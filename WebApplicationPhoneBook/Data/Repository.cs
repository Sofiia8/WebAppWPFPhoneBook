using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPhoneBook.Models;

namespace WebApplicationPhoneBook.Data
{
    public class Repository/*: IRepository<Person>*/
    {
        private ApplicationContext _db;
        public Repository(ApplicationContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<Person>> GetItems()
        {
            return _db.Phonebook.ToList();
        }

        public async Task<Person> GetItemById(int id)
        {
            var item = _db.Phonebook.Where(i => i.ID == id).First();
            return item;
        }
        public async Task SaveNewData(Person person)
        {
            await _db.Phonebook.AddAsync(person);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRecord(int id)
        {
            var item = await GetItemById(id);
            _db.Phonebook.Remove(item);
            await _db.SaveChangesAsync();
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
            _db.Phonebook.Update(item);
            await _db.SaveChangesAsync();
        }

    }
}
