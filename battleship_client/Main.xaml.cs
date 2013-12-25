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
        LoginPage loginPage;
        RoomsPage roomsPage;

        public Main()
        {
            InitializeComponent();
            this.client = new BattleshipServiceClient(new InstanceContext(this));
            roomsPage = new RoomsPage(this);
            SetContent(roomsPage);
            loginPage = new LoginPage(this);
            ((RoomsPage)this.Content).grid.Children.Add(loginPage);
            loginPage.SetValue(Grid.RowSpanProperty, 2);
            this.Closing += Dispatcher_ShutdownStarted;
        }

        public void Join(string name)
        {
            try
            {
                this._name = name;
                client.Join(name);
            }
            catch (Exception exception)
            {
                CantConnectToServer(exception.Message);
            }
        }

        public void CreateRoom()
        {
            try
            {
                client.CreateRoom(_name, _GUID);
            }
            catch (Exception exception)
            {
                CantConnectToServer(exception.Message);
            }
        }

        public void DeleteRoom()
        {
            try
            {
                client.DeleteRoom(_name, _GUID);
            }
            catch (Exception exception)
            {
                CantConnectToServer(exception.Message);
            }
        }

        private void SetContent(UserControl nextPage)
        {
            this.Content = nextPage;
            this.MinHeight = nextPage.MinHeight + 40;
            this.MinWidth = nextPage.MinWidth + 20;
            this.Height = nextPage.MinHeight + 40;
            this.Width = nextPage.MinWidth + 20;
        }

        public void LogIn(string GUID)
        {
            this._GUID = GUID;
            ((RoomsPage)this.Content).grid.Children.Remove(loginPage);
            loginPage = null;
        }

        public void UserNameExists()
        {
            MessageBox.Show("User name already exists on server!", "Retry!", MessageBoxButton.OK, MessageBoxImage.Warning);
            loginPage.Retry();
        }

        public void RoomCreated(battleship_common.Room room)
        {
            if (room.Name == _name)
            {
                WaitingOpponentPage waiting = new WaitingOpponentPage(this);
                SetContent(waiting);
            }
            roomsPage.AddRoom(room);
        }

        public void RoomDeleted(string name)
        {
            if (name == _name)
            {
                roomsPage.ResetButtons();
                SetContent(roomsPage);
            }
            roomsPage.DeleteRoom(name);
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

        public void YouCheated()
        {

        }

        public void CantConnectToServer(string message)
        {
            MessageBox.Show(message, "Can't connect to server!", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
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
