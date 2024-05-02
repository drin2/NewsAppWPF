using GalaSoft.MvvmLight.Command;
using NewsAppWPF.Models;
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
using System.Windows.Input;

namespace NewsAppWPF.ViewModels
{
    public class ArticleDetailViewModel : INotifyPropertyChanged
    {
        private Article _article;
        public Article Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }
        private string _commentText;
        public string CommentText
        {
            get => _commentText;
            set { _commentText = value; OnPropertyChanged(); }
        }

        public ICommand PostCommentCommand { get; private set; }

        public ArticleDetailViewModel(Article article)
        {
            Article = article;
            PostCommentCommand = new RelayCommand(PostComment);

        }
        private async void PostComment()
        {
            if (string.IsNullOrWhiteSpace(CommentText))
            {
                MessageBox.Show("Comment cannot be empty.");
                return;
            }

            var comment = new PostComment
            {
                Text = CommentText,
                ArticleId = Article.ArticleId,
                UserId = SessionManager.CurrentUser.UserId, // Assuming you have a way to get the current user's ID
                CommentDate = DateOnly.FromDateTime(DateTime.Now)
            };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("https://localhost:7002/api/Comments", comment);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Comment posted successfully.");
                    Article.Comments.Add(new Comment {CommentDate = DateTime.Now, CommentId = 0, Text = comment.Text, UserName = SessionManager.CurrentUser.Name }); // Update the local list of comments
                }
                else
                {
                    MessageBox.Show("Failed to post comment.");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
