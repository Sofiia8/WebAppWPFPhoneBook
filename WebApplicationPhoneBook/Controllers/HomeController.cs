using WebApplicationPhoneBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using WebApplicationPhoneBook.Data;
using System.Threading.Tasks;
using System.Net;

namespace WebApplicationPhoneBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Person> _repository;
        public HomeController(IRepository<Person> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var answer = await _repository.GetItems();
            if (answer != null)
                return View(answer);
            else
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен"});
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewData(string surname, string name, string secondname, string phonenum, string address, string description)
        {
            Person newPerson = 
                        new Person()
                        {
                            Surname = surname,
                            Name = name,
                            Secondname = secondname,
                            Phonenum = phonenum,
                            Address = address,
                            Description = description
                        };
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            HttpStatusCode result = await _repository.SaveNewData(newPerson, jwt);

            if (result != HttpStatusCode.OK)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
            }

            return Redirect("~/");
        }

        public async Task<IActionResult> DeleteRecord(int id)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            HttpStatusCode result = await _repository.DeleteRecord(id, jwt);

            if (result != HttpStatusCode.OK) 
            { 
                if (result == HttpStatusCode.Forbidden)
                    return RedirectToAction("NotEnoughRights", "Account");
                else
                    return RedirectToAction("NotSuccess", "Account", new {errors = "Неизвестная ошибка"});
            }

            return Redirect("~/");
        }

        public async Task<IActionResult> EditRecord(int id)
        {
            if (Request.Cookies["jwt"] == null) return Redirect("/Account/Login");
            var response = await _repository.GetItemById(id);
            if (response != null)
            {
                return View(response);
            }
            else
                return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });

        }
        [HttpPost]
        public async Task<IActionResult> EditRecord(int id, string surname, string name, string secondname, string phonenum, string address, string description)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            HttpStatusCode result = await _repository.EditRecord(id, surname, name, secondname, phonenum, address, description, jwt);
            if (result != HttpStatusCode.OK)
            {
                if (result == HttpStatusCode.Forbidden)
                    return RedirectToAction("NotEnoughRights", "Account");
                else
                    return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
            }
            return Redirect("~/");
        }
        public async Task<IActionResult> Details(int id) 
        {

            var answer = await _repository.GetItemById(id);
            if (answer != null)
                return View(answer);
            else
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });

        }
    }
}
