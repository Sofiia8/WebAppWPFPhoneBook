using ClassLibraryLogic.Account;
using ClassLibraryLogic.Roles;
using ClassLibraryLogic.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.Roles
{
    /// <summary>
    /// Логика взаимодействия для PageUsersRoles.xaml
    /// </summary>
    public partial class PageUsersRoles : Page
    {
        private IAccount _account;
        private IRole _roles;
        public PageUsersRoles(IAccount account)
        {
            InitializeComponent();
            _account = account;
            _roles = new Role();
        }

        private async void ButtonRightsClick(object sender, RoutedEventArgs e)
        {
            var item = userList.SelectedItem;
            if (item is null || item.GetType() != typeof(IdentityUser))
            {
                NavigationService.Navigate(new PageUnsuccess("Невозможно показать права."));
                return;
            }
            else
            {
                IdentityUser itemUser = item as IdentityUser;
                NavigationService.Navigate(new PageEditListRoles(_account, itemUser));
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ResponseUsers responseUsers = await _roles.UserList(_account.Token);
            if (responseUsers.HttpStatus != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new PageUnsuccess(responseUsers.Message));
            }
            else
            {
                userList.ItemsSource = responseUsers.IdentityUsers;
            }
        }
    }
}
