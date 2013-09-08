using System;
using System.Runtime.InteropServices;

namespace VP.Native
{
    public class Functions
    {
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_init(int build);

        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern IntPtr vp_create();
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_destroy(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_connect_universe(IntPtr instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string host, 
            int port);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_login(IntPtr instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string username,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string password,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string botname);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_wait(IntPtr instance, int time);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_enter(IntPtr instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string worldname);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_leave(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_say(IntPtr instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message);

        [DllImport(DLLHandler.VPDLL, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int vp_console_message(IntPtr instance, int session,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message,
            int effects, byte red, byte green, byte blue);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_event_set(IntPtr instance, int eventName, [MarshalAs(UnmanagedType.FunctionPtr)]EventDelegate eventFunction);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_callback_set(IntPtr instance, int callbackName, [MarshalAs(UnmanagedType.FunctionPtr)]CallbackDelegate callbackFunction);

        // Unused
        //[DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        //internal static extern IntPtr vp_user_data(IntPtr instance);
        
        // Unused
        //[DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void vp_user_data_set(IntPtr instance, IntPtr data);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_state_change(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_int(IntPtr instance, [MarshalAs(UnmanagedType.I4)]IntAttributes name);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float vp_float(IntPtr instance, [MarshalAs(UnmanagedType.I4)]FloatAttributes name);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToManaged))]
        internal static extern string vp_string(IntPtr instance, [MarshalAs(UnmanagedType.I4)]StringAttributes name);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr vp_data(IntPtr instance, [MarshalAs(UnmanagedType.I4)]DataAttributes name, out int length);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_int_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]IntAttributes name, int value);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_float_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]FloatAttributes name, float value);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void vp_string_set(IntPtr instance, 
            [MarshalAs(UnmanagedType.I4)] StringAttributes name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string value);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_data_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]DataAttributes name, int length, IntPtr data);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_query_cell(IntPtr instance, int x, int z);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_object_add(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_object_change(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_object_delete(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_object_click(IntPtr instance);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_world_list(IntPtr instance, int time);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_user_attributes_by_id(IntPtr instance, int userId);
        
        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_user_attributes_by_name(IntPtr instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);

        // Unused
        //[DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        //internal static extern int vp_friends_get(IntPtr instance);

        // Unused
        //[DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        //internal static extern int vp_friend_add_by_name(IntPtr instance,
        //    [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);
        
        // Unused
        //[DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        //internal static extern int vp_friend_delete(IntPtr instance, int friendId);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_terrain_query(IntPtr instance, int tile_x, int tile_z, int[,] nodes);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_terrain_node_set(IntPtr instance, int tile_x, int tile_z, int node_x, int node_z, byte[] node);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_avatar_click(IntPtr instance, int session);

        [DllImport(DLLHandler.VPDLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int vp_teleport_avatar(
            IntPtr instance,
            int session,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
            float x, float y, float z,
            float yaw, float pitch);

        internal static byte[] GetData(IntPtr instance, DataAttributes attribute)
        {
            int length;
            var ptr    = vp_data(instance, attribute, out length);
            var result = new byte[length];

            Marshal.Copy(ptr, result, 0, length);
            return result;
        }

        internal static void SetData(IntPtr instance, DataAttributes attribute, byte[] data)
        {
            var length = data.Length;
            var ptr    = Marshal.AllocHGlobal(length);
            Marshal.Copy(data, 0, ptr, length);

            vp_data_set(instance, attribute, length, ptr);
            Marshal.FreeHGlobal(ptr);
        }
    }
}
