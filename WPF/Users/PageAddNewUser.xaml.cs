using ClassLibraryLogic.Models;
using ClassLibraryLogic.Users;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.Users
{
    /// <summary>
    /// Логика взаимодействия для PageAddNewUser.xaml
    /// </summary>
    public partial class PageAddNewUser : Page
    {
        private readonly string _token;
        private readonly IUser _user;
        public PageAddNewUser(string token)
        {
            InitializeComponent();
            _token = token;
            _user = new User();
        }
        public string userName { get { return NameBox.Text; } }
        public string Password { get { return PasswordBox.Text; } }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            RegisterModel registerModel = new RegisterModel { Login = userName, Password = this.Password, PasswordConfirm = this.Password };
            string[] result = await _user.Create(registerModel, _token);
            if (int.Parse(result[0]) == 0)
            {
                NavigationService.Navigate(new PageUnsuccess(result[1]));
            }
            else
            {
                NavigationService.Navigate(new PageSuccess(result[1]));
            }
        }
    }
}
