using NewsAppWPF.Models;
using NewsAppWPF.Services;
using Newtonsoft.Json;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly HttpClient _client = new HttpClient();

        public bool IsLoggedIn { get; private set; }
        public string UserRole { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            IsLoggedIn = false;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool authenticated = await AuthenticateUser(EmailTextBox.Text, PasswordTextBox.Password);
            if (authenticated)
            {
                IsLoggedIn = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials, please try again.");
            }
        }

        private async Task<bool> AuthenticateUser(string email, string password)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("https://localhost:7002/api/Users/login", new { Email = email, Password = password });
                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                    UserRole = user.Role;
                    SessionManager.SetCurrentUser(user);
                    var subscriptionResponse = await _client.GetAsync($"https://localhost:7002/api/Users/{user.UserId}/SubscriptionType");
                    if (subscriptionResponse.IsSuccessStatusCode)
                    {
                        var subscriptionType = await subscriptionResponse.Content.ReadAsStringAsync();
                        user.SubscriptionType = subscriptionType;
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve subscription details.");
                        return false;
                    }
                    await EventService.LogEvent("User login", SessionManager.CurrentUser.UserId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the server: " + ex.Message);
            }
            return false;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            var result = registrationWindow.ShowDialog();  // This will show the registration window as a modal dialog

            if (result == true)
            {
                MessageBox.Show("Please login using your new credentials.");
            }
        }
    }
}
