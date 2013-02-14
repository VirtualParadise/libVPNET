using System;
using Attribute = VP.Native.VPAttribute;
using VP.Native;
using VP;

namespace VP
{
    public class VPObject
    {
        /// <summary>
        /// ID number of the object in the world; automatically set by the server
        /// </summary>
        public int Id;
        /// <summary>
        /// Timestamp of the object's last modification
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// Owner's account number
        /// </summary>
        public int Owner;
        /// <summary>
        /// Position of the object in the world
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Quaternion (3 axis + angle) rotation
        /// </summary>
        public Quaternion Rotation;

        public string Action;
        public string Description;
        public int    ObjectType;
        public string Model;

        public VPObject() { }
        
        /// <summary>
        /// Creates a VPObject from a native instance's attributes
        /// </summary>
        internal VPObject (IntPtr pointer) {
            Action      = Functions.vp_string(pointer, VPAttribute.ObjectAction);
            Description = Functions.vp_string(pointer, VPAttribute.ObjectDescription);
            Id          = Functions.vp_int(pointer, VPAttribute.ObjectId);
            Model       = Functions.vp_string(pointer, VPAttribute.ObjectModel);
            Time        = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, VPAttribute.ObjectTime));
            ObjectType  = Functions.vp_int(pointer, VPAttribute.ObjectType);
            Owner       = Functions.vp_int(pointer, VPAttribute.ObjectUserId);

            Position = new Vector3
            {
                X = Functions.vp_float(pointer, VPAttribute.ObjectX),
                Y = Functions.vp_float(pointer, VPAttribute.ObjectY),
                Z = Functions.vp_float(pointer, VPAttribute.ObjectZ)
            };

            Rotation = new Quaternion
            {
                W = Functions.vp_float(pointer, VPAttribute.ObjectRotationAngle),
                X = Functions.vp_float(pointer, VPAttribute.ObjectRotationX),
                Y = Functions.vp_float(pointer, VPAttribute.ObjectRotationY),
                Z = Functions.vp_float(pointer, VPAttribute.ObjectRotationZ)
            };
        }

        /// <summary>
        /// Applies the properties of this object to the native SDK's attributes
        /// </summary>
        internal void ToNative(IntPtr pointer)
        {
            Functions.vp_int_set   (pointer, Attribute.ObjectId,            this.Id);
            Functions.vp_string_set(pointer, Attribute.ObjectAction,        this.Action);
            Functions.vp_string_set(pointer, Attribute.ObjectDescription,   this.Description);
            Functions.vp_string_set(pointer, Attribute.ObjectModel,         this.Model);
                                                                            
            Functions.vp_float_set (pointer, Attribute.ObjectX,             this.Position.X);
            Functions.vp_float_set (pointer, Attribute.ObjectY,             this.Position.Y);
            Functions.vp_float_set (pointer, Attribute.ObjectZ,             this.Position.Z);
                                   
            Functions.vp_float_set (pointer, Attribute.ObjectRotationX,     this.Rotation.X);
            Functions.vp_float_set (pointer, Attribute.ObjectRotationY,     this.Rotation.Y);
            Functions.vp_float_set (pointer, Attribute.ObjectRotationZ,     this.Rotation.Z);
            Functions.vp_float_set (pointer, Attribute.ObjectRotationAngle, this.Rotation.W);
            Functions.vp_int_set   (pointer, Attribute.ObjectType,          this.ObjectType);
        }
    }
}
