using System;
using Attribute = VpNet.NativeApi.Attribute;
using VpNet.NativeApi;

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

        /// <summary>
        /// Applies the properties of this object to the native SDK's attributes
        /// </summary>
        internal void ToNative(IntPtr _instance)
        {
            Functions.vp_int_set(_instance, Attribute.ObjectId, this.Id);
            Functions.vp_string_set(_instance, Attribute.ObjectAction, this.Action);
            Functions.vp_string_set(_instance, Attribute.ObjectDescription, this.Description);
            Functions.vp_string_set(_instance, Attribute.ObjectModel, this.Model);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationX, this.Rotation.X);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationY, this.Rotation.Y);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationZ, this.Rotation.Z);
            Functions.vp_float_set(_instance, Attribute.ObjectX, this.Position.X);
            Functions.vp_float_set(_instance, Attribute.ObjectY, this.Position.Y);
            Functions.vp_float_set(_instance, Attribute.ObjectZ, this.Position.Z);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationAngle, this.Angle);
        }

        /// <summary>
        /// Sets the native object ID attribute to this object, targeting it for
        /// object modification functions
        /// </summary>
        /// <param name="_instance"></param>
        internal void Target(IntPtr _instance)
        {
            Functions.vp_int_set(_instance, Attribute.ObjectId, this.Id);
        }
    }
}
