using System;
using VP.Extensions;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an object in 3D space
    /// </summary>
    public class VPObject
    {
        int id = 0;
        /// <summary>
        /// Gets the ID number of the object in the world. This field is read-only.
        /// </summary>
        /// <remarks>
        /// Automatically set by the server for new objects and is used to reference
        /// existing objects for changes or deletions
        /// </remarks>
        public int Id
        {
                     get { return id; }
            internal set { id = value; }
        }
        /// <summary>
        /// Gets the timestamp of this object's last known modification. This field is
        /// read-only.
        /// </summary>
        public readonly DateTime Time;
        /// <summary>
        /// Gets the account number of the owner that owns this object. This field is
        /// read-only.
        /// </summary>
        public readonly int Owner;
        /// <summary>
        /// Gets or sets the in-world Vector3D position of this object
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Gets or sets the Quaternion (3 axis + angle) rotation of this object
        /// </summary>
        public Rotation Rotation;
        /// <summary>
        /// Gets or sets the model file name of this object
        /// </summary>
        public string Model;
        /// <summary>
        /// Gets or sets the action script which powers client-side interactivity on this object
        /// </summary>
        public string Action;
        /// <summary>
        /// Gets or sets the description text of this object
        /// </summary>
        public string Description;
        /// <summary>Gets or sets the type of this object</summary>
        /// <remarks>
        /// Currently unused but is made available to allow for complete object backups
        /// </remarks>
        /// <value>0</value>
        public int Type;
        /// <summary>
        /// Gets or sets any arbitary data of this object
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Creates a VPObject for adding to the world using a <see cref="Vector3D"/>
        /// for position and default rotation
        /// </summary>
        public VPObject(string model, Vector3 position)
        {
            this.Model    = model;
            this.Position = position;
            this.Rotation = new Rotation();
        }

        /// <summary>
        /// Creates a VPObject for adding to the world using a <see cref="Vector3D"/>
        /// for position and a <see cref="Quaternion"/> for rotation
        /// </summary>
        public VPObject(string model, Vector3 position, Rotation rotation)
        {
            this.Model    = model;
            this.Position = position;
            this.Rotation = rotation;
        }
       
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
            Type        = Functions.vp_int(pointer, IntAttributes.ObjectType);
            Owner       = Functions.vp_int(pointer, IntAttributes.ObjectUserId);
            Data        = DataHandlers.GetData(pointer, DataAttributes.ObjectData);
            Position    = Vector3.FromObject(pointer);
            Rotation    = Rotation.FromObject(pointer);
        }

        internal void ToNative(IntPtr pointer)
        {
            Functions.vp_int_set   (pointer, IntAttributes.ObjectId,             this.Id);
            Functions.vp_string_set(pointer, StringAttributes.ObjectAction,      this.Action);
            Functions.vp_string_set(pointer, StringAttributes.ObjectDescription, this.Description);
            Functions.vp_string_set(pointer, StringAttributes.ObjectModel,       this.Model);
                                                                            
            Functions.vp_float_set (pointer, FloatAttributes.ObjectX, this.Position.X);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectY, this.Position.Y);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectZ, this.Position.Z);
            this.Rotation.ToObject(pointer);

            Functions.vp_int_set   (pointer, IntAttributes.ObjectType,            this.Type);

            if (Data != null)
                DataHandlers.SetData(pointer, DataAttributes.ObjectData, Data);
        }
    }
}
