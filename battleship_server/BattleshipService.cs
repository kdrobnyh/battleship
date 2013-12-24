using battleship_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace battleship_server
{
    class Client
    {
        private static List<Room> _rooms = new List<Room>();
        private IClientCallback _callback;
        private string _GUID;
        private string _name;
        private Room _room;

        public Client(IClientCallback callback, string name)
        {
            this._callback = callback;
            this._name = name;
            this._GUID = new Guid().ToString();
            //callback.LogIn(this._GUID);
            _room = null;
        }

        public IClientCallback Callback
        {
            get { return _callback; }
        }

        public bool CheckGUID(string GUID)
        {
            return _GUID == GUID;
        }

        public string Name
        {
            get { return _name; }
        }

        public Room CreateRoom()
        {
            if (_room != null)
                return null;

            _room = new Room(_name, DateTime.Now);
            _rooms.Add(_room);
            return _room;
        }

        public bool DeleteRoom()
        {
            if (_room == null)
                return false;
            _rooms.Remove(_room);
            _room = null;
            return true;
        }

        public static List<Room> Rooms
        {
            get
            {
                return _rooms;
            }
        }

        public bool HaveRoom
        {
            get
            {
                return _room != null;
            }
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BattleshipService : IBattleshipService
    {
        Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();

        public void Join(string name)
        {
            if (!clientsDictionary.ContainsKey(name))
            {
                Console.WriteLine("Client {0} joined!", name);
                clientsDictionary.Add(name, new Client(OperationContext.Current.GetCallbackChannel<IClientCallback>(), name));
            }
            else
            {
                Console.WriteLine("Someone wants to get the used login {0}!", name);
                OperationContext.Current.GetCallbackChannel<IClientCallback>().UserNameExists();
            }
        }

        public void CreateRoom(string name, string GUID)
        {
            /*if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].GUID == GUID)
            {
                Room room = clientsDictionary[name].CreateRoom();
                if (room != null)
                {
                    foreach (var client in clientsDictionary.Values)
                    {
                        if (client.Name != name)
                        {
                            client.Callback.RoomAdded(room);
                        }
                    }
                    Console.WriteLine("Client {0} created room!", name);
                    //return true;
                }
                Console.WriteLine("Client {0} wants to create room, but room already created!", name);
                //return false;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to create room!", name, GUID);
            //return false;*/
        }

        public void Leave(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                clientsDictionary.Remove(name);
                Console.WriteLine("Client {0} leave!", name);
            }
            else
            {
                Console.WriteLine("Unknown client wants to leave!", name, GUID);
            }
        }

        public void GetRooms(string name, string GUID)
        {
            /*if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].GUID == GUID)
            {
                Console.WriteLine("Client {0} got rooms!", name);
                //return Client.Rooms;
            }
            Console.WriteLine("Unknown client wants to get rooms!", name, GUID);
            //return null;*/
        }

        public void DeleteRoom(string name, string GUID)
        {
            /*if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].GUID == GUID)
            {
                bool deleted = clientsDictionary[name].DeleteRoom();
                if (deleted)
                {
                    foreach (var client in clientsDictionary.Values)
                    {
                        if (client.Name != name)
                        {
                            try
                            {
                                client.Callback.RoomDeleted(name);
                            }
                            catch (Exception exception)
                            {
                                
                            }
                        }
                    }
                    Console.WriteLine("Client {0} deleted room!", name);
                    //return true;
                }
                Console.WriteLine("Client {0} wants to delete room, but room is not exists!", name);
                //return false;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to delete room!", name, GUID);
            //return false;*/
        }

        public void JoinGame(string name, string GUID, string oponent_name)
        {
            //if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].GUID == GUID)
            //{
            //    if (clientsDictionary.ContainsKey(oponent_name))
            //    {

            //    }
            //    Console.WriteLine("Client {0} wants to join game, but opponent is not exists!", name);
            //    //return false;
            //}
            //Console.WriteLine("Unknown client: ({0}, {1}) wants to join game!", name, GUID);
            ////return false;
        }

        public void ReadyForGame(string name, string GUID, bool[] field)
        {
            throw new NotImplementedException();
        }
        public void SendMessage(string name, string GUID, string text)
        {
            throw new NotImplementedException();
        }

        public void Turn(string name, string GUID, ShootType type, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
