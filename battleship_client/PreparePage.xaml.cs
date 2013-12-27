using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace battleship_client
{
    public partial class PreparePage : UserControl
    {
        private Main main;
        private bool []field;
        public PreparePage(Main main, string opponent_name, string initial_chat)
        {
            InitializeComponent();
            this.main = main;
            field = new bool[100];
            Greetings.Content = "Please, set you ships. You are playing with " + opponent_name;
            textBlockMessages.Text = initial_chat;
        }

        public void PostMessage(string message)
        {
            textBlockMessages.Text = textBlockMessages.Text + message + "\n";
        }

        public void Retry()
        {
        }

        private void Cell_LeftClick(object sender, RoutedEventArgs e)
        {
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = true;
            Style style = this.FindResource("FilledCell") as Style;
            cell.Style = style;
        }

        private void Cell_RightClick(object sender, RoutedEventArgs e)
        {
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = false;
            Style style = this.FindResource("EmptyCell") as Style;
            cell.Style = style;
        }

        private void Cell_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle cell = sender as Rectangle;
                field[int.Parse(cell.Name.Remove(0, 4))] = true;
                Style style = this.FindResource("FilledCell") as Style;
                cell.Style = style;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Rectangle cell = sender as Rectangle;
                field[int.Parse(cell.Name.Remove(0, 4))] = false;
                Style style = this.FindResource("EmptyCell") as Style;
                cell.Style = style;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.LeaveGame();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (messageInput.Text.Length > 0 || !string.IsNullOrEmpty(messageInput.Text))
            {
                main.SendMessage(messageInput.Text);
                messageInput.Clear();
            }
            else
                MessageBox.Show("Message is empty...", "Try again...", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
