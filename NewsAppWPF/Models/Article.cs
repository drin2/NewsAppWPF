using System;
using System.Collections.Generic;
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
        private string? title;
        private string? text;
        private DateTime publicationDate;
        private string? category;
        private string? authorName;  // Assuming you only need the author's name for display

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
