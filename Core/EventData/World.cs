
namespace VpNet.Core.EventData
{
    public class World
    {
        public World() { }

        public World(string worldName, WorldState state, int userCount)
        {
            this.Name = worldName;
            this.UserCount = userCount;
            this.State = state;
        }

        public enum WorldState : int
        {
            Online,
            Stopped,
            Unknown
        }

        public string Name { get; set; }
        public int UserCount { get; set; }
        public WorldState State { get; set; }
    }
}
