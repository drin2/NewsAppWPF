using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class ArticlePhoto : INotifyPropertyChanged
    {
        private int photoId;
        private string photoUrl;

        public int PhotoId
        {
            get => photoId;
            set { photoId = value; OnPropertyChanged(); }
        }

        public string PhotoUrl
        {
            get => photoUrl;
            set { photoUrl = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
