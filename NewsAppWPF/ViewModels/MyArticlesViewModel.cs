using NewsAppWPF.Models;
using NewsAppWPF.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

namespace NewsAppWPF.ViewModels
{
    public class MyArticlesViewModel: INotifyPropertyChanged
    {
        public ICommand ClearFiltersCommand { get; }
        public ICommand OpenArticleDetailCommand { get; private set; }
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand ExportToXMLCommand { get; private set; }


        public ObservableCollection<Article> Articles { get; private set; }
        public ObservableCollection<Article> FilteredArticles { get; private set; }
        public ObservableCollection<string> Categories { get; private set; }


        private string _selectedCategory;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _searchAuthor;

        public MyArticlesViewModel()
        {
            Articles = new ObservableCollection<Article>();
            FilteredArticles = new ObservableCollection<Article>();
            Categories = new ObservableCollection<string>();
            LoadCategories();
            ClearFiltersCommand = new RelayCommand(ExecuteClearFilters);
            OpenArticleDetailCommand = new RelayCommand<object>(ArticleSelected);
            SearchAuthor = SessionManager.CurrentUser.Name;
            ExportToCSVCommand = new RelayCommand(ExportToCSV);
            ExportToXMLCommand = new RelayCommand(ExportToXML);



            LoadArticlesAsync();
        }
        private void ExportToCSV()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Title,Author,Category,PublicationDate");
            foreach (var article in FilteredArticles)
            {
                sb.AppendLine($"\"{article.Title}\",\"{article.AuthorName}\",\"{article.Category}\",\"{article.PublicationDate:yyyy-MM-dd}\"");
            }
            SaveFile(sb.ToString(), "CSV files|*.csv");
        }

        private void ExportToXML()
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Article>));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, FilteredArticles);
                SaveFile(writer.ToString(), "XML files|*.xml");
            }
        }

        private void SaveFile(string data, string filter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, data);
            }
        }
        private void ArticleSelected(object articleObject)
        {
            if (articleObject is Article article)
            {
                var detailWindow = new ArticleDetailEditWindow(article);
                detailWindow.Show();
            }
        }
        private void ExecuteClearFilters()
        {
            StartDate = null;
            EndDate = null;
            SelectedCategory = null;
            FilterArticles(); // Refresh the filtered articles
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterArticles();
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                if (EndDate.HasValue && EndDate < StartDate)
                {
                    EndDate = null;  // Reset end date if it's before the start date
                }
                FilterArticles();
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (value.HasValue && StartDate.HasValue && value < StartDate)
                {
                    MessageBox.Show("End date cannot be before start date.");
                    return;
                }
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                FilterArticles();
            }
        }

        public string SearchAuthor
        {
            get => _searchAuthor;
            set
            {
                _searchAuthor = value;
                OnPropertyChanged(nameof(SearchAuthor));
                FilterArticles();
            }
        }


        private void LoadCategories()
        {
            Categories.Add("Finance");
            Categories.Add("Politics");
            Categories.Add("War");
            Categories.Add("Tech");
            Categories.Add("IT");
            Categories.Add("Movies");
            Categories.Add("Sport");
            Categories.Add("Oil");
            Categories.Add("Games");

        }

        private void FilterArticles()
        {
            var filtered = Articles.Where(a =>
                (string.IsNullOrEmpty(SelectedCategory) || a.Category == SelectedCategory) &&
                (!StartDate.HasValue || a.PublicationDate.Date >= StartDate.Value.Date) &&
                (!EndDate.HasValue || a.PublicationDate.Date <= EndDate.Value.Date) &&
                (string.IsNullOrEmpty(SearchAuthor) || a.AuthorName.Contains(SearchAuthor, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            FilteredArticles.Clear();
            foreach (var article in filtered)
            {
                FilteredArticles.Add(article);
            }
        }

        private async void LoadArticlesAsync()
        {
            var apiUrl = "https://localhost:7002/api/Articles";  // Replace with your actual API URL
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var articles = JsonConvert.DeserializeObject<List<Article>>(json);
                    Articles.Clear();  // Ensure the main list is clear before adding new items
                    foreach (var article in articles)
                    {
                        Articles.Add(article);
                    }
                    Console.WriteLine(articles);
                    FilterArticles();  // Call filter articles here to initialize the filtered list
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

