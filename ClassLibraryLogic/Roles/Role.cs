using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ClassLibraryLogic.Users;
using ClassLibraryLogic.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace ClassLibraryLogic.Roles
{
    public class Role: IRole
    {
        static HttpClient HClient;

        public Role()
        {
            HClient = new HttpClient();
        }
        public async Task<ResponseRoles> GetRoles(string token)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                                        $"https://localhost:44315/api/rolesapi");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                        return new ResponseRoles(response.StatusCode,
                                                    "У пользователя не достаточно прав.", null);
                    else
                        return new ResponseRoles(response.StatusCode,
                                                    "Неизвестная ошибка.", null);
                }

                string json = await response.Content.ReadAsStringAsync();
                JToken jToken = JToken.Parse(json);
                ObservableCollection<IdentityRole> listRoles = jToken.ToObject<ObservableCollection<IdentityRole>>();
                return new ResponseRoles(response.StatusCode, "Успех", listRoles);
            }

            catch (Exception ex)
            {
                return new ResponseRoles(HttpStatusCode.BadRequest,
                                                    "Сервис не доступен.", null);
            }
        }

        public async Task<ResponseUsers> UserList(string token)
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

        public async Task<ChangeRoleModel> GetRolesByUser(string userId, string userName, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                            $"https://localhost:44315/api/rolesapi/{userId}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new ChangeRoleModel { StatusCode = response.StatusCode };
                }
                string json = await response.Content.ReadAsStringAsync();
                JToken jT = JToken.Parse(json);

                List<string> userRoles = jT.ToObject<List<string>>();// получем список ролей пользователя

                httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                                $"https://localhost:44315/api/rolesapi");
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                response = await HClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new ChangeRoleModel { StatusCode = response.StatusCode };
                }
                json = await response.Content.ReadAsStringAsync();
                jT = JToken.Parse(json);

                List<IdentityRole> allRoles = jT.ToObject<List<IdentityRole>>();
                ChangeRoleModel model = new ChangeRoleModel
                {
                    UserId = userId,
                    UserLogin = userName,
                    UserRoles = userRoles,
                    AllRoles = allRoles,
                    StatusCode = response.StatusCode
                };
                return model;
            }
            catch (Exception ex)
            {
                return new ChangeRoleModel { StatusCode = HttpStatusCode.ServiceUnavailable };
            }
        }

        public async Task<HttpStatusCode> Edit(string userId, List<string> roles, string jwt)
        {

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                $"https://localhost:44315/api/rolesapi/edit/{userId}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            httpRequestMessage.Content = JsonContent.Create(roles, typeof(List<string>));

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
