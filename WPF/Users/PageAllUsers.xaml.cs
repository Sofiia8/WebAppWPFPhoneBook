using ClassLibraryLogic.Account;
using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using ClassLibraryLogic.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using WPF.PhoneBook;

namespace WPF.Users
{
    /// <summary>
    /// Логика взаимодействия для PageAllUsers.xaml
    /// </summary>
    public partial class PageAllUsers : Page
    {
        private readonly IUser _users;
        private readonly IAccount _account;
        private readonly IRepository<Person> _repository;
        public PageAllUsers(IAccount account, IRepository<Person> repository)
        {
            InitializeComponent();
            _users = new User();
            _account = account;
            _repository = repository;
        }

        private void ButtonEditUserClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                var item = userList.SelectedItem;
                if (item is null || item.GetType() != typeof(IdentityUser))
                {
                    NavigationService.Navigate(new PageUnsuccess("Невозможно отредактировать запись."));
                    return;
                }
                else
                {
                    IdentityUser itemUser = item as IdentityUser;
                    NavigationService.Navigate(new PageEditUser(_users, _account, itemUser));
                }
            }
            else
            {
                NavigationService.Navigate(new PagePhoneBook(_repository));
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ResponseUsers responseUsers = await _users.GetUsers(_account.Token);
            if (responseUsers.HttpStatus != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new PageUnsuccess(responseUsers.Message));
            }
            else
            {
                userList.ItemsSource = responseUsers.IdentityUsers;
            }
        }

        private async void ButtonDeleteUserClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                WindowDeleteUser windowDeleteUser = new WindowDeleteUser();
                if (windowDeleteUser.ShowDialog() == true)
                {
                    var item = userList.SelectedItem;
                    if (item is null || item.GetType() != typeof(IdentityUser))
                    {
                        NavigationService.Navigate(new PageUnsuccess("Невозможно удалить пользователя."));
                        return;
                    }
                    else
                    {
                        IdentityUser itemUser = item as IdentityUser;
                        HttpStatusCode result = await _users.Delete(itemUser.Id, _account.Token);
                        if (result != HttpStatusCode.OK)
                        {
                            if (result == HttpStatusCode.Forbidden)
                            {
                                NavigationService.Navigate(new PageUnsuccess("У пользователя не хватает прав."));
                                return;
                            }
                            NavigationService.Navigate(new PageUnsuccess("Ошибка. Не удалось удалить пользователя."));
                            return;
                        }
                       
                        NavigationService.Navigate(new PageAllUsers(_account,_repository));
                        return;
                    }
                }
            }
            else
            {
                NavigationService.Navigate(new PagePhoneBook(_repository));
            }
        }
    }
}
