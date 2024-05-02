using NewsAppWPF.Models;
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
    /// Interaction logic for AddPublisher.xaml
    /// </summary>
    public partial class AddPublisher : UserControl
    {
        public AddPublisher()
        {
            InitializeComponent();
        }
        private async void AddPublisher_Click(object sender, RoutedEventArgs e)
        {
             
            var email = EmailTextBox.Text;
            var name = NameTextBox.Text;
            var password = PasswordTextBox.Text;
            var publisher = new PublisherDto { Email = email, Name = name, Password = password };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("https://localhost:7002/api/Users/AddPublisher", publisher);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("publisher added successfully.");
                }
                else
                {
                    MessageBox.Show($"Failed to add publisher. Status Code: {response.StatusCode}");
                }
            }
        }
    }
}
