using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для PageSuccess.xaml
    /// </summary>
    public partial class PageUnsuccess : Page
    {
        public PageUnsuccess(string text)
        {
            InitializeComponent();

            try
            {
                JObject jo = JObject.Parse(text);
                string error = jo["errors"].ToString().Trim('{','}');
                Alert.Text = error;
            }
            catch
            {
                Alert.Text = text;
            }
        }
    }
}
