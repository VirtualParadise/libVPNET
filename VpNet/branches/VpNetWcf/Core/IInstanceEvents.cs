#if (WCF)
using System.ServiceModel;
#endif
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VpNet.Core
{
    //[ServiceContract] <-- not needed, it is implied, by IInstanceMethods contract specification.
    public interface IInstanceEvents
    {
#if (WCF)
        [OperationContract(IsOneWay = true)]
        void ChatCallback(Chat eventData);
        [OperationContract(IsOneWay = true)]
        void AvatarCallback(Avatar eventData);
        [OperationContract(IsOneWay = true)]
        void WorldListCallback(World eventData);
        [OperationContract(IsOneWay = true)]
        void ObjectChangeCallback(int sessionId, VpObject objectData);
        [OperationContract(IsOneWay = true)]
        void ObjectCreateCallback(int sessionId, VpObject objectData);
        [OperationContract(IsOneWay = true)]
        void ObjectDeleteCallback(int sessionId, int objectId);
        [OperationContract(IsOneWay = true)]
        void ObjectClickCallback(int sessionId, int objectId);
        [OperationContract(IsOneWay = true)]
        void WorldDisconnectCallback();
        [OperationContract(IsOneWay = true)]
        void UniverseDisconnectCallback();
        [OperationContract(IsOneWay = true)]
        void QueryCellResultCallback(VpObject objectData);

#endif
    }
}