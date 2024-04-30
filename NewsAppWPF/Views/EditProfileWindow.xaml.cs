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
using System.Windows.Shapes;

namespace NewsAppWPF.Views
{
    /// <summary>
    /// Interaction logic for EditProfileWindow.xaml
    /// </summary>
    public partial class EditProfileWindow : Window
    {
        public User User { get; set; }

        public EditProfileWindow(User user)
        {
            InitializeComponent();
            User = user;
            this.DataContext = User;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Assume we have a method to update the user via API
            UpdateUserProfile(User);
            this.DialogResult = true;  // Close the window
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;  // Just close the window
        }

        private async Task UpdateUserProfile(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7002/");
                var response = await client.PutAsJsonAsync($"api/Users/{user.UserId}", user);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Profile updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update profile.");
                }
            }
        }
    }
}
