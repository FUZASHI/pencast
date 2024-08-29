#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace InputInjectionWIN32{
    public class InputDevice{
#region SINGLETON
        private static InputDevice instance = null;
        private InputDevice()
        {
        }

        private static InputDevice Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputDevice();
                }
                return instance;
            }
        }

        public static InputDevice Get() => Instance;
#endregion

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);
        private readonly INPUT[] mouseInputs = new INPUT[2]
        {
            //Position
            new INPUT
            {
                type = INPUT_TYPE.MOUSE,
                union = {
                    mouse = new MOUSEINPUT
                    {
                        time = 0,
                        ExtraInfo = IntPtr.Zero
                    }

                }
            },
            //Clicks
            new INPUT
            {
                type = INPUT_TYPE.MOUSE,
                union = {
                    mouse = new MOUSEINPUT
                    {
                        time = 0,
                        ExtraInfo = IntPtr.Zero
                    }

                }
            }
        };
        private readonly INPUT[] keybdInputs = new INPUT[]
        {
            new INPUT
            {
                type = INPUT_TYPE.KEYBOARD,
                union = {
                    keyboard = new KEYBDINPUT
                    {
                        time = 0,
                        ExtraInfo = IntPtr.Zero
                    }

                }
            }
        };
        
        int ConvertScreenPosX(float x)
        {
            return (int)(x * 65536);
        }
        int ConvertScreenPosY(float y)
        {
            return (int)(y * 65536);
        }
        private void UpdatePosition(float x, float y)
        {
            mouseInputs[0].union.mouse.dwFlags = MOUSEEVENTF.ABSOLUTE | MOUSEEVENTF.MOVE;
            mouseInputs[0].union.mouse.dx = ConvertScreenPosX(x);
            mouseInputs[0].union.mouse.dy = ConvertScreenPosY(y);
        }
        public void ProcessInput(KEYCODE_WIN keyCode, float x, float y, bool isDown){
            if (GetDeviceType(keyCode) == INPUT_TYPE.MOUSE){
                UpdatePosition(x, y);
                mouseInputs[1].union.mouse.dwFlags = GetMouseFlags(keyCode, isDown);
                // Inject mouse input.
                SendInput(2, mouseInputs, INPUT.Size);
            } else {
                keybdInputs[0].union.keyboard.wVk = (ushort)keyCode;
                keybdInputs[0].union.keyboard.dwFlags = isDown ? 0 : KEYEVENTF.KeyUp;

                SendInput(1, keybdInputs, INPUT.Size);
            }
        }

        INPUT_TYPE GetDeviceType(KEYCODE_WIN keyCode){
            var mouseMask = KEYCODE_WIN.MOUSE_LEFT | KEYCODE_WIN.MOUSE_MIDDLE | KEYCODE_WIN.MOUSE_RIGHT;
            if((keyCode & mouseMask) == keyCode) return INPUT_TYPE.MOUSE;
            return INPUT_TYPE.KEYBOARD;
        }
        MOUSEEVENTF GetMouseFlags(KEYCODE_WIN keyCode, bool isDown){
            switch (keyCode)
            {
                case KEYCODE_WIN.MOUSE_LEFT:
                return isDown ? MOUSEEVENTF.LEFTDOWN : MOUSEEVENTF.LEFTUP;
                case KEYCODE_WIN.MOUSE_MIDDLE:
                return isDown ? MOUSEEVENTF.MIDDLEDOWN : MOUSEEVENTF.MIDDLEUP;
                case KEYCODE_WIN.MOUSE_RIGHT:
                return isDown ? MOUSEEVENTF.RIGHTDOWN : MOUSEEVENTF.RIGHTUP;
                default:
                return isDown ? MOUSEEVENTF.RIGHTDOWN : MOUSEEVENTF.RIGHTUP;
            }
        }
    }
}
#endif