using System;
using System.Windows;
using System.Windows.Controls;

namespace battleship_client
{
    public partial class GamePage : UserControl
    {
        private Main main;
        public GamePage(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        public void Retry()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
