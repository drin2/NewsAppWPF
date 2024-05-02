using NewsAppWPF.ViewModels;
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

namespace NewsAppWPF.Views
{
    /// <summary>
    /// Interaction logic for MyArticlesView.xaml
    /// </summary>
    public partial class MyArticlesView : UserControl
    {
        public MyArticlesView()
        {
            InitializeComponent();
            DataContext = new MyArticlesViewModel();

        }
    }
}
