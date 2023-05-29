using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiIdentity.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        private ApplicationContext _db;
        public PhoneBookController(ApplicationContext applicationContext)
        {
            _db = applicationContext;
        }

        // GET: api/PhoneBook
        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(_db.Phonebook.ToList());
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("That's done");
        }

        // GET api/PhoneBook/5
        [HttpGet("{id}")]
        public Person GetItemById(int id)
        {
            Person item = _db.Phonebook.Where(i => i.ID == id).First();
            return item;
        }

        // POST api/PhoneBook/SaveNewData
        [Authorize]
        [HttpPost("SaveNewData")]
        public async Task SaveNewData([FromBody] Person person)
        {
            await _db.Phonebook.AddAsync(person);
            await _db.SaveChangesAsync();
        }

        // PUT api/PhoneBook/10
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task EditRecord(int id, [FromBody] Person person)
        {
            Person item = GetItemById(id);
            item.Surname = person.Surname;
            item.Name = person.Name;
            item.Secondname = person.Secondname;
            item.Phonenum = person.Phonenum;
            item.Address = person.Address;
            item.Description = person.Description;
            _db.Phonebook.Update(item);
            await _db.SaveChangesAsync();
        }

        // DELETE api/PhoneBook/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task DeleteRecord(int id)
        {
            var item = GetItemById(id);
            _db.Phonebook.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
}
