using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplicationPhoneBook.Models;
using WebApplicationPhoneBook.ViewModels;

namespace WebApplicationPhoneBook.Controllers
{
    public class RolesController : Controller
    {
        static HttpClient HClient;

        public RolesController()
        {
            HClient = new HttpClient();
        }
        public async Task<IActionResult> Index() 
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                                        $"https://localhost:44315/api/rolesapi");
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
                return View(jToken.ToObject<List<IdentityRole>>());
            }

            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        }

        public async Task<IActionResult> UserList() 
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

            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        }

        public async Task<IActionResult> Edit(string userId, string userName)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                            $"https://localhost:44315/api/rolesapi/{userId}");
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
                JToken jT = JToken.Parse(json);

                List<string> userRoles = jT.ToObject<List<string>>();// получем список ролей пользователя

                httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                                $"https://localhost:44315/api/rolesapi");
                jwt = Request.Cookies["jwt"];
                if (jwt == null) return Redirect("/Account/Login");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("NotEnoughRights", "Account");
                    else
                        return RedirectToAction("NotSuccess", "Account", new { errors = "Неизвестная ошибка" });
                }
                json = await response.Content.ReadAsStringAsync();
                jT = JToken.Parse(json);

                List<IdentityRole> allRoles = jT.ToObject<List<IdentityRole>>();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = userId,
                    UserLogin = userName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                $"https://localhost:44315/api/rolesapi/edit/{userId}");
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Account/Login");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            httpRequestMessage.Content = JsonContent.Create(roles, typeof(List<string>));

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
                         { message = "Роли пользователя успешно отредактированы." });
            }

            catch (Exception ex)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
            }
        }
    }
}
