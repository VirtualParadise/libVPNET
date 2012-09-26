using VpNet.Core.Structs;

namespace VpNet.Core.Interfaces
{
    public interface IInstance
    {
        void Wait(int milliseconds);
        void Connect(string host = "universe.virtualparadise.org", ushort port = 57000);
        void Login(string username, string password, string botname);
        void Enter(string worldname);

        /// <summary>
        /// Leave the current world
        /// </summary>
        void Leave();

        void UpdateAvatar(
            float x=0.0f, float y=0.0f, float z=0.0f, 
            float yaw=0.0f, float pitch=0.0f);

        void ListWorlds();
        void QueryCell(int cellX, int cellZ);
        void Say(string message);
        void ChangeObject(VpObject vpObject);
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
        void ReleaseEvents();
        void Dispose();
    }
}