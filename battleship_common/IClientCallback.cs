using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace battleship_common
{
    public enum ShootType {Shoot};

    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void LogIn(string GUID);

        [OperationContract(IsOneWay = true)]
        void UserNameExists();

        [OperationContract(IsOneWay = true)]
        void RoomCreated(Room room);

        [OperationContract(IsOneWay = true)]
        void RoomDeleted(string name);

        [OperationContract(IsOneWay = true)]
        void FatalError(string error);

        [OperationContract(IsOneWay = true)]
        void GameNotExists();

        [OperationContract(IsOneWay = true)]
        void PrepareToGame(string opponent_name);

        [OperationContract(IsOneWay = true)]
        void GoodField();

        [OperationContract(IsOneWay = true)]
        void BadField(string comment);

        [OperationContract(IsOneWay = true)]
        void StartGame();

        [OperationContract(IsOneWay = true)]
        void YouTurn();

        [OperationContract(IsOneWay = true)]
        void YouCheated();
    }
}

