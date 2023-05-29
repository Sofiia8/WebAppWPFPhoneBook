using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using WebApplicationPhoneBook.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace WebApplicationPhoneBook.Controllers
{
    public class AccountController : Controller
    {

        public static HttpClient HClient;

        public AccountController()
        {
            HClient = new HttpClient();
            HClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await HClient.PostAsync("https://localhost:44315/api/userapi/register",
                            JsonContent.Create(
                                new { Login = model.Login, Password = model.Password, PasswordConfirm = model.PasswordConfirm }));
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Success", new 
                                        { message = $"Регистрация пользовтеля {model.Login} прошла успешно." });
                    else 
                    {
                        var text = await response.Content.ReadAsStringAsync();
                        JToken jt = JToken.Parse(text);
                        var errors = jt.Select(er => er["description"].ToString());
                        return RedirectToAction("NotSuccess", new 
                                        { errors = String.Join('\n', errors.ToList<string>()) });
                    }
                }

                catch (Exception ex)
                {
                    return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await HClient.PostAsync("https://localhost:44315/api/userapi/login",
                            JsonContent.Create(new { Login = model.Login, Password = model.Password }));

                    if (!response.IsSuccessStatusCode)
                    {
                        string text = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("NotSuccess", new {errors = text.Trim('"')}); 
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    JObject jo = JObject.Parse(json);
                    string token = jo["token"].ToString();

                    Response.Cookies.Append("jwt", jo["token"].ToString(), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    var claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, jo["userName"].ToString()),
                                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                         new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("NotSuccess", "Account", new { errors = "Сервис не доступен" });
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");
            // удаляем аутентификационные куки
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> NotEnoughRights()
        {
            return View();
        }

        public async Task<IActionResult> Success(string message)
        {
            ViewBag.message = message;
            return View();
        }
        public async Task<IActionResult> NotSuccess(string errors)
        {
            ViewBag.errors = errors;
            return View();
        }
    }
}
