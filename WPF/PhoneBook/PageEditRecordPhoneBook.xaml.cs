using ClassLibraryLogic.Account;
using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using System.Net;
using System.Windows.Controls;

namespace WPF.PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для PageDeleteRecordPhoneBook.xaml
    /// </summary>
    public partial class PageEditRecordPhoneBook : Page
    {
        private readonly IRepository<Person> _repository;
        private readonly IAccount _account;
        private readonly int _idPersonEdited;

        public PageEditRecordPhoneBook(IRepository<Person> repository, IAccount account, int id, Person person)
        {
            InitializeComponent();
            _repository = repository;
            _account = account;
            _idPersonEdited = id;
            SurnameBox.Text = person.Surname;
            NameBox.Text = person.Name;
            SecondnameBox.Text = person.Secondname;
            PhoneBox.Text = person.Phonenum;
            AddressBox.Text = person.Address;
            DescriptionBox.Text = person.Description;
        }
        public string Surname { get { return SurnameBox.Text; } }
        public string Name_ { get { return NameBox.Text; } }
        public string Secondname { get { return SecondnameBox.Text; } }
        public string Phone { get { return PhoneBox.Text; } }
        public string Address { get { return AddressBox.Text; } }
        public string Description { get { return DescriptionBox.Text; } }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HttpStatusCode result = await _repository.EditRecord(_idPersonEdited,
                                                                 Surname,
                                                                 Name_,
                                                                 Secondname,
                                                                 Phone,
                                                                 Address,
                                                                 Description,
                                                                 _account.Token);
            if (result != HttpStatusCode.OK)
            {
                if (result == HttpStatusCode.Forbidden)
                {
                    NavigationService.Navigate(new PageUnsuccess("У пользователя не хватает прав."));
                    return;
                }
                NavigationService.Navigate(new PageUnsuccess("Ошибка. Не удалось удалить запись."));
                return;
            }
            NavigationService.Navigate(new PagePhoneBook(_repository));
            return;

        }
    }
}
