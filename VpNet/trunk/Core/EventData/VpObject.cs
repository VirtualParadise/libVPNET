using System;

namespace VpNet.Core.EventData
{
    public class VpObject
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }
        public int Owner { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float Angle { get; set; }

        public string Action { get; set; }
        public string Description { get; set; }
        public int ObjectType { get; set; }
        public string Model { get; set; }

        internal VpObject(int id, int type, DateTime time, int owner, float x, float y, float z, float rotationX, float rotationY, float rotationZ, float angle, string action, string description, int objectType, string model)
        {
            Id = id;
            Type = type;
            Time = time;
            Owner = owner;
            X = x;
            Y = y;
            Z = z;
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
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
