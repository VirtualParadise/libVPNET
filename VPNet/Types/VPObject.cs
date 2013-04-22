using System;
using VP.Native;

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
        internal VPObject (IntPtr pointer)
        {
            Action      = Functions.vp_string(pointer, StringAttributes.ObjectAction);
            Description = Functions.vp_string(pointer, StringAttributes.ObjectDescription);
            Id          = Functions.vp_int(pointer, IntAttributes.ObjectId);
            Model       = Functions.vp_string(pointer, StringAttributes.ObjectModel);
            Time        = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.ObjectTime));
            ObjectType  = Functions.vp_int(pointer, IntAttributes.ObjectType);
            Owner       = Functions.vp_int(pointer, IntAttributes.ObjectUserId);

            Position = new Vector3
            {
                X = Functions.vp_float(pointer, FloatAttributes.ObjectX),
                Y = Functions.vp_float(pointer, FloatAttributes.ObjectY),
                Z = Functions.vp_float(pointer, FloatAttributes.ObjectZ)
            };

            Rotation = new Quaternion
            {
                W = Functions.vp_float(pointer, FloatAttributes.ObjectRotationAngle),
                X = Functions.vp_float(pointer, FloatAttributes.ObjectRotationX),
                Y = Functions.vp_float(pointer, FloatAttributes.ObjectRotationY),
                Z = Functions.vp_float(pointer, FloatAttributes.ObjectRotationZ)
            };
        }

        /// <summary>
        /// Applies the properties of this object to the native SDK's attributes
        /// </summary>
        internal void ToNative(IntPtr pointer)
        {
            Functions.vp_int_set   (pointer, IntAttributes.ObjectId,             this.Id);
            Functions.vp_string_set(pointer, StringAttributes.ObjectAction,      this.Action);
            Functions.vp_string_set(pointer, StringAttributes.ObjectDescription, this.Description);
            Functions.vp_string_set(pointer, StringAttributes.ObjectModel,       this.Model);
                                                                            
            Functions.vp_float_set (pointer, FloatAttributes.ObjectX, this.Position.X);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectY, this.Position.Y);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectZ, this.Position.Z);
                                   
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationX,     this.Rotation.X);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationY,     this.Rotation.Y);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationZ,     this.Rotation.Z);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationAngle, this.Rotation.W);
            Functions.vp_int_set   (pointer, IntAttributes.ObjectType,            this.ObjectType);
        }
    }
}
