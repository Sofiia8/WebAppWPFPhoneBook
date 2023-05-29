using ClassLibraryLogic.Account;
using ClassLibraryLogic.Models;
using ClassLibraryLogic.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Windows;
using System.Windows.Controls;

namespace WPF.Users
{
    /// <summary>
    /// Логика взаимодействия для PageEditUser.xaml
    /// </summary>
    public partial class PageEditUser : Page
    {
        private readonly IUser _user;
        private readonly IAccount _account;
        private readonly IdentityUser _userEdited;

        public PageEditUser(IUser user, IAccount account, IdentityUser userEdited)
        {
            InitializeComponent();
            _user = user;
            _account = account;
            _userEdited = userEdited;
            NameBox.Text = userEdited.UserName;
        }

        public string UserName { get { return NameBox.Text; } }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] result = await _user.Edit(new EditUserModel { Id = _userEdited.Id, Login = UserName},
                                                                 _account.Token);
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
