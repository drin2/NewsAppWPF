using NewsAppWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Interaction logic for AddAdvertisementView.xaml
    /// </summary>
    public partial class AddAdvertisementView : UserControl
    {
        public AddAdvertisementView()
        {
            InitializeComponent();
        }
        private async void AddAdvertisement_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text;
            var text = TextTextBox.Text;
            if (!int.TryParse(DurationTextBox.Text, out var duration))
            {
                MessageBox.Show("Invalid duration. Please enter a number.");
                return;
            }
            var orderersEmail = OrderersEmailTextBox.Text;

            var ad = new
            {
                Title = title,
                Text = text,
                Duration = duration,
                OrderersEmail = orderersEmail
            };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("https://localhost:7002/api/Advertisements", ad);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Advertisement added successfully.");
                }
                else
                {
                    MessageBox.Show($"Failed to add advertisement. Status Code: {response.StatusCode}");
                }
            }
        }
    }
}
