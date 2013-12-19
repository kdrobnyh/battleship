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
    public class Room : INotifyPropertyChanged
    {
        private string name;
        private DateTime creationTime;
        public event PropertyChangedEventHandler PropertyChanged;

        public Room()
        {
        }

        public Room(string name, DateTime creationTime)
        {
            this.name = name;
            this.creationTime = creationTime;
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                creationTime = value;
                OnPropertyChanged("CreationTime");
            }
        }

        protected void OnPropertyChanged(string propValue)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propValue));
            }
        }
    }
}
