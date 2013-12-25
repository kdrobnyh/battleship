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
    class Game
    {

    }


    class Client
    {
        private static List<Room> _rooms = new List<Room>();
        private IClientCallback _callback;
        private string _GUID;
        private string _name;
        private Room _room;
        private Game _game;

        public Client(IClientCallback callback, string name)
        {
            this._callback = callback;
            this._name = name;
            this._GUID = Guid.NewGuid().ToString();
            
            callback.LogIn(this._GUID);
            foreach (var room in Client.Rooms)
            {
                callback.RoomCreated(room);
            }
            _room = null;
            _game = null;
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
            if (_room != null || _game != null)
            {
                return null;
            }
            _room = new Room(_name, DateTime.Now);
            _rooms.Add(_room);
            return _room;
        }

        public bool DeleteRoom()
        {
            if (_room == null)
            {
                return false;
            }
            _rooms.Remove(_room);
            _room = null;
            return true;
        }

        public void Leave()
        {

        }

        public void JoinTo(Client opponent)
        {

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

        public bool HaveGame
        {
            get
            {
                return _game != null;
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
                Client newClient = null;
                try
                {
                    newClient = new Client(OperationContext.Current.GetCallbackChannel<IClientCallback>(), name);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error joining client {0}!", name);
                    return;
                }
                clientsDictionary.Add(name, newClient);
                Console.WriteLine("Client {0} joined!", name);
            }
            else
            {
                Console.WriteLine("Someone wants to get the used login {0}!", name);
                try
                {
                    OperationContext.Current.GetCallbackChannel<IClientCallback>().UserNameExists();
                }
                catch (Exception) { }
            }
        }

        public void CreateRoom(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                Room room = clientsDictionary[name].CreateRoom();
                if (room != null)
                {
                    Console.WriteLine("Client {0} created room!", name);
                    List<Client> failed = new List<Client>();
                    foreach (var client in clientsDictionary.Values)
                    {
                        try
                        {
                            client.Callback.RoomCreated(room);
                        }
                        catch (Exception e)
                        {
                            failed.Add(client);
                        }
                    }
                    SecureDeleteClients(failed);
                    return;
                }
                Console.WriteLine("Client {0} wants to create room, but room already created!", name);
                try
                {
                    clientsDictionary[name].Callback.FatalError("Room already created!");
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to create room!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void Leave(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                SecureDeleteClient(clientsDictionary[name]);
                return;
            }
            Console.WriteLine("Unknown client wants to leave!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        private void SecureDeleteClient(Client client)
        {
            clientsDictionary.Remove(client.Name);
            client.Leave();
            bool deleted = client.DeleteRoom();
            if (deleted)
            {
                Console.WriteLine("Client {0} deleted room!", client.Name);
                List<Client> failed = new List<Client>();
                foreach (var iclient in clientsDictionary.Values)
                {
                    try
                    {
                        iclient.Callback.RoomDeleted(client.Name);
                    }
                    catch (Exception)
                    {
                        failed.Add(client);
                    }
                }
                SecureDeleteClients(failed);
            }
            Console.WriteLine("Client {0} leave!", client.Name);
            return;
        }

        private void SecureDeleteClients(List<Client> clients)
        {
            if (clients.Count == 0)
            {
                return;
            }
            List<Client> failed = new List<Client>();
            foreach (var client in clients)
            {
                clientsDictionary.Remove(client.Name);
            }
            foreach (var client in clients)
            {
                client.Leave();
                bool deleted = client.DeleteRoom();
                if (deleted)
                {
                    Console.WriteLine("Client {0} deleted room!", client.Name);
                    foreach (var iclient in clientsDictionary.Values)
                    {
                        try
                        {
                            iclient.Callback.RoomDeleted(client.Name);
                        }
                        catch (Exception)
                        {
                            if (!failed.Contains(client))
                                failed.Add(client);
                        }
                    }
                    continue;
                }
                Console.WriteLine("Client {0} leave!", client.Name);
            }
            SecureDeleteClients(failed);
        }

        public void DeleteRoom(string name, string GUID)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                bool deleted = clientsDictionary[name].DeleteRoom();
                if (deleted)
                {
                    Console.WriteLine("Client {0} delete room!", name);
                    List<Client> failed = new List<Client>();
                    foreach (var client in clientsDictionary.Values)
                    {
                        try
                        {
                            client.Callback.RoomDeleted(name);
                        }
                        catch (Exception)
                        {
                            failed.Add(client);
                        }
                    }
                    SecureDeleteClients(failed);
                    return;
                }
                Console.WriteLine("Client {0} wants to delete room, but room does not exists!", name);
                try
                {
                    clientsDictionary[name].Callback.FatalError("Room already created!");
                }
                catch (Exception)
                {
                    SecureDeleteClient(clientsDictionary[name]);
                }
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to delete room!", name, GUID);
            try
            {
                OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
            }
            catch (Exception) { }
        }

        public void JoinGame(string name, string GUID, string oponent_name)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (clientsDictionary.ContainsKey(oponent_name) || clientsDictionary[name].HaveRoom || clientsDictionary[name].HaveGame)
                {
                    clientsDictionary[name].JoinTo(clientsDictionary[oponent_name]);
                }
                clientsDictionary[name].Callback.FatalError("Can't join game!");
                Console.WriteLine("Client {0} wants to join game, but can't!", name);
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to join game!", name, GUID);
            OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
        }

        public void ReadyForGame(string name, string GUID, bool[] field)
        {
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].CheckGUID(GUID))
            {
                if (!clientsDictionary[name].HaveGame)
                {
                    Console.WriteLine("Client {0} can't own game!", name);
                    OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("");
                    return;
                }
                if (field.Length != 100)
                {
                    Console.WriteLine("Client {0} send wrong field (size != 100)!", name);
                    OperationContext.Current.GetCallbackChannel<IClientCallback>().BadField("Wrong size of field!");
                    return;
                }
                int[] ships_count = new int[5];
                byte[] cell_status = new byte[100];
                for (int i = 0; i < 100; i++)
                {
                    if (field[i] && cell_status[i] == 2)
                    {
                        Console.WriteLine("Client {0} send wrong field (ship touch another ship)!", name, GUID);
                        clientsDictionary[name].Callback.BadField("Ship touch another ship!");
                        return;
                    }
                    if (field[i] && cell_status[i] == 0)
                    {
                        int horizontal_length = 1;
                        int j = i;
                        while (j % 10 != 0 && field[j])
                        {
                            horizontal_length += 1;
                            cell_status[j] = 1;
                            if (j < 90)
                                cell_status[j + 10] = 2; //2 is ship surroundings
                            j++;
                        }
                        if (j % 10 != 0)
                        {
                            cell_status[j] = 2;
                            if (j < 90)
                                cell_status[j + 10] = 2;
                        }
                        if (horizontal_length > 1)
                        {
                            if (horizontal_length > 4)
                            {
                                Console.WriteLine("Client {0} send ship which horizontal length > 4!", name, GUID);
                                clientsDictionary[name].Callback.BadField("Wrong ship length!");
                                return;
                            }
                            ships_count[horizontal_length]++;
                        }
                        else
                        {
                            int vertical_length = 1;
                            j = i;
                            while (j < 100 && field[j])
                            {
                                vertical_length += 1;
                                cell_status[j] = 1;
                                if (j % 10 != 0)
                                    cell_status[j - 1] = 2;
                                if (j % 10 != 9)
                                    cell_status[j + 1] = 2;
                                j += 10;
                            }
                            if (j < 100)
                            {
                                cell_status[j] = 2;
                                if (j % 10 != 0)
                                    cell_status[j - 1] = 2;
                                if (j % 10 != 9)
                                    cell_status[j + 1] = 2;
                            }
                            if (vertical_length > 4)
                            {
                                Console.WriteLine("Client {0} send ship which vertical length > 4!", name, GUID);
                                clientsDictionary[name].Callback.BadField("Wrong ship length!");
                                return;
                            }
                            ships_count[vertical_length]++;
                        }
                    }
                }
                if (ships_count[1] != 4)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    return;
                }
                if (ships_count[2] != 3)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    return;
                }
                if (ships_count[3] != 2)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    return;
                }
                if (ships_count[4] != 1)
                {
                    Console.WriteLine("Client {0} send wrong field (wrong ships number)!", name, GUID);
                    clientsDictionary[name].Callback.BadField("Wrong ship number!");
                    return;
                }
                Console.WriteLine("Client {0} send good field!", name, GUID);
                clientsDictionary[name].Callback.GoodField();
                return;
            }
            Console.WriteLine("Unknown client: ({0}, {1}) wants to join game!", name, GUID);
            OperationContext.Current.GetCallbackChannel<IClientCallback>().FatalError("Server don't know you!");
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
