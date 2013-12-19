using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using battleship_common;

namespace battleship_client
{
    /// <summary>
    /// Interaction logic for RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : UserControl
    {
        private ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        public RoomsPage()
        {
            Room room = new Room("ya", DateTime.Now);
            InitializeComponent();
            rooms.Add(room);
        }
        public ObservableCollection<Room> Rooms
        {
            get
            {
                return rooms;
            }
        }
    }
}
