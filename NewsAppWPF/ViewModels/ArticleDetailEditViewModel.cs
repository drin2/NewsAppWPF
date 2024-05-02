using GalaSoft.MvvmLight.Command;
using NewsAppWPF.Models;
using NewsAppWPF.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NewsAppWPF.ViewModels
{
    public class ArticleDetailEditViewModel: INotifyPropertyChanged
    {
        private Article _article;
        private string _text;

        private readonly HttpClient _client = new HttpClient();

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private Action CloseAction { get; set; }

        public Article Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }

        public DependencyObject TextTextBox { get; private set; }

        public ArticleDetailEditViewModel(Article article, Action closeAction)
        {
            Article = article;
            _text = article.Text;
            SaveCommand = new RelayCommand(SaveChanges);
            DeleteCommand = new RelayCommand(DeleteArticle);
            CloseAction = closeAction;
        }
        private async void SaveChanges()
        {
            if (_text != Article.Text)
            {
                if(string.IsNullOrWhiteSpace(_text))
                {
                    MessageBox.Show("Text can not be empty");
                    return;
                }
                try
                {
                    ArticleUpdateDTO dto = new ArticleUpdateDTO();
                    dto.Text = Article.Text;
                    HttpResponseMessage response = await _client.PutAsJsonAsync($"https://localhost:7002/api/Articles/{Article.ArticleId}", dto);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Article updated successfully.");
                        await EventService.LogEvent($"Article {Article.ArticleId} edited", SessionManager.CurrentUser.UserId);
                    }
                    else
                    {
                        // Read the response body for more detailed error info
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to update article. Response: {response.StatusCode}, Details: {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception encountered: {ex.Message}");
                }
            }
            CloseAction?.Invoke();
        }


        private async void DeleteArticle()
        {
            var response = await _client.DeleteAsync($"https://localhost:7002/api/Articles/{Article.ArticleId}");
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Article deleted.");
                await EventService.LogEvent($"Article {Article.ArticleId} deleted", SessionManager.CurrentUser.UserId);
            }
            else
                MessageBox.Show("Failed to delete article.");
            CloseAction?.Invoke();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
