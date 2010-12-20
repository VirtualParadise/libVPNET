using System;
using System.Runtime.InteropServices;

namespace VpNet.NativeApi
{
    public class Functions
    {
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern int vp_init(int build);

        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr vp_create();
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern int vp_destroy(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        public static extern int vp_connect_universe(IntPtr instance, string host, int port);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        public static extern int vp_login(IntPtr instance, string username, string password, string botname);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern int vp_wait(IntPtr instance, int time);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        public static extern int vp_enter(IntPtr instance, string worldname);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern int vp_say(IntPtr instance, string message);
        
        [DllImport("vpsdk", CallingConvention=CallingConvention.Cdecl)]
        public static extern int vp_event_set(IntPtr instance, int eventName, EventDelegate eventname);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vp_user_data(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vp_user_data_set(IntPtr instance, IntPtr data);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_state_change(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_int(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern float vp_float(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string vp_string(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr vp_data(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, out int length);
        //public static extern int vp_int_get(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, int* value);
        //public static extern int vp_float_get(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, float* value);
        //[DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        //public static extern int vp_string_get(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, char** value);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_int_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, int value);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_float_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, float value);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void vp_string_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, string value);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_data_set(IntPtr instance, [MarshalAs(UnmanagedType.I4)]Attribute name, int length, IntPtr data);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_query_cell(IntPtr instance, int x, int z);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_object_add(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_object_change(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_object_delete(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_object_click(IntPtr instance);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_world_list(IntPtr instance, int time);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_user_attributes_by_id(IntPtr instance, int userId);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int vp_user_attributes_by_name(IntPtr instance, string name);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_friends_get(IntPtr instance);

        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int vp_friend_add_by_name(IntPtr instance, string name);
        
        [DllImport("vpsdk", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vp_friend_delete(IntPtr instance, int friendId);

        public static byte[] GetData(IntPtr instance, Attribute attribute)
        {
            int length;
            var ptr = vp_data(instance, attribute, out length);
            var result = new byte[length];
            Marshal.Copy(ptr, result, 0, length);
            return result;
        }
    }
}
