using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.ObjectModel;
using ClassLibraryLogic.Models;

namespace ClassLibraryLogic.Users
{
    public class User : IUser
    {
        public static HttpClient HClient;
        public User()
        {
            HClient = new HttpClient();
        }

        public async Task<ResponseUsers> GetUsers(string token)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44315/api/userapi");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return new ResponseUsers(response.StatusCode,
                                                    "У пользователя не достаточно прав.", null);
                    else
                        return new ResponseUsers(response.StatusCode,
                                                    "Неизвестная ошибка.", null);
                }

                string json = await response.Content.ReadAsStringAsync();
                JToken jToken = JToken.Parse(json);
                ObservableCollection<IdentityUser> listUsers = jToken.ToObject<ObservableCollection<IdentityUser>>();
                return new ResponseUsers(response.StatusCode, "Успех", listUsers);
            }

            catch (Exception ex)
            {
                return new ResponseUsers(HttpStatusCode.BadRequest,
                                                    "Сервис не доступен.", null);
            }
        }

        public async Task<string[]> Create(RegisterModel model, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                        $"https://localhost:44315/api/userapi/create");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            httpRequestMessage.Content = JsonContent.Create(
                            new
                            {
                                Login = model.Login,
                                Password = model.Password
                            });

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                    return new string[] { "1", $"Пользователь {model.Login} создан успешно." };
                else
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return new string[] { "0", $"У пользователя не достаточно прав." };
                    else
                    {
                        var text = await response.Content.ReadAsStringAsync();
                        JToken jt = JToken.Parse(text);
                        var errors = jt.Select(er => er["description"].ToString()).ToList<string>();
                        return new string[] { "0", String.Join("\n", errors) };
                    }
                }
            }

            catch (Exception ex)
            {
                return new string[] { "0", "Сервис не доступен" };
            }
        }

        public async Task<string[]> Edit(EditUserModel model, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                                        $"https://localhost:44315/api/userapi/edit/{model.Id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            httpRequestMessage.Content = JsonContent.Create(
                    new { newLogin = model.Login });

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return new string[] { "0", $"У пользователя не достаточно прав." };
                    else
                        return new string[] { "0", $"Неизвестная ошибка" };
                }
                return new string[] { "1", $"Пользователь {model.Login} отредактирован успешно." };
            }

            catch (Exception ex)
            {
                return new string[] { "0", "Сервис не доступен" };
            }

        }

        public async Task<HttpStatusCode> Delete(string id, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                                        $"https://localhost:44315/api/userapi/delete/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                return response.StatusCode;
            }

            catch (Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }
    }
}
