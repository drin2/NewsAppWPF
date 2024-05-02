using NewsAppWPF.Services;
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
using System.Windows.Shapes;

namespace NewsAppWPF.Views
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = new RegistrationViewModel();
            this.Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            EmailTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            PasswordBox.GetBindingExpression(PasswordBox.TagProperty)?.UpdateSource();
            SubscriptionTypeComboBox.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(NameTextBox) || Validation.GetHasError(EmailTextBox) ||
          Validation.GetHasError(PasswordBox) || Validation.GetHasError(SubscriptionTypeComboBox))
            {
                MessageBox.Show("Please correct the errors before submitting.");
                return;
            }
            var name = NameTextBox.Text;
                var email = EmailTextBox.Text;
                var password = PasswordBox.Password;
                var subscriptionType = (SubscriptionTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                var subscriber = new { Name = name, Email = email, Password = password, SubscriptionType = subscriptionType };

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("https://localhost:7002/api/Users/AddSubscriber", subscriber);
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the JSON response to get the user ID
                        var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                        if (userResponse != null && userResponse.UserId > 0)
                        {
                            MessageBox.Show("Registration successful.");
                            await EventService.LogEvent("User Registered", userResponse.UserId);

                            this.DialogResult = true;  // Close the window on success
                        }
                        else
                        {
                            MessageBox.Show("Registration successful, but failed to retrieve user ID.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Registration failed: " + await response.Content.ReadAsStringAsync());
                    }
                }
           
        }

        public class UserResponse
        {
            public int UserId { get; set; }
        }
       
    }
}
