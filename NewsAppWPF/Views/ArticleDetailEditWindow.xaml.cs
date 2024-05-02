using NewsAppWPF.Models;
using NewsAppWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ArticleDetailEditWindow.xaml
    /// </summary>
    public partial class ArticleDetailEditWindow : Window
    {
        public ArticleDetailEditWindow(Article article)
        {
            InitializeComponent();
            DataContext = new ArticleDetailEditViewModel(article, CloseWindow);
        }
        private void CloseWindow()
        {
            this.Close();
        }
    }
}
