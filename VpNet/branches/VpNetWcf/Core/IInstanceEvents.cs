using System.ServiceModel;
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VpNet.Core
{
    //[ServiceContract] <-- not needed, it is implied, by IInstanceMethods contract specification.
    public interface IInstanceEvents
    {
#if (WCF)
        [OperationContract]
        void ChatCallback(Instance sender, Chat eventData);
        [OperationContract]
        void AvatarCallback(Instance sender, Avatar eventData);
        [OperationContract]
        void WorldListCallback(Instance sender, World eventData);
        [OperationContract]
        void ObjectChangeCallback(Instance sender, int sessionId, VpObject objectData);
        [OperationContract]
        void ObjectCreateCallback(Instance sender, int sessionId, VpObject objectData);
        [OperationContract]
        void ObjectDeleteCallback(Instance sender, int sessionId, int objectId);
        [OperationContract]
        void ObjectClickCallback(Instance sender, int sessionId, int objectId);
        [OperationContract]
        void QueryCellResultCallback(Instance sender, VpObject objectData);

#endif
    }
}