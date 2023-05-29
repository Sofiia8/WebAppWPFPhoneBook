using System.Windows;

namespace WPF.Users
{
    /// <summary>
    /// Логика взаимодействия для WindowDeleteUser.xaml
    /// </summary>
    public partial class WindowDeleteUser : Window
    {
        public WindowDeleteUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }   
}
