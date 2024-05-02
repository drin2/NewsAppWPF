using NewsAppWPF.Models;
using NewsAppWPF.Services;
using NewsAppWPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PublishArticleView.xaml
    /// </summary>
    public partial class PublishArticleView : UserControl
    {
        public ObservableCollection<string> PhotoUrls { get; } = new ObservableCollection<string>();

        public PublishArticleView()
        {
            InitializeComponent();
            PhotoUrlsListBox.ItemsSource = PhotoUrls;
        }

        private void AddPhotoUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PhotoUrlTextBox.Text))
            {
                PhotoUrls.Add(PhotoUrlTextBox.Text);
                PhotoUrlTextBox.Clear();
            }
        }

        private void RemovePhotoUrl_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoUrlsListBox.SelectedItem != null)
            {
                PhotoUrls.Remove(PhotoUrlsListBox.SelectedItem as string);
            }
        }

        private async void PublishArticle_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(TitleTextBox) || Validation.GetHasError(TextTextBox))
            {
                MessageBox.Show("Please correct the errors before submitting.");
                return;
            }
            var title = TitleTextBox.Text;
            var text = TextTextBox.Text;
            var photoUrls = PhotoUrls.ToList();
            var categoryItem = CategoryComboBox.SelectedItem as ComboBoxItem;
            var category = categoryItem?.Content.ToString();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Title and text cannot be empty.");
                return;
            }

            try
            {
                var article = new ArticleCreationDTO();
                article.Title = title;
                article.Text = text;
                article.PhotoUrls = photoUrls;
                article.UserId = SessionManager.CurrentUser.UserId;
                article.Category = category;

                using (var client = new HttpClient())
                {
                    // Update the URI to the correct endpoint where your API is hosted
                    var response = await client.PostAsJsonAsync("https://localhost:7002/api/Articles", article);

                    if (response.IsSuccessStatusCode)
                    {
                        await EventService.LogEvent("New Article Published", SessionManager.CurrentUser.UserId);
                        MessageBox.Show("Article published successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to publish article. Status: " + response.StatusCode + article.Category);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
