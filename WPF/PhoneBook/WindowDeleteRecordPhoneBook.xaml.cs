using System.Windows;

namespace WPF.PhoneBook
{
    /// <summary>
    /// Логика взаимодействия для WindowDeleteRecordPhobeBook.xaml
    /// </summary>
    public partial class WindowDeleteRecordPhoneBook : Window
    {
        public WindowDeleteRecordPhoneBook()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
