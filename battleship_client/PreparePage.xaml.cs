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
        public PreparePage(Main main)
        {
            InitializeComponent();
            this.main = main;
            field = new bool[100];
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
    }
}
