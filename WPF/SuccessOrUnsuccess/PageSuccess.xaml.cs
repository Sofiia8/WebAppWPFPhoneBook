using System.Windows.Controls;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для PageSuccess.xaml
    /// </summary>
    public partial class PageSuccess : Page
    {
        public PageSuccess(string text)
        {
            InitializeComponent();

            Alert.Text = text;
        }
    }
}
