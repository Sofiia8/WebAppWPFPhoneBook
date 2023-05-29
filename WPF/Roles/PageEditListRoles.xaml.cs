using ClassLibraryLogic.Account;
using ClassLibraryLogic.Models;
using ClassLibraryLogic.Roles;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.Roles
{
    /// <summary>
    /// Логика взаимодействия для PageEditListRoles.xaml
    /// </summary>
    public partial class PageEditListRoles : Page
    {
        private readonly IAccount _account;
        private readonly IRole _roles;
        private readonly IdentityUser _userEditedRoles;
        public ObservableCollection<IsRoleAvailableModel> DataModel;
        public PageEditListRoles(IAccount account, IdentityUser identityUser)
        {
            InitializeComponent();
            _account = account;
            _userEditedRoles = identityUser;
            _roles = new Role();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeRoleModel changeRoleModel = await _roles.GetRolesByUser(_userEditedRoles.Id,
                                                                        _userEditedRoles.UserName, _account.Token);
            if (changeRoleModel.StatusCode != HttpStatusCode.OK)
            {
                if (changeRoleModel.StatusCode == HttpStatusCode.Forbidden)
                {
                    NavigationService.Navigate(new PageUnsuccess("У пользователя не хватает прав."));
                    return;
                }
                NavigationService.Navigate(new PageUnsuccess("Ошибка. Не удалось удалить запись."));
                return;
            }
            else
            {
                var allRoles = changeRoleModel.AllRoles;
                var roles = changeRoleModel.UserRoles;
                DataModel = new ObservableCollection<IsRoleAvailableModel>();
                foreach(var item in allRoles)
                {
                    if (roles.Contains(item.Name))
                        DataModel.Add(new IsRoleAvailableModel { RoleName = item.Name, IsAvailable = true });
                    else
                        DataModel.Add(new IsRoleAvailableModel { RoleName = item.Name, IsAvailable = false });
                }
                roleList.ItemsSource = DataModel;
                DataContext = DataModel;
            }
            return;
        }

        private async void ButtonSaveRolesClick(object sender, RoutedEventArgs e)
        {
            List<string> newRoles = new List<string>();
            foreach(var item in DataModel)
            {
                if (item.IsAvailable)
                    newRoles.Add(item.RoleName);
            }
            HttpStatusCode httpStatusCode = await _roles.Edit(_userEditedRoles.Id, newRoles, _account.Token);
            if (httpStatusCode != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new PageUnsuccess("Ошибка. Не удалось изменить список ролей."));
            }
            else
            {
                NavigationService.Navigate(new PageUsersRoles(_account));
            }
            return;
        }
    }
}
