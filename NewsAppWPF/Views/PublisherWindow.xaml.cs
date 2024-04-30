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
using System.Windows.Shapes;

namespace NewsAppWPF.Views
{
    /// <summary>
    /// Interaction logic for PublisherWindow.xaml
    /// </summary>
    public partial class PublisherWindow : Window
    {
        public PublisherWindow()
        {
            InitializeComponent();
            this.Closed += (sender, args) => Application.Current.Shutdown();
        }
        private void ArticleButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new ArticlesViews();
        }
        private void ProfileButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new ProfileView();
        }
    }
}
