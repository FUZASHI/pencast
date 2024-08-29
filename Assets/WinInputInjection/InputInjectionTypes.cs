#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace InputInjectionWIN32{
    public enum INPUT_TYPE
    {
        MOUSE,
        KEYBOARD,
        //not used
        //HARDWARE_INPUT 
    }
    public enum KEYEVENTF : uint
    {
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Unicode = 0x0004,
        ScanCode = 0x0008,
    }

    //https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    public enum KEYCODE_WIN : ushort
    {
        #region MOUSE
        MOUSE_LEFT = 0x01,
        MOUSE_RIGHT = 0x02,
        MOUSE_MIDDLE = 0x04,
        
        #endregion
        #region KEYS
		KEY_0 = 0x30,
		KEY_1 = 0x31,
		KEY_2 = 0x32,
		KEY_3 = 0x33,
		KEY_4 = 0x34,
		KEY_5 = 0x35,
		KEY_6 = 0x36,
		KEY_7 = 0x37,
		KEY_8 = 0x38,
		KEY_9 = 0x39,
		KEY_A = 0x41,
		KEY_B = 0x42,
		KEY_C = 0x43,
		KEY_D = 0x44,
		KEY_E = 0x45,
		KEY_F = 0x46,
		KEY_G = 0x47,
		KEY_H = 0x48,
		KEY_I = 0x49,
		KEY_J = 0x4a,
		KEY_K = 0x4b,
		KEY_L = 0x4c,
		KEY_M = 0x4d,
		KEY_N = 0x4e,
		KEY_O = 0x4f,
		KEY_P = 0x50,
		KEY_Q = 0x51,
		KEY_R = 0x52,
		KEY_S = 0x53,
		KEY_T = 0x54,
		KEY_U = 0x55,
		KEY_V = 0x56,
		KEY_W = 0x57,
		KEY_X = 0x58,
		KEY_Y = 0x59,
		KEY_Z = 0x5a,
		#endregion
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT 
    {
        public ushort wVk;
        public ushort wScan;
        public KEYEVENTF dwFlags;
        public uint time;
        public IntPtr ExtraInfo;
    }
    public enum MOUSEEVENTF : uint
    {
        ABSOLUTE = 0x8000,
        MOVE = 0x0001,
        VIRTUALDESK = 0x4000,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        XDOWN = 0x0080,
        XUP = 0x0100,
        MOVE_NOCOALESCE = 0x2000
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public IntPtr ExtraInfo;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_UNION
    {
        [FieldOffset(0)]
        public KEYBDINPUT keyboard;
        [FieldOffset(0)]
        public MOUSEINPUT mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public INPUT_TYPE type;
        public INPUT_UNION union;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }

}
#endif