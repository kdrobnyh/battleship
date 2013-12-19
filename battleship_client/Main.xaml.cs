using battleship_client.BattleshipServerRef;
using battleship_common;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace battleship_client
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window, IBattleshipServiceCallback
    {
        private BattleshipServiceClient client;
        private string _GUID = "";
        private string _name;

        public Main()
        {
            InitializeComponent();
            
            this.client = new BattleshipServiceClient(new InstanceContext(this));
            Navigate(new RoomsPage());
            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
            this.MinHeight = nextPage.MinHeight + 40;
            this.MinWidth = nextPage.MinWidth + 20;
            this.Height = nextPage.MinHeight + 40;
            this.Width = nextPage.MinWidth + 20;
            LoginPage login = new LoginPage(this);
            ((RoomsPage)this.Content).grid.Children.Add(login);
            login.SetValue(Grid.RowSpanProperty, 2);
            //this.panel.Children.Add(nextPage);
            //cc.Content = nextPage;
            //this.Content = 
        }

        public void RoomAdded(Room room)
        {
            throw new NotImplementedException();
        }

        public void RoomDeleted(string name)
        {
            throw new NotImplementedException();
        }
        public BattleshipServiceClient Client
        {
            get
            {
                return client;
            }
        }

        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            if (_GUID != "")
            {
                client.Leave(_name, _GUID);
            }
        }
    }
}
