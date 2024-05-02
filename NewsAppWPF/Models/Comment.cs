using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppWPF.Models
{
    public class Comment : INotifyPropertyChanged
    {
        private int commentId;
        private string text;
        private DateTime commentDate;
        private string userName; // Assuming the API sends only the name of the user

        public int CommentId
        {
            get => commentId;
            set { commentId = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
        }

        public DateTime CommentDate
        {
            get => commentDate;
            set { commentDate = value; OnPropertyChanged(); }
        }

        public string UserName
        {
            get => userName;
            set { userName = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
