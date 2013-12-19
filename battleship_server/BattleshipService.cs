using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace battleship_server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BattleshipService : IBattleshipService
    {
        HashSet<string> users = new HashSet<string>();

        public bool Join(string name)
        {
            if (!users.Contains(name))
            {
                Console.WriteLine("Client {0} joined!", name);
                users.Add(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Leave(string name)
        {
            Console.WriteLine("Client {0} leave!", name);
            users.Remove(name);
        }
    }
}
