using ClassLibraryLogic.Account;
using ClassLibraryLogic.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.Account_
{
    /// <summary>
    /// Логика взаимодействия для PageRegister.xaml
    /// </summary>
    public partial class PageRegister : Page
    {
        private readonly IAccount _account;
        public PageRegister(IAccount account)
        {
            InitializeComponent();
            _account = account;
        }
        public string userName { get { return NameBox.Text; } }
        public string Password { get { return PasswordBox.Password; } }
        public string PasswordConfirm { get { return PasswordBoxSecond.Password; } }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Password != PasswordConfirm)
            {
                NavigationService.Navigate(new PageUnsuccess("Пароли не совпадают!"));
            }
            else
            {
                RegisterModel registerModel = new RegisterModel { Login = userName, Password = this.Password, PasswordConfirm = this.PasswordConfirm };
                string[] result = await _account.Register(registerModel);
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
}
