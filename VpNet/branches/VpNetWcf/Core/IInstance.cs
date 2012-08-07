using System;
using System.ServiceModel;
using VpNet.Core.Structs;

namespace VpNet.Core
{
#if (WCF)
[ServiceContract(CallbackContract = typeof(IInstanceEvents),SessionMode = SessionMode.Required)] 
#endif
    public interface IInstance : IDisposable
    {
        event Instance.ChatEvent EventChat;
        event Instance.AvatarEvent EventAvatarAdd;
        event Instance.AvatarEvent EventAvatarChange;
        event Instance.AvatarEvent EventAvatarDelete;
        event Instance.ObjectChangeEvent EventObjectCreate;
        event Instance.ObjectChangeEvent EventObjectChange;
        event Instance.ObjectDeleteEvent EventObjectDelete;
        event Instance.ObjectClickEvent EventObjectClick;
        event Instance.WorldListEvent EventWorldList;
        event Instance.Event EventWorldSetting;
        event Instance.Event EventWorldSettingsChanged;
        event Instance.Event EventFriend;
        event Instance.Event EventWorldDisconnect;
        event Instance.Event EventUniverseDisconnect;
        event Instance.Event EventUserAttributes;
        event Instance.QueryCellResult EventQueryCellResult;

#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Wait(int milliseconds);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Connect(string host = "universe.virtualparadise.org", ushort port = 57000);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Login(string username, string password, string botname);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Enter(string worldname);
        /// <summary>
        /// Leave the current world
        /// </summary>
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Leave();

#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void UpdateAvatar(
            float x=0.0f, float y=0.0f, float z=0.0f, 
            float yaw=0.0f, float pitch=0.0f);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void ListWorlds();
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void QueryCell(int cellX, int cellZ);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void Say(string message);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void ChangeObject(VpObject vpObject);
#if (WCF)
        [OperationContract(IsOneWay = true)]
#endif
        void ReleaseEvents();

#if (WCF)
        bool ConnectWcf();
#endif

    }
}