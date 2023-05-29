using System;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using ClassLibraryLogic.Account;
using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using WPF.Account_;
using WPF.PhoneBook;
using WPF.Roles;
using WPF.Users;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IRepository<Person> _repository;
        private IAccount _account;
        static public event Action<bool> OnChangeMode;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            _repository = new RepositoryApi();
            _account = new Account();
            Account.OnAuthorizedChanged += AuthorizedChanged;
        }

        private void AuthorizedChanged(bool authorized)
        {
            if (authorized)
            {
                Login.Content = _account.UserName + " В СЕТИ";
                Logout.Visibility = Visibility.Visible;
                Register.Visibility = Visibility.Collapsed;
                Users.Visibility = Visibility.Visible;
                Roles.Visibility = Visibility.Visible;
                AddUser.Visibility = Visibility.Visible;
            }
            else
            {
                Login.Content = "ВОЙТИ";
                Logout.Visibility = Visibility.Collapsed;
                Register.Visibility = Visibility.Visible;
                Users.Visibility = Visibility.Collapsed;
                Roles.Visibility = Visibility.Collapsed;
                AddUser.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new PagePhoneBook(_repository));
        }

        private void ButtonLoginClick(object sender, RoutedEventArgs e)
        {
            if (!_account.Authorized)
            {
                _mainFrame.Navigate(new PageLogin(_account, _repository));
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new PagePhoneBook(_repository));
        }

        private void ButtonLogoutClick(object sender, RoutedEventArgs e)
        {
            _account.Logout();
        }

        private void ButtonRegisterClick(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new PageRegister(_account));
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
                _mainFrame.Navigate(new PageAddRecordPhoneBook(_repository, _account));
            else
                _mainFrame.Navigate(new PageLogin(_account, _repository));
        }


        private async void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                WindowDeleteRecordPhoneBook windowDeleteRecordPhoneBook = new WindowDeleteRecordPhoneBook();
                if (windowDeleteRecordPhoneBook.ShowDialog() == true)
                {
                    var pagePhoneBook = _mainFrame.Content as PagePhoneBook;
                    var item = pagePhoneBook.personList.SelectedItem;
                    if (item is null || item.GetType() != typeof(Person))
                    {
                        _mainFrame.Navigate(new PageUnsuccess("Невозможно удалить запись."));
                        return;
                    }
                    else
                    {
                        Person itemPerson = item as Person;
                        HttpStatusCode result = await _repository.DeleteRecord(itemPerson.ID, _account.Token);
                        if (result != HttpStatusCode.OK)
                        {
                            if (result == HttpStatusCode.Forbidden)
                            {
                                _mainFrame.Navigate(new PageUnsuccess("У пользователя не хватает прав."));
                                return;
                            }
                            _mainFrame.Navigate(new PageUnsuccess("Ошибка. Не удалось удалить запись."));
                            return;
                        }
                        _mainFrame.Navigate(new PagePhoneBook(_repository));
                        return;
                    }
                }
            }
            else
                _mainFrame.Navigate(new PageLogin(_account, _repository));
        }

        private void ButtonEditClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                var pagePhoneBook = _mainFrame.Content as PagePhoneBook;
                var item = pagePhoneBook.personList.SelectedItem;
                if (item is null || item.GetType() != typeof(Person))
                {
                    _mainFrame.Navigate(new PageUnsuccess("Невозможно отредактировать запись."));
                    return;
                }
                else
                {
                    Person itemPerson = item as Person;
                    _mainFrame.Navigate(new PageEditRecordPhoneBook(_repository, _account, itemPerson.ID, itemPerson));
                }
            }
            else
            {
                _mainFrame.Navigate(new PageLogin(_account, _repository));
            }
        }

        private void ButtonUsersClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                _mainFrame.Navigate(new PageAllUsers(_account, _repository));
            }
            else
            {
                _mainFrame.Navigate(new PageLogin(_account, _repository));
            }
        }

        private void ButtonAddUserClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                _mainFrame.Navigate(new PageAddNewUser(_account.Token));
            }
            else
            {
                _mainFrame.Navigate(new PageLogin(_account, _repository));
            }
        }

        private void ButtonAllRolesClick(object sender, RoutedEventArgs e)
        {
            if (_account.Authorized)
            {
                _mainFrame.Navigate(new PageAllRoles(_account));
            }
            else
            {
                _mainFrame.Navigate(new PageLogin(_account, _repository));
            }
        }
    }
}
