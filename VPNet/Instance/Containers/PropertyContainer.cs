using Nexus;
using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container class for Instance's property-related members
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
        }
        #endregion

        #region Object references
        Dictionary<int, VPObject> references = new Dictionary<int, VPObject>();

        int _nextRef = int.MinValue;
        int nextReference
        {
            get
            {
                lock (instance.mutex)
                    if (_nextRef < int.MaxValue)
                        _nextRef++;
                    else
                        _nextRef = int.MinValue;

                return _nextRef;
            }
        } 
        #endregion

        #region Events and callbacks
        public delegate void QueryCellResultArgs(Instance sender, VPObject objectData);
        public delegate void QueryCellEndArgs(Instance sender, int x, int z);
        public delegate void ObjectChangeArgs(Instance sender, int sessionId, VPObject objectData);
        public delegate void ObjectCreateArgs(Instance sender, int sessionId, VPObject objectData);
        public delegate void ObjectDeleteArgs(Instance sender, int sessionId, int objectId);
        public delegate void ObjectClickArgs(Instance sender, ObjectClick click);

        public delegate void ObjectCallbackArgs(Instance sender, ObjectCallbackData args);

        public event QueryCellResultArgs QueryCellResult;
        public event QueryCellEndArgs    QueryCellEnd;
        public event ObjectChangeArgs    ObjectCreate;
        public event ObjectChangeArgs    ObjectChange;
        public event ObjectDeleteArgs    ObjectDelete;
        public event ObjectClickArgs     ObjectClick;

        public event ObjectCallbackArgs CallbackObjectCreate;
        public event ObjectCallbackArgs CallbackObjectChange;
        public event ObjectCallbackArgs CallbackObjectDelete;
        #endregion

        #region Methods
        /// <summary>
        /// Queries a cell for objects and fires QueryCellResult for each object returned
        /// and QueryCellEnd when finished
        /// </summary>
        /// <param name="cellX"></param>
        /// <param name="cellZ"></param>
        public void QueryCell(int cellX, int cellZ)
        {
            int rc;
            lock (instance.mutex)
                rc = Functions.vp_query_cell(instance.pointer, cellX, cellZ);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Adds a raw vpObject to the world
        /// </summary>
        /// <param name="vpObject">New instance of vpObject with model and position set</param>
        public void AddObject(VPObject vpObject)
        {
            int rc;
            int referenceNumber;

            lock (instance.mutex)
            {
                referenceNumber = nextReference;
                references.Add(referenceNumber, vpObject);
                vpObject.ToNative(instance.pointer);

                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);
                rc = Functions.vp_object_add(instance.pointer);
            }

            if (rc != 0)
            {
                references.Remove(referenceNumber);
                throw new VPException((ReasonCode)rc);
            }
        }

        /// <summary>
        /// Creates and adds a new vpObject
        /// </summary>
        /// <param name="model">Model name</param>
        /// <param name="position">Vector3D position</param>
        /// <param name="rotation">Quaternion rotation</param>
        public void AddObject(string model, Vector3D position, Quaternion rotation)
        {
            AddObject(new VPObject
            {
                Model = model,
                Position = position,
                Rotation = rotation
            });
        }

        /// <summary>
        /// Creates and adds a new vpObject with default rotation
        /// </summary>
        /// <param name="model">Model name</param>
        /// <param name="position">Vector3D position</param>
        public void AddObject(string model, Vector3D position)
        {
            AddObject(model, position, Quaternion.Zero);
        }

        public void ChangeObject(VPObject vpObject)
        {
            int rc;
            int referenceNumber;

            lock (instance.mutex)
            {
                referenceNumber = nextReference;
                references.Add(referenceNumber, vpObject);
                vpObject.ToNative(instance.pointer);

                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);
                rc = Functions.vp_object_change(instance.pointer);
            }

            if (rc != 0)
            {
                references.Remove(referenceNumber);
                throw new VPException((ReasonCode)rc);
            }
        }

        /// <summary>
        /// Deletes a given object
        /// </summary>
        /// <param name="vpObject">Object to delete</param>
        public void DeleteObject(VPObject vpObject)
        {
            int rc;
            int referenceNumber;

            lock (instance.mutex)
            {
                referenceNumber = nextReference;
                references.Add(referenceNumber, vpObject);

                Functions.vp_int_set(instance.pointer, IntAttributes.ReferenceNumber, referenceNumber);
                Functions.vp_int_set(instance.pointer, IntAttributes.ObjectId, vpObject.Id);
                rc = Functions.vp_object_delete(instance.pointer);
            }

            if (rc != 0)
            {
                references.Remove(referenceNumber);
                throw new VPException((ReasonCode)rc);
            }
        }

        /// <summary>
        /// Attempts to delete an object by ID
        /// 
        /// TODO: seems wasteful to create new VPObject; investigate nessecity
        /// </summary>
        /// <param name="id">ID of object to delete</param>
        public void DeleteObject(int id)
        {
            DeleteObject( new VPObject { Id = id } );
        }

        /// <summary>
        /// Sends a click event on a given object
        /// </summary>
        /// <param name="vpObject">Object to click</param>
        public void ClickObject(VPObject vpObject)
        {
            int rc;
            lock (instance.mutex)
            {
                vpObject.ToNative(instance.pointer);
                rc = Functions.vp_object_click(instance.pointer);
            }

            if (rc != 0) throw new VPException((ReasonCode)rc);
        } 
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
            if (CallbackObjectCreate == null) return;

            lock (instance.mutex)
            {
                var vpObject = references[reference];
                references.Remove(reference);

                vpObject.Id = Functions.vp_int(sender, IntAttributes.ObjectId);
                CallbackObjectCreate(instance, new ObjectCallbackData((ReasonCode)rc, vpObject));
            }
        }

        void OnObjectChangeCallback(IntPtr sender, int rc, int reference)
        {
            if (CallbackObjectChange == null) return;

            lock (instance.mutex)
            {
                var vpObject = references[reference];
                references.Remove(reference);

                CallbackObjectChange(instance, new ObjectCallbackData((ReasonCode)rc, vpObject));
            }
        }

        void OnObjectDeleteCallback(IntPtr sender, int rc, int reference)
        {
            if ( CallbackObjectDelete == null ) return;

            lock (instance.mutex)
            {
                var vpObject = references[reference];
                references.Remove(reference);

                if (CallbackObjectDelete != null)
                    CallbackObjectDelete(instance, new ObjectCallbackData((ReasonCode)rc, vpObject));
            }
        }
        #endregion
    }
}
