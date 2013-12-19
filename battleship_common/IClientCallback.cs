using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace battleship_common
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void RoomAdded(Room room);

        [OperationContract(IsOneWay = true)]
        void RoomDeleted(string name);
    }
}
