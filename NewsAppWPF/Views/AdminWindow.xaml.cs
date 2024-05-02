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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
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
        
        private void AdvertismentsButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new AdvertisementView();
        }
        
        private void AdvertismentsPlaceButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new AdPlacementView();
        }
        private void EventLogButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new EventLogView();
        }
        private void ViewSessionButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new ViewingSessionView();
        }
        private void AddAdvertismentButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new AddAdvertisementView();
        }
        private void AddPublisherButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new AddPublisher();
        }
        private void ArticleStatsButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new ArticleStatisticsView();
        }
        private void SubscriptionStatsButton_Checked(object sender, RoutedEventArgs e)
        {
            CC.Content = new SubscriptionPieChartView();
        }


    }
}
