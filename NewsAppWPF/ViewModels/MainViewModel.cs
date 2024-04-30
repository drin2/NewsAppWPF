using GalaSoft.MvvmLight.Command;
using NewsAppWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NewsAppWPF.ViewModels
{
    public class MainViewModel
    {
        private Frame contentFrame;
        public ICommand NavigateCommand { get; }

        public MainViewModel(Frame contentFrame)
        {
            this.contentFrame = contentFrame;
            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        private void Navigate(string destination)
        {
            switch (destination)
            {
                case "Articles":
                    contentFrame.Navigate(new ArticlesViews());
                    break;
                case "Profile":
                    //contentFrame.Navigate(new ProfilePage());
                    break;
            }
        }
    }
}
