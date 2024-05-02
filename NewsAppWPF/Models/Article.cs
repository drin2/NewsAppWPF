using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class Article : INotifyPropertyChanged
    {
        private int articleId;
        private string title;
        private string text;
        private DateTime publicationDate;
        private string category;
        private string authorName;
        private ObservableCollection<ArticlePhoto> articlePhotos;
        private ObservableCollection<Comment> comments;

        public int ArticleId
        {
            get => articleId;
            set { articleId = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
        }

        public DateTime PublicationDate
        {
            get => publicationDate;
            set { publicationDate = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => category;
            set { category = value; OnPropertyChanged(); }
        }

        public string AuthorName
        {
            get => authorName;
            set { authorName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ArticlePhoto> ArticlePhotos
        {
            get => articlePhotos ?? (articlePhotos = new ObservableCollection<ArticlePhoto>());
            set { articlePhotos = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Comment> Comments
        {
            get => comments ?? (comments = new ObservableCollection<Comment>());
            set { comments = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
