using System;
using System.Windows;
using System.Windows.Controls;

namespace battleship_client
{
    public partial class WaitingOpponentPage : UserControl
    {
        private Main main;
        public WaitingOpponentPage(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            main.DeleteRoom();
        }
    }
}
