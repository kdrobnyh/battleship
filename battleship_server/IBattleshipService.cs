using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;


namespace battleship_server
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IBattleshipService
    {
        [OperationContract]
        bool Join(string name);

        [OperationContract(IsOneWay=true, IsTerminating = true)]
        void Leave(string name);
    }
}

