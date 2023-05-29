using ClassLibraryLogic.Data;
using ClassLibraryLogic.Models;
using System.Windows;
using System.Windows.Controls;

namespace WPF.PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для WinPhoneBook.xaml
    /// </summary>
    public partial class PagePhoneBook : Page
    {
        private IRepository<Person> _repository;

        public PagePhoneBook(IRepository<Person> repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            personList.ItemsSource = await _repository.GetItems();
        }
    }
}
