using NewsAppWPF.Commands;
using NewsAppWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsAppWPF.ViewModels
{

    public class ArticlesViewModel : INotifyPropertyChanged
    {
        public ICommand ClearFiltersCommand { get; }

        public ObservableCollection<Article> Articles { get; private set; }
        public ObservableCollection<Article> FilteredArticles { get; private set; }
        public ObservableCollection<string> Categories { get; private set; }


        private string _selectedCategory;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _searchAuthor;

        public ArticlesViewModel()
        {
            Articles = new ObservableCollection<Article>();
            FilteredArticles = new ObservableCollection<Article>();
            Categories = new ObservableCollection<string>();
            LoadCategories();
            ClearFiltersCommand = new RelayCommand(ExecuteClearFilters);

            LoadArticlesAsync();
        }
        private void ExecuteClearFilters()
        {
            StartDate = null;
            EndDate = null;
            SelectedCategory = null;
            SearchAuthor = "";
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
