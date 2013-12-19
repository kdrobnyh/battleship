using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace battleship_common
{
    [DataContract]
    public class Room
    {
        private string name;
        private DateTime creationTime;

        public Room(string name, DateTime creationTime)
        {
            this.name = name;
            this.creationTime = creationTime;
        }

        [DataMember]
        public string Name
        {
            get { return name; }
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return creationTime; }
        }
    }
}
