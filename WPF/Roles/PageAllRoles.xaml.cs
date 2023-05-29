using ClassLibraryLogic.Account;
using ClassLibraryLogic.Roles;
using ClassLibraryLogic.Users;
using System.Net;
using System.Windows.Controls;

namespace WPF.Roles
{
    /// <summary>
    /// Логика взаимодействия для PageAllRoles.xaml
    /// </summary>
    public partial class PageAllRoles : Page
    {
        //private readonly IUser _users;
        private readonly IAccount _account;
        private readonly IRole _roles;
        public PageAllRoles(IAccount account)
        {
            InitializeComponent();
            _account = account;
            //_users = new User();
            _roles = new Role();
        }

        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ResponseRoles responseRoles = await _roles.GetRoles(_account.Token);
            if (responseRoles.HttpStatus != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new PageUnsuccess(responseRoles.Message));
            }
            else
            {
                rolesList.ItemsSource = responseRoles.IdentityRoles;
            }
        }

        private void ButtonAllRolesUsersClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageUsersRoles(_account));
        }
    }
}
