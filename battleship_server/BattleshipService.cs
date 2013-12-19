using battleship_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace battleship_server
{
    class Client
    {
        private IClientCallback _callback;
        private string _GUID;
        private string _name;

        public Client(IClientCallback callback, string name)
        {
            this._callback = callback;
            this._name = name;
            this._GUID = new Guid().ToString();
        }

        public IClientCallback Callback
        {
            get{return _callback;}
        }

        public string GUID
        {
            get{return _GUID;}
        }

        public string Name
        {
            get{return _name;}
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BattleshipService : IBattleshipService
    {
        HashSet<string> clientsSet = new HashSet<string>();
        Dictionary<string, Client> clientsDictionary = new Dictionary<string, Client>();

        public string Join(string name)
        {
            if (!clientsSet.Contains(name))
            {
                Console.WriteLine("Client {0} joined!", name);
                clientsSet.Add(name);
                clientsDictionary.Add(name, new Client(OperationContext.Current.GetCallbackChannel<IClientCallback>(), name));
                return clientsDictionary[name].GUID;
            }
            else
            {
                return "";
            }
        }

        public bool CreateRoom(string name, string GUID)
        {
            return true;
        }

        public void Leave(string name, string GUID)
        {
            Console.WriteLine("Client {0} leave!", name);
            if (clientsDictionary.ContainsKey(name) && clientsDictionary[name].GUID == GUID)
            {
                clientsSet.Remove(name);
                clientsDictionary.Remove(name);
            }
        }
    }
}
