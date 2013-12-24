using System;
using System.Windows;
using System.Windows.Controls;

namespace battleship_client
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private Main main;
        public LoginPage(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        public void LoggedIn()
        {
            Connect.IsEnabled = true;
            Login.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Connect.IsEnabled = false;
            Login.IsEnabled = false;
            try
            {
                // Connect.IsEnabled = true;
                // Login.IsEnabled = true;
                //main.Client.Join();
                main.Client.Join(Login.Text);
                main.Name = Login.Text;
                // if (GUID == "")
                // {
                //     MessageBox.Show("User with the same name is exists...", "Wrong username", MessageBoxButton.OK, MessageBoxImage.Error);
                // }
                // else
                // {
                //     ((RoomsPage)main.Content).grid.Children.Remove(this);
                //     main.GUID = GUID;
                //     main.Name = Login.Text;
                // }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Can't connect to server!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
