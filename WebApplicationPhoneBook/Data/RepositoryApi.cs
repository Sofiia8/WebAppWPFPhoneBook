using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplicationPhoneBook.Models;

namespace WebApplicationPhoneBook.Data
{
    public class RepositoryApi : IRepository<Person>
    {
        public static HttpClient HClient;
        public RepositoryApi()
        {
            HClient = new HttpClient();
            HClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<HttpStatusCode> DeleteRecord(int id, string jwt)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                            $"https://localhost:44315/api/PhoneBook/{id}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await HClient.SendAsync(requestMessage);
                return response.StatusCode;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }

        public async Task<HttpStatusCode> EditRecord(int id, string surname, string name, string secondname, string phonenum, string address, string description, string jwt)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put,
                            $"https://localhost:44315/api/PhoneBook/{id}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            requestMessage.Content = JsonContent.Create (
                                      new Person(){
                                          Surname = surname,
                                          Name = name,
                                          Secondname = secondname,
                                          Phonenum = phonenum,
                                          Address = address,
                                          Description = description
                                      }, typeof(Person));
            try
            {
                var response = await HClient.SendAsync(requestMessage);
                return response.StatusCode;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }

        public async Task<Person> GetItemById(int id)
        {
            try
            {
                var response = await HClient.GetAsync($"https://localhost:44315/api/PhoneBook/{id}");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                string result = await response.Content.ReadAsStringAsync();
                JObject jO = JObject.Parse(result);
                return jO.ToObject<Person>();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Person>> GetItems()
        {
            try
            {
                var response = await HClient.GetAsync("https://localhost:44315/api/PhoneBook");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jT = JToken.Parse(result);
                return jT.ToObject<List<Person>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> SaveNewData(Person person, string jwt)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post,
                            "https://localhost:44315/api/PhoneBook/SaveNewData");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            requestMessage.Content = JsonContent.Create(person, typeof(Person));

            try
            {
                var response = await HClient.SendAsync(requestMessage);
                return response.StatusCode;
            }

            catch (Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }
    }
}
