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
        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
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
                    MessageBox.Show("Registration successful.");
                    this.DialogResult = true;  // Close the window on success
                }
                else
                {
                    MessageBox.Show("Registration failed.");
                }
            }
            }
            else
            {
                MessageBox.Show("Please correct the errors before submitting.");
            }
        }
        private bool IsFormValid()
        {
            return !Validation.GetHasError(NameTextBox) &&
                   !Validation.GetHasError(EmailTextBox) &&
                   !Validation.GetHasError(PasswordBox); // Make sure you check the correct control for password
        }
    }
}
