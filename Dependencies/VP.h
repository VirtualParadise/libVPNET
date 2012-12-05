#ifndef __VP_SDK
#define __VP_SDK

#if defined(WIN32) || defined(UNDER_CE)
#   ifdef VPSDK_EXPORTS
#       ifdef VPSDK_STATIC
#           define VPSDK_API extern "C"
#       else
#           define VPSDK_API extern "C" __declspec(dllexport)
#       endif
#   else
#       ifdef __cplusplus
#           ifdef VPSDK_STATIC
#               define VPSDK_API extern "C"
#           else
#               define VPSDK_API extern "C" __declspec(dllimport)
#           endif
#       else
#           ifdef VPSDK_STATIC
#               define VPSDK_API extern
#           else
#               define VPSDK_API __declspec(dllimport)
#           endif
#       endif
#   endif
#else
#   if defined(VPSDK_EXPORTS) && (__GNUC__ >= 4)
#       ifdef __cplusplus
#           define VPSDK_API extern "C" __attribute__ ((visibility ("default")))
#       else
#           define VPSDK_API extern __attribute__ ((visibility ("default")))
#       endif
#   else
#       ifdef __cplusplus
#           define VPSDK_API extern "C"
#       else
#           define VPSDK_API extern
#       endif
#   endif
#endif

/* API Version */
#define VPSDK_VERSION 1

/**
 *	Events can be registered using #vp_event_set
 */
typedef enum VPEvent
{
/**
 *	Called when a user in the same world the instance is in sends a message
 *  Attributes:
 *  - #VP_AVATAR_SESSION
 *  - #VP_AVATAR_NAME
 *  - #VP_CHAT_MESSAGE
 */
	VP_EVENT_CHAT,
/**
 *	Called when a user enters the world
 *  Attributes:
 *  - #VP_AVATAR_NAME
 *  - #VP_AVATAR_SESSION
 *  - #VP_AVATAR_X
 *  - #VP_AVATAR_Y
 *  - #VP_AVATAR_Z
 *  - #VP_AVATAR_ROTATION_PITCH
 *  - #VP_AVATAR_ROTATION_YAW
 *  - #VP_AVATAR_TYPE
 */
	VP_EVENT_AVATAR_ADD,
/**
 *	Called when a users' avatar changes shape or position
 *  Attributes:
 *  - #VP_AVATAR_NAME
 *  - #VP_AVATAR_SESSION
 *  - #VP_AVATAR_X
 *  - #VP_AVATAR_Y
 *  - #VP_AVATAR_Z
 *  - #VP_AVATAR_ROTATION_PITCH
 *  - #VP_AVATAR_ROTATION_YAW
 *  - #VP_AVATAR_TYPE
 */
	VP_EVENT_AVATAR_CHANGE,
/**
 *	Called when a user leaves the world
 *  Attributes:
 *  - #VP_AVATAR_NAME
 *  - #VP_AVATAR_SESSION
 */
	VP_EVENT_AVATAR_DELETE,
/**
 *	Called when a new object is created or sent as a result to a query. The
 *	following attributes are set when the event is called:
 *		- #VP_OBJECT_ID
 *		- #VP_OBJECT_USER_ID
 *		- #VP_OBJECT_TIME
 *		- #VP_OBJECT_X
 *		- #VP_OBJECT_Y
 *		- #VP_OBJECT_Z
 *		- #VP_OBJECT_ROTATION_X
 *		- #VP_OBJECT_ROTATION_Y
 *		- #VP_OBJECT_ROTATION_Z
 *		- #VP_OBJECT_ROTATION_ANGLE
 *		- #VP_OBJECT_TYPE
 *		- #VP_OBJECT_DATA
 *		- #VP_OBJECT_MODEL
 *		- #VP_OBJECT_DESCRIPTION
 *		- #VP_OBJECT_ACTION
 */
	VP_EVENT_OBJECT,
/**
 *	Called when an object is changed by another user. The following attributes
 *	are set when the event is called:
 *		- #VP_OBJECT_ID
 *		- #VP_OBJECT_USER_ID
 *		- #VP_OBJECT_TIME
 *		- #VP_OBJECT_X
 *		- #VP_OBJECT_Y
 *		- #VP_OBJECT_Z
 *		- #VP_OBJECT_ROTATION_X
 *		- #VP_OBJECT_ROTATION_Y
 *		- #VP_OBJECT_ROTATION_Z
 *		- #VP_OBJECT_ROTATION_ANGLE
 *		- #VP_OBJECT_TYPE
 *		- #VP_OBJECT_DATA
 *		- #VP_OBJECT_MODEL
 *		- #VP_OBJECT_DESCRIPTION
 *		- #VP_OBJECT_ACTION
 */
	VP_EVENT_OBJECT_CHANGE,

/**
 *	Called when an object is deleted another user. The #VP_OBJECT_ID attribute
 *	is set when the event is called.
 *  Attributes:
 *  - #VP_AVATAR_SESSION
 *  - #VP_OBJECT_ID
 */
	VP_EVENT_OBJECT_DELETE,

/**
 *	Called when an object is clicked by another user. The following attributes
 *	are set when the event is called:
 *		- #VP_AVATAR_SESSION
 *		- #VP_OBJECT_ID
 */
	VP_EVENT_OBJECT_CLICK,

	/**
	 *  Attributes:
	 *	Called when a world changes state or when the world list is requested using
	 *	#vp_world_list. 
	 *		- #VP_WORLD_NAME
	 *		- #VP_WORLD_USERS
	 *		- #VP_WORLD_STATE
	 */
	VP_EVENT_WORLD_LIST,

	/**
	 *  Attributes:
	 *  - #VP_WORLD_SETTING_KEY
	 *  - #VP_WORLD_SETTING_VALUE
	 */
	VP_EVENT_WORLD_SETTING,

	/**
	 *  Attributes:
	 *  Called when the server is done sending world settings
	 */
	VP_EVENT_WORLD_SETTINGS_CHANGED,

	VP_EVENT_FRIEND,
	VP_EVENT_WORLD_DISCONNECT,
	VP_EVENT_UNIVERSE_DISCONNECT,

	/**
	 *  Attributes:
	 *  - #VP_USER_ID
	 *  - #VP_USER_REGISTRATION_TIME
	 *  - #VP_USER_ONLINE_TIME
	 *  - #VP_USER_LAST_LOGIN
	 *  - #VP_USER_NAME
	 *  - #VP_USER_EMAIL
	 */
	VP_EVENT_USER_ATTRIBUTES,
	
	/**
	 *  Attributes:
	 *  - #VP_CELL_X
	 *  - #VP_CELL_Z
	 */
	VP_EVENT_CELL_END,
    
	/**
	 *  Attributes:
	 *  - #VP_TERRAIN_NODE_DATA
	 *  - #VP_TERRAIN_NODE_X
	 *  - #VP_TERRAIN_NODE_Z
	 *  - #VP_TERRAIN_NODE_REVISION
	 *  - #VP_TERRAIN_TILE_X
	 *  - #VP_TERRAIN_TILE_Z
	 */
    VP_EVENT_TERRAIN_NODE,
	
	VP_HIGHEST_EVENT
} VPEvent;

typedef enum VPCallback
{
	/**
	 *  The attribute #VP_OBJECT_ID is set to the object ID of the new object
	 */
	VP_CALLBACK_OBJECT_ADD,
	VP_CALLBACK_OBJECT_CHANGE,
	VP_CALLBACK_OBJECT_DELETE,
	VP_CALLBACK_GET_FRIENDS,
	VP_CALLBACK_FRIEND_ADD,
	VP_CALLBACK_FRIEND_DELETE,
    VP_CALLBACK_TERRAIN_QUERY,
    VP_CALLBACK_TERRAIN_NODE_SET,
	VP_HIGHEST_CALLBACK
} VPCallback;

/* Ints */
typedef enum VPIntegerProperty
{
	VP_AVATAR_SESSION,
	VP_AVATAR_TYPE,
	VP_MY_TYPE,
	
	VP_OBJECT_ID,
	VP_OBJECT_TYPE,
	VP_OBJECT_TIME,
	VP_OBJECT_USER_ID,
	
	VP_WORLD_STATE,
	VP_WORLD_USERS,
	
	VP_REFERENCE_NUMBER,
	VP_CALLBACK,
	
	VP_USER_ID,
	VP_USER_REGISTRATION_TIME,
	VP_USER_ONLINE_TIME,
	VP_USER_LAST_LOGIN,
	
	VP_FRIEND_ID,
	VP_FRIEND_USER_ID,
	VP_FRIEND_ONLINE,
	
	VP_MY_USER_ID,
	VP_PROXY_TYPE,
	VP_PROXY_PORT,
	
	VP_CELL_X,
	VP_CELL_Z,
    
    VP_TERRAIN_TILE_X,
    VP_TERRAIN_TILE_Z,
    VP_TERRAIN_NODE_X,
    VP_TERRAIN_NODE_Z,
    VP_TERRAIN_NODE_REVISION,
	
	VP_HIGHEST_INT
} VPIntegerProperty;

/* Floats */
typedef enum VPFloatProperty
{
	VP_AVATAR_X,
	VP_AVATAR_Y,
	VP_AVATAR_Z,
	VP_AVATAR_YAW,
	VP_AVATAR_PITCH,
	
	VP_MY_X,
	VP_MY_Y,
	VP_MY_Z,
	VP_MY_YAW,
	VP_MY_PITCH,
	
	VP_OBJECT_X,
	VP_OBJECT_Y,
	VP_OBJECT_Z,
	VP_OBJECT_ROTATION_X,
	VP_OBJECT_ROTATION_Y,
	VP_OBJECT_ROTATION_Z,
	/**
	 * Only works correctly when #VP_OBJECT_ROTATION_ANGLE is set to infinity
	 * \deprecated Use #VP_OBJECT_ROTATION_X, #VP_OBJECT_ROTATION_Y, #VP_OBJECT_ROTATION_Z and #VP_OBJECT_ROTATION_ANGLE
	 */
	VP_OBJECT_YAW = VP_OBJECT_ROTATION_X,
	/**
	 * Only works correctly when #VP_OBJECT_ROTATION_ANGLE is set to infinity
	 * \deprecated Use #VP_OBJECT_ROTATION_X, #VP_OBJECT_ROTATION_Y, #VP_OBJECT_ROTATION_Z and #VP_OBJECT_ROTATION_ANGLE
	 */
	VP_OBJECT_PITCH = VP_OBJECT_ROTATION_Y,
	/**
	 * Only works correctly when #VP_OBJECT_ROTATION_ANGLE is set to infinity
	 * \deprecated Use #VP_OBJECT_ROTATION_X, #VP_OBJECT_ROTATION_Y, #VP_OBJECT_ROTATION_Z and #VP_OBJECT_ROTATION_ANGLE
	 */
	VP_OBJECT_ROLL = VP_OBJECT_ROTATION_Z,
	VP_OBJECT_ROTATION_ANGLE,
	
	VP_HIGHEST_FLOAT
} VPFloatProperty;

/* Strings */
typedef enum VPStringProperty
{
	VP_AVATAR_NAME,
	VP_CHAT_MESSAGE,
	
	VP_OBJECT_MODEL,
	VP_OBJECT_ACTION,
	VP_OBJECT_DESCRIPTION,
	
	VP_WORLD_NAME,
	
	VP_USER_NAME,
	VP_USER_EMAIL,
	
	VP_WORLD_SETTING_KEY,
	VP_WORLD_SETTING_VALUE,
	
	VP_FRIEND_NAME,
	VP_PROXY_HOST,
	
	VP_HIGHEST_STRING
} VPStringProperty;

/* Data */
typedef enum VPDataProperty
{
	VP_OBJECT_DATA,
    VP_TERRAIN_NODE_DATA,
	VP_HIGHEST_DATA
} VPDataProperty;

/**
 *	Proxy types
 */
typedef enum VPProxyType {
	VP_PROXY_TYPE_NONE,
	VP_PROXY_TYPE_SOCKS4A
} VPProxyType;

#ifdef __cplusplus
namespace vpsdk {
    struct VPInstance_;
}

typedef vpsdk::VPInstance_ *VPInstance;
#else
typedef void *VPInstance;
#endif

typedef void(*VPEventHandler)(VPInstance);
typedef void(*VPCallbackHandler)(VPInstance, int, int);

struct vp_terrain_cell_t {
    float height;
    
    /* 1 bit visibility, 2 bits rotation, 1 bit padding, 12 bits texture */
    unsigned short attributes;
};

#define VP_PACK_TERRAIN_ATTRIBUTES(tex, rotation, visible) ((tex & 0x1FFF) |   \
                                                            (visible << 15) |  \
                                                            (rotation << 13))

#define VP_UNPACK_TERRAIN_VISIBILITY(x) ((x & 0x8000) >> 15)
#define VP_UNPACK_TERRAIN_ROTATION(x) ((x & 0x6000) >> 13)
#define VP_UNPACK_TERRAIN_TEXTURE(x) (x & 0x0FFF)

/**
 *  Initialize the Virtual Paradise SDK API
 */
VPSDK_API int vp_init(int version);

/**
 *  Create a new instance.
 *  \return New instance or NULL on failure.
 */
VPSDK_API VPInstance vp_create(void);

/**
 *  Destroy a Virtual Paradise SDK instance.
 */
VPSDK_API int vp_destroy(VPInstance instance);

/**
 *  Connect to a universe server
 *  \param instance
 *  \param host Host address of server to connect to.
 *  \param port TCP port of remote server.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_connect_universe(VPInstance instance, const char * host, int port);

/**
 *  Login to the universe server
 *  \param instance
 *  \param username
 *  \param password
 *  \param botname
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_login(VPInstance instance, const char * username, const char * password, const char * botname);

/**
 *  Wait for incoming messages.
 *  \param milliseconds The maximum time to wait in milliseconds.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_wait(VPInstance instance, int milliseconds);

/**
 *  Enter a world. The current world will be left.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_enter(VPInstance instance, const char * worldname);

/**
 *	Leave the current world.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_leave(VPInstance instance);

/**
 *  Send a message to everyone in the current world.
 *  \param message The message to send.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_say(VPInstance instance, const char * message);

/**
 *  Register an event handler.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_event_set(VPInstance instance, VPEvent eventname, VPEventHandler event);

/**
 *  Register a callback function.
 *  \return Zero when successful, otherwise nonzero. See RC.h
 */
VPSDK_API int vp_callback_set(VPInstance instance, VPCallback callbackname, VPCallbackHandler callback);

/**
 *  Retrieve the pointer to user-defined data for this instance.
 *  \return Pointer to user-defined data.
 */
VPSDK_API void * vp_user_data(VPInstance instance);

/**
 *  Sets a pointer to user-defined data for this instance.
 *  This pointer is not accessed in any way, allocating and freeing it is the responsibility of the application programmer.
 *  \param data The pointer to your user-defined data.
 */
VPSDK_API void vp_user_data_set(VPInstance instance, void * data);
VPSDK_API int vp_state_change(VPInstance instance);

VPSDK_API int vp_int(VPInstance instance, VPIntegerProperty name);
VPSDK_API float vp_float(VPInstance instance, VPFloatProperty name);
VPSDK_API const char* vp_string(VPInstance instance, VPStringProperty name);
VPSDK_API const char* vp_data(VPInstance instance, VPDataProperty name, int* length);

VPSDK_API int vp_int_get(VPInstance instance, VPIntegerProperty name, int* value);
VPSDK_API int vp_float_get(VPInstance instance, VPFloatProperty name, float* value);
VPSDK_API int vp_string_get(VPInstance instance, VPStringProperty name, char** value);

VPSDK_API int vp_int_set(VPInstance instance, VPIntegerProperty name, int value);
VPSDK_API int vp_float_set(VPInstance instance, VPFloatProperty name, float value);
VPSDK_API void vp_string_set(VPInstance instance, VPStringProperty name, const char * str);
VPSDK_API int vp_data_set(VPInstance instance, VPDataProperty name, int length, char* data);

/**
 *	Query the objects in a single cell
 *	Each object will be sent in a #VP_EVENT_OBJECT event. After all the objects
 *  for the cell have been sent, a #VP_EVENT_CELL_END event will be raised.
 */
VPSDK_API int vp_query_cell(VPInstance instance, int x, int z);

VPSDK_API int vp_object_add(VPInstance instance);
VPSDK_API int vp_object_change(VPInstance instance);
VPSDK_API int vp_object_delete(VPInstance instance);
VPSDK_API int vp_object_click(VPInstance instance);

/**
 *  Request the world list.
 *  The worlds will be listed in the #VP_EVENT_WORLD_LIST event. See vp_event_set().
 *  \param time Time since your last update. This is not used yet, the whole list will be requested.
 */
VPSDK_API int vp_world_list(VPInstance instance, int time);

/* VPSDK_API void* vp_callback_pointer(VPInstance instance); */
/* VPSDK_API void vp_callback_pointer_set(VPInstance instance, void* ptr); */

/**
 *  Request user attributes by user ID.
 *  The user attributes will be returned in the #VP_EVENT_USER_ATTRIBUTES event.
 *  \return Zero when successful, otherwise nonzero. This value will return the result
 */
VPSDK_API int vp_user_attributes_by_id(VPInstance instance, int user_id);

/**
 *  Get user attributes by user name. Not implemented.
 */
VPSDK_API int vp_user_attributes_by_name(VPInstance instance, const char * name);

VPSDK_API int vp_friends_get(VPInstance instance);
VPSDK_API int vp_friend_add_by_name(VPInstance instance, const char* name);
VPSDK_API int vp_friend_delete(VPInstance instance, int friend_id);

VPSDK_API int vp_terrain_query(VPInstance instance, int tile_x, int tile_z, int revision[][4]);
VPSDK_API int vp_terrain_node_set(VPInstance instance, 
                                  int tile_x, int tile_z, 
                                  int node_x, int node_z, 
                                  struct vp_terrain_cell_t* cells);

#endif
