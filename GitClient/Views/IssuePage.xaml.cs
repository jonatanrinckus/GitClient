using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitClient.Views
{
    /// <summary>
    /// Interaction logic for IssuePage.xaml
    /// </summary>
    public partial class IssuePage : Page
    {
        public IssuePage()
        {

            InitializeComponent();
            var list = new List<string>();
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");
            list.Add("adasdasdaddadsa");

            listBox.ItemsSource = list;

            comboBox.ItemsSource = list;

        }
    }
}
