using System;
using System.Threading.Tasks;
using ClassLibraryLogic.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ClassLibraryLogic.Account
{
    public class Account: IAccount
    {
        public static HttpClient HClient;
        private string _token;
        public string Token { get { return _token; } }
        private string _userName;
        public string UserName { get { return _userName; } }
        private bool _authorized;
        public bool Authorized
        {
            get { return _authorized; }
            set
            {
                _authorized = value;
                OnAuthorizedChanged?.Invoke(_authorized);
            }
        }

        public static event Action<bool> OnAuthorizedChanged;

        public Account()
        {
            HClient = new HttpClient();
            HClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string[]> Register(RegisterModel model)
        {
            try
            {
                var response = await HClient.PostAsync("https://localhost:44315/api/userapi/register",
                        JsonContent.Create(
                            new { Login = model.Login, Password = model.Password, PasswordConfirm = model.PasswordConfirm }));
                if (response.IsSuccessStatusCode)
                    return new string[]{"1", $"Регистрация пользователя {model.Login} прошла успешно." };
                else
                {
                    var text = await response.Content.ReadAsStringAsync();
                    JToken jt = JToken.Parse(text);
                    var errors = jt.Select(er => er["description"].ToString()).ToList<string>();
                    return new string[]{"0", String.Join("\n", errors )};
                }
            }

            catch (Exception ex)
            {
                return new string[]{"0", "Сервис не доступен" };
            }
        }
        public async Task<string[]> Login(LoginModel model)
        {
            try
            {
                var response = await HClient.PostAsync("https://localhost:44315/api/userapi/login",
                        JsonContent.Create(new { Login = model.Login, Password = model.Password }));
                string[] result;

                if (!response.IsSuccessStatusCode)
                {
                    string text = await response.Content.ReadAsStringAsync();
                    result = new string[2] { "0", text.Trim('"') };
                    return result;
                }

                string json = await response.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(json);
                string token = jo["token"].ToString();

                _token = token;
                _userName = jo["userName"].ToString();
                Authorized = true;

                result = new string[2] { "1", $"Добро пожаловать, {_userName}! " };
                return result;

            }
            catch (Exception ex)
            {
                return new string[2] { "0", "Сервис не доступен" };
            }
        }

        public async void Logout()
        {
            _token = "";
            _userName = "";
            Authorized = false;
        }

    }
}
