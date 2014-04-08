using Nexus;
using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods, events and properties related to property management,
    /// including queries and global object events
    /// </summary>
    public class PropertyContainer
    {
        #region Construction & disposal
        Instance instance;

        internal PropertyContainer(Instance instance)
        {
            this.instance = instance;
            instance.setNativeEvent(Events.Object, OnObjectCreate);
            instance.setNativeEvent(Events.ObjectChange, OnObjectChange);
            instance.setNativeEvent(Events.ObjectDelete, OnObjectDelete);
            instance.setNativeEvent(Events.ObjectClick, OnObjectClick);
            instance.setNativeEvent(Events.QueryCellEnd, OnQueryCellEnd);

            instance.setNativeCallback(Callbacks.ObjectAdd, OnObjectCreateCallback);
            instance.setNativeCallback(Callbacks.ObjectChange, OnObjectChangeCallback);
            instance.setNativeCallback(Callbacks.ObjectDelete, OnObjectDeleteCallback);
            instance.setNativeCallback(Callbacks.ObjectGet, OnObjectGetCallback);
        }

        internal void Dispose()
        {
            QueryCellResult = null;
            QueryCellEnd    = null;
            ObjectCreate    = null;
            ObjectChange    = null;
            ObjectDelete    = null;
            ObjectClick     = null;

            CallbackObjectCreate = null;
            CallbackObjectChange = null;
            CallbackObjectDelete = null;
            CallbackObjectGet    = null;

            objReferences.Clear();
            idReferences.Clear();
        }
        #endregion

        #region Callback references
        Dictionary<int, VPObject> objReferences = new Dictionary<int, VPObject>();
        Dictionary<int, int>      idReferences  = new Dictionary<int, int>();

        int nextRef = int.MinValue;
        int nextReference
        {
            get
            {
                if (nextRef < int.MaxValue)
                    nextRef++;
                else
                    nextRef = int.MinValue;

                return nextRef;
            }
        } 
        #endregion

        #region Events and callbacks
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and an
        /// in-world object's data for the <see cref="QueryCellResult"/> event
        /// </summary>
        public delegate void QueryCellResultArgs(Instance sender, VPObject objectData);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and 2D
        /// coordinates for the <see cref="QueryCellEnd"/> event
        /// </summary>
        public delegate void QueryCellEndArgs(Instance sender, int x, int z);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, session
        /// ID and an in-world object's data for the <see cref="ObjectCreate"/> or
        /// <see cref="ObjectChange"/> events
        /// </summary>
        public delegate void ObjectCreateOrChangeArgs(Instance sender, int sessionId, VPObject objectData);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, session
        /// ID and an in-world object's ID for the <see cref="ObjectDelete"/> event
        /// </summary>
        public delegate void ObjectDeleteArgs(Instance sender, int sessionId, int objectId);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and an
        /// <see cref="VP.ObjectClick"/> for the <see cref="ObjectClick"/> event
        /// </summary>
        public delegate void ObjectClickArgs(Instance sender, ObjectClick click);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// reason code and a <see cref="VPObject"/> for
        /// <see cref="CallbackObjectCreate"/> and <see cref="CallbackObjectChange"/>
        /// </summary>
        public delegate void ObjectCallbackArgs(Instance sender, ReasonCode result, VPObject obj);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// reason code and a unique ID for <see cref="CallbackObjectDelete"/>
        /// </summary>
        public delegate void ObjectDeleteCallbackArgs(Instance sender, ReasonCode result, int id);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// reason code and a <see cref="VPObject"/> for <see cref="CallbackObjectGet"/>
        /// </summary>
        public delegate void ObjectGetCallbackArgs(Instance sender, ReasonCode result, VPObject obj);

        /// <summary>
        /// Fired for each object found in a cell after a call to
        /// <see cref="QueryCell"/>, providing the object's data
        /// </summary>
        public event QueryCellResultArgs QueryCellResult;
        /// <summary>
        /// Fired when a <see cref="QueryCell"/> call is complete after all objects found
        /// in the cell have been sent
        /// </summary>
        public event QueryCellEndArgs QueryCellEnd;
        /// <summary>
        /// Fired when an object is created anywhere in world, providing the object data
        /// and source session ID
        /// </summary>
        public event ObjectCreateOrChangeArgs ObjectCreate;
        /// <summary>
        /// Fired when an object is changed anywhere in world, providing the object data
        /// and source session ID
        /// </summary>
        public event ObjectCreateOrChangeArgs ObjectChange;
        /// <summary>
        /// Fired when an object is deleted anywhere in world, providing the object's ID
        /// and source session ID
        /// </summary>
        public event ObjectDeleteArgs ObjectDelete;
        /// <summary>
        /// Fired when an object is clicked anywhere in the world, providing click
        /// coordinates and source ID
        /// </summary>
        public event ObjectClickArgs ObjectClick;

        /// <summary>
        /// Fired after a call to the asynchronous <see cref="AddObject"/>, providing a
        /// result code and, if successful, the created 3D object
        /// </summary>
        public event ObjectCallbackArgs CallbackObjectCreate;
        /// <summary>
        /// Fired after a call to the asynchronous <see cref="ChangeObject"/>, providing a
        /// result code and, if successful, the affected 3D object
        /// </summary>
        public event ObjectCallbackArgs CallbackObjectChange;
        /// <summary>
        /// Fired after a call to the asynchronous <see cref="DeleteObject(int)"/>,
        /// providing a result code and, if successful, the deleted object's server-
        /// provided unique ID
        /// </summary>
        public event ObjectDeleteCallbackArgs CallbackObjectDelete;
        /// <summary>
        /// Fired after a call to the asynchronous <see cref="GetObject(int)"/>,
        /// providing a result code and, if successful, a <see cref="VPObject"/> of the
        /// object's details
        /// </summary>
        public event ObjectGetCallbackArgs CallbackObjectGet;
        #endregion

        #region Event handlers
        internal void OnQueryCellEnd(IntPtr sender)
        {
            if (QueryCellEnd == null) return;
            var x = Functions.vp_int(sender, IntAttributes.CellX);
            var z = Functions.vp_int(sender, IntAttributes.CellZ);
            QueryCellEnd(instance, x, z);
        }

        internal void OnObjectClick(IntPtr sender)
        {
            if (ObjectClick == null) return;
            ObjectClick click;

            lock (instance.mutex)
                click = new ObjectClick(sender);

            ObjectClick(instance, click);
        }

        /// <summary>
        /// Note: The native VP SDK uses the ObjectCreate event for query cell results
        /// </summary>
        internal void OnObjectCreate(IntPtr sender)
        {
            if (ObjectCreate == null && QueryCellResult == null) return;
            VPObject vpObject;
            int sessionId;

            lock (instance.mutex)
            {
                sessionId = Functions.vp_int(sender, IntAttributes.AvatarSession);
                vpObject  = new VPObject(instance.pointer);
            }

            if      (sessionId == -1 && QueryCellResult != null)
                QueryCellResult(instance, vpObject);
            else if (ObjectCreate != null)
                ObjectCreate(instance, sessionId, vpObject);
        }

        internal void OnObjectChange(IntPtr sender)
        {
            if (ObjectChange == null) return;
            VPObject vpObject;
            int sessionId;

            lock (instance.mutex)
            {
                vpObject  = new VPObject(instance.pointer);
                sessionId = Functions.vp_int(sender, IntAttributes.AvatarSession);
            }

            ObjectChange(instance, sessionId, vpObject);
        }

        internal void OnObjectDelete(IntPtr sender)
        {
            if (ObjectDelete == null) return;
            int session;
            int objectId;

            lock (instance.mutex)
            {
                session = Functions.vp_int(sender, IntAttributes.AvatarSession);
                objectId = Functions.vp_int(sender, IntAttributes.ObjectId);
            }

            ObjectDelete(instance, session, objectId);
        }
        #endregion

        #region Callback handlers
        void OnObjectCreateCallback(IntPtr sender, int rc, int reference)
        {
            var obj = objReferences[reference];
            objReferences.Remove(reference);

            if (CallbackObjectCreate == null)
                return;

            obj.Id = Functions.vp_int(sender, IntAttributes.ObjectId);

            CallbackObjectCreate(instance, (ReasonCode) rc, obj);
        }

        void OnObjectChangeCallback(IntPtr sender, int rc, int reference)
        {
            var obj = objReferences[reference];
            objReferences.Remove(reference);

            if (CallbackObjectChange == null)
                return;

            CallbackObjectChange(instance, (ReasonCode) rc, obj);
        }

        void OnObjectDeleteCallback(IntPtr sender, int rc, int reference)
        {
            var id = idReferences[reference];
            idReferences.Remove(reference);

            if (CallbackObjectDelete == null)
                return;

            CallbackObjectDelete(instance, (ReasonCode) rc, id);
        }

        void OnObjectGetCallback(IntPtr sender, int rc, int reference)
        {
            var id = idReferences[reference];
            idReferences.Remove(reference);

            if (CallbackObjectGet == null)
                return;

            var obj = new VPObject(sender);
            obj.Id = id;

            CallbackObjectGet(instance, (ReasonCode) rc, obj);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Queries a cell for all 3D objects contained within. Thread-safe.
        /// </summary>
        public void QueryCell(int cellX, int cellZ)
        {
            lock (instance.mutex)
                Functions.Call( () => Functions.vp_query_cell(instance.pointer, cellX, cellZ) );
        }

        /// <summary>
        /// Gets the attributes of a single object by its ID. Asyncrhonous and
        /// thread-safe. Responds using <see cref="CallbackObjectGet"/>
        /// </summary>
        /// <param name="id">ID of the object to query</param>
        public void GetObject(int id)
        {
            lock (instance.mutex)
            {
                var referenceNumber = nextReference;
                idReferences.Add(referenceNumber, id);

                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);

                try { Functions.Call( () => Functions.vp_object_get(instance.pointer, id) ); }
                catch
                {
                    idReferences.Remove(referenceNumber);
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds a raw <see cref="VPObject"/> to the world. Asynchronous and thread-safe.
        /// </summary>
        /// <param name="obj">Instance of VPObject with model and position pre-set</param>
        public void AddObject(VPObject obj)
        {
            lock (instance.mutex)
            {
                var referenceNumber = nextReference;
                objReferences.Add(referenceNumber, obj);

                obj.ToNative(instance.pointer);
                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);

                try { Functions.Call( () => Functions.vp_object_add(instance.pointer) ); }
                catch
                {
                    objReferences.Remove(referenceNumber);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates and adds a new <see cref="VPObject"/> with a specified model name,
        /// position and rotation. Asynchronous and thread-safe.
        /// </summary>
        /// <param name="model">Model name</param>
        /// <param name="position"><see cref="Vector3D"/> position</param>
        /// <param name="rotation"><see cref="Quaternion"/> rotation</param>
        public void CreateObject(string model, Vector3D position, Quaternion rotation)
        {
            AddObject( new VPObject(model, position, rotation) );
        }

        /// <summary>
        /// Creates and adds a new <see cref="VPObject"/> with a specified model name,
        /// position and default rotation. Asynchronous and thread-safe.
        /// </summary>
        /// <param name="model">Model name</param>
        /// <param name="position"><see cref="Vector3D"/> position</param>
        public void CreateObject(string model, Vector3D position)
        {
            AddObject( new VPObject(model, position) );
        }

        /// <summary>
        /// Changes an object in-world using a <see cref="VPObject"/> with newer state
        /// and the target unique ID. Asynchronous and thread-safe.
        /// </summary>
        public void ChangeObject(VPObject obj)
        {
            lock (instance.mutex)
            {
                var referenceNumber = nextReference;
                objReferences.Add(referenceNumber, obj);

                obj.ToNative(instance.pointer);
                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);

                try { Functions.Call( () => Functions.vp_object_change(instance.pointer) ); }
                catch
                {
                    objReferences.Remove(referenceNumber);
                    throw;
                }
            }
        }

        /// <summary>
        /// Attempts to delete an object by unique ID. Asynchronous and thread-safe.
        /// </summary>
        public void DeleteObject(int id)
        {
            lock (instance.mutex)
            {
                var referenceNumber = nextReference;
                idReferences.Add(referenceNumber, id);

                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);
                Functions.vp_int_set(instance.pointer, IntAttributes.ObjectId,        id);

                try { Functions.Call( () => Functions.vp_object_delete(instance.pointer) ); }
                catch
                {
                    idReferences.Remove(referenceNumber);
                    throw;
                }
            }
        }

        /// <summary>
        /// Attempts to delete an object in-world using the unique ID of a given
        /// <see cref="VPObject"/>.  Asynchronous and thread-safe.
        /// </summary>
        public void DeleteObject(VPObject obj)
        {
            DeleteObject(obj.Id);
        }

        /// <summary>
        /// Sends a click event on a given in-world object by unique ID, on the specified
        /// coordinates using a <see cref="Vector3D"/>. Thread-safe.
        /// </summary>
        /// <param name="id">ID of object to click</param>
        /// <param name="coordinates">3D coordinates of the click</param>
        /// <param name="eventTarget">Optional session to limit event to</param>
        public void ClickObject(int id, Vector3D coordinates, int eventTarget = 0)
        {
            lock (instance.mutex)
            {
                coordinates.ToClick(instance.pointer);
                Functions.vp_int_set(instance.pointer, IntAttributes.ObjectId, id);
                Functions.Call( () => Functions.vp_object_click(instance.pointer) );
            }
        }

        /// <summary>
        /// Sends a click event on a given in-world object by unique ID. Thread-safe.
        /// </summary>
        /// <param name="id">ID of object to click</param>
        /// <param name="eventTarget">Optional session to limit event to</param>
        public void ClickObject(int id, int eventTarget = 0)
        {
            ClickObject(id, Vector3D.Zero, eventTarget);
        }

        /// <summary>
        /// Sends a click event on a given in-world object on the specified coordinates
        /// using a <see cref="Vector3D"/>. Thread-safe.
        /// </summary>
        /// <param name="obj">Object to click</param>
        /// <param name="coordinates">3D coordinates of the click</param>
        /// <param name="eventTarget">Optional session to limit event to</param>
        public void ClickObject(VPObject obj, Vector3D coordinates, int eventTarget = 0)
        {
            ClickObject(obj.Id, coordinates, eventTarget);
        } 

        /// <summary>
        /// Sends a click event on a given in-world object. Thread-safe.
        /// </summary>
        /// <param name="obj">Object to click</param>
        /// <param name="eventTarget">Optional session to limit event to</param>
        public void ClickObject(VPObject obj, int eventTarget = 0)
        {
            ClickObject(obj.Id, eventTarget);
        } 
        #endregion
    }
}
