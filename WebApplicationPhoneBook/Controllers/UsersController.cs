using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationPhoneBook.Models;
using WebApplicationPhoneBook.ViewModels;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Json;
using System;
using System.Net;

namespace WebApplicationPhoneBook.Controllers
{
    public class UsersController : Controller
    {
        static HttpClient HClient;
        public UsersController()
        {
            HClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44315/api/userapi");
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("NotEnoughRights", "Account");
                    else
                        return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                }

                string json = await response.Content.ReadAsStringAsync();
                JToken jToken = JToken.Parse(json);
                return View(jToken.ToObject<List<User>>());
            }

            catch(Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        } 

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                            $"https://localhost:44315/api/userapi/create");
                string? jwt = Request.Cookies["jwt"];
                if (jwt == null) return Redirect("/Account/Login");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                httpRequestMessage.Content = JsonContent.Create(
                        new { Login = model.Login, Password = model.Password});

                try
                {
                    var response = await HClient.SendAsync(httpRequestMessage);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                            return RedirectToAction("NotEnoughRights", "Account");
                        else
                            return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                    }
                    return RedirectToAction("Success", "Account", new
                        { message = $"Регистрация пользователя {model.Login} прошла успешно." });
                }

                catch (Exception ex)
                {
                    return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                            $"https://localhost:44315/api/userapi/{id}");
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("NotEnoughRights", "Account");
                    else
                        return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                }
                string json = await response.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(json);
            
                User user = jo.ToObject<User>();
                EditUserViewModel model = new EditUserViewModel { Id = user.Id, Login = user.UserName };
                return View(model);
            }

            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                                            $"https://localhost:44315/api/userapi/edit/{model.Id}");
                string? jwt = Request.Cookies["jwt"];
                if (jwt == null) return Redirect("/Account/Login");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                httpRequestMessage.Content = JsonContent.Create(
                        new { newLogin = model.Login});

                try
                {
                    var response = await HClient.SendAsync(httpRequestMessage);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                            return RedirectToAction("NotEnoughRights", "Account");
                        else
                            return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                    }
                    return RedirectToAction("Success", "Account", new
                                 { message = $"Пользователь {model.Login} успешно изменён." });
                }

                catch (Exception ex)
                {
                    return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                                        $"https://localhost:44315/api/userapi/delete/{id}");
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("NotEnoughRights", "Account");
                    else
                        return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                }
                return RedirectToAction("Success", "Account", new
                            { message = "Пользователь успешно удалён." });

            }

            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        }
    }
}
