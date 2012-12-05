using System;

namespace VpNet.Core.Structs
{
    public class VpObject
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }
        public int Owner { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float Angle { get; set; }

        public string Action { get; set; }
        public string Description { get; set; }
        public int ObjectType { get; set; }
        public string Model { get; set; }

        internal VpObject(int id, int type, DateTime time, int owner, Vector3 position, Vector3 rotation, float angle, string action, string description, int objectType, string model)
        {
            Id = id;
            Type = type;
            Time = time;
            Owner = owner;
            Position = position;
            Rotation = rotation;
            Angle = angle;
            Action = action;
            Description = description;
            ObjectType = objectType;
            Model = model;
        }

        public VpObject()
        {
            Angle = float.MaxValue;
        }
    }
}
