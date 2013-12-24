using battleship_client.BattleshipServerRef;
using battleship_common;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace battleship_client
{
    //class ClientInstance
    //{
    //    ClientInstance(string name, string GUID);
    //}

    public partial class Main : Window, IBattleshipServiceCallback
    {
        private BattleshipServiceClient client;
        private string _GUID = "";
        private string _name;
        LoginPage login;

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
            login = new LoginPage(this);
            ((RoomsPage)this.Content).grid.Children.Add(login);
            login.SetValue(Grid.RowSpanProperty, 2);
            //this.panel.Children.Add(nextPage);
            //cc.Content = nextPage;
            //this.Content = 
        }

        public void LogIn(string GUID)
        {
            this._GUID = GUID;
            login.LoggedIn();
        }

        public void UserNameExists()
        {

        }

        public void RoomCreated(battleship_common.Room room)
        {
            throw new NotImplementedException();
        }

        public void RoomDeleted(string name)
        {
            throw new NotImplementedException();
        }

        public void FatalError(string error)
        {

        }

        public void GoodField()
        {

        }

        public void BadField(string comment)
        {

        }

        public void PrepareToGame(string opponent_name)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {

        }

        public void YouTurn()
        {

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
