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
        private bool ready;
        private string opponent_name;
        public PreparePage(Main main, string opponent_name, string initial_chat)
        {
            InitializeComponent();
            this.main = main;
            field = new bool[100];
            Greetings.Content = "Please, set you ships. You are playing with " + opponent_name;
            textBlockMessages.Text = initial_chat;
            ready = false;
            this.opponent_name = opponent_name;
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
            if (ready)
                return;
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = true;
            Style style = this.FindResource("FilledCell") as Style;
            cell.Style = style;
        }

        private void Cell_RightClick(object sender, RoutedEventArgs e)
        {
            if (ready)
                return;
            Rectangle cell = sender as Rectangle;
            field[int.Parse(cell.Name.Remove(0, 4))] = false;
            Style style = this.FindResource("EmptyCell") as Style;
            cell.Style = style;
        }

        private void Cell_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ready)
                return;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ready = true;
            ReadyButton.IsEnabled = false;
            main.CheckField(field);
        }

        public void GoodField()
        {
            MessageBox.Show("Please, wait your opponent!", "Good!", MessageBoxButton.OK, MessageBoxImage.Information);
            Greetings.Content = "Good! Wait your opponent. You are playing with " + opponent_name;
        }

        public void BadField(string message)
        {
            MessageBox.Show(message, "Wrong placements...", MessageBoxButton.OK, MessageBoxImage.Warning);
            ready = false;
            ReadyButton.IsEnabled = true;
        }

        public GamePage GetGamePage()
        {
            return new GamePage(main, opponent_name, textBlockMessages.Text, field);

        }
    }
}
