using NewsAppWPF.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NewsAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;  // Set how the application should shut down

        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.IsLoggedIn)
            {
                Window mainWindow = null;  // Decide based on role
                switch (loginWindow.UserRole)
                {
                    case "Admin":
                        mainWindow = new AdminWindow();
                        break;
                    case "Publisher":
                        mainWindow = new PublisherWindow();
                        break;
                    case "Subscriber":
                        mainWindow = new SubscriberWindow();
                        break;
                    default:
                        MessageBox.Show("Unknown role or no role provided.");
                        break;
                }

                if (mainWindow != null)
                {
                    mainWindow.Show();
                }
                else
                {
                    Application.Current.Shutdown();  // If no window is shown, shut down
                }
            }
            else
            {
                Application.Current.Shutdown();  // Shutdown if not logged in
            }
        }
    }

}
