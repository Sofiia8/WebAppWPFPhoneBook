using ClassLibraryLogic.Account;
using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF.PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для PageAddRecordPhoneBook.xaml
    /// </summary>
    public partial class PageAddRecordPhoneBook : Page
    {
        private readonly IRepository<Person> _repository;
        private readonly IAccount _account;

        public PageAddRecordPhoneBook(IRepository<Person> repository, IAccount account)
        {
            InitializeComponent();
            _repository = repository;
            _account = account;
        }
        public string Surname { get { return SurnameBox.Text; } }
        public string Name_ { get { return NameBox.Text; } }
        public string Secondname { get { return SecondnameBox.Text; } }
        public string Phone { get { return PhoneBox.Text; } }
        public string Address { get { return AddressBox.Text; } }
        public string Description { get { return DescriptionBox.Text; } }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpStatusCode result = await _repository.SaveNewData(new Person
            {
                Surname = this.Surname,
                Name = this.Name_,
                Secondname = this.Secondname,
                Phonenum = Phone,
                Address = this.Address,
                Description = this.Description
            }, _account.Token);

            if (result != HttpStatusCode.OK)
            {
                NavigationService.Navigate(new PageUnsuccess("Неизвестная ошибка"));
                return;
            }
            //if (_navigationService.CanGoBack)
            //    _navigationService.GoBack();
            NavigationService.Navigate(new PagePhoneBook(_repository));
        }
    }
}
