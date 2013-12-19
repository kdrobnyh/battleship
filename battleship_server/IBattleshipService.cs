using battleship_common;
using System.ServiceModel;


namespace battleship_server
{
    [ServiceContract(CallbackContract = typeof(IClientCallback), SessionMode = SessionMode.Required)]
    public interface IBattleshipService
    {
        [OperationContract]
        string Join(string name);

        [OperationContract]
        bool CreateRoom(string name, string GUID);

        [OperationContract(IsOneWay=true, IsTerminating = true)]
        void Leave(string name, string GUID);
    }
}

