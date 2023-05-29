using ClassLibraryLogic.Account;
using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF.PhoneBook;

namespace WPF.Account_
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        private IAccount _account;
        private readonly IRepository<Person> _repository;
        public PageLogin(IAccount account, IRepository<Person> repository)
        {
            InitializeComponent();
            _account = account;
            _repository = repository;
        }
        public string userName { get { return NameBox.Text; } }
        public string Password { get { return PasswordBox.Password; } }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginModel loginModel = new LoginModel { Login = userName, Password = this.Password };
            string[] result = await _account.Login(loginModel);
            if (int.Parse(result[0]) == 0)
            {
                NavigationService.Navigate(new PageUnsuccess(result[1]));

            }
            else
                NavigationService.Navigate(new PagePhoneBook(_repository));
        }
    }
}
