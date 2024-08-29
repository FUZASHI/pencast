#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PenInjectionWIN32
{
    public class PenDevice{
        IntPtr penHandle;
        POINTER_TYPE_INFO[] myPointer;
        public event Action<bool> DeviceFoundState;

        [DllImport("user32.dll")]
        public static extern IntPtr CreateSyntheticPointerDevice(
            POINTER_INPUT_TYPE pointerType,
            ulong maxCount,
            POINTER_FEEDBACK_MODE mode
        );

        [DllImport("user32.dll")]
        public static extern bool InjectSyntheticPointerInput(
            IntPtr device,
            POINTER_TYPE_INFO[] pointerInfo,
            uint count
        );

        [DllImport("user32.dll")]
        public static extern void DestroySyntheticPointerDevice(
            IntPtr device
        );

        /// <summary>
        /// Allocates pointer device and pointer info structure for pen input injection.
        /// </summary>
        public void CreateDevice(){

            penHandle = CreateSyntheticPointerDevice(POINTER_INPUT_TYPE.PT_PEN, 1, POINTER_FEEDBACK_MODE._DEFAULT);

            var err = Marshal.GetLastWin32Error();
            if (err < 0 || penHandle == IntPtr.Zero){
                Console.WriteLine("Failed to create pointer device");
                DeviceFoundState?.Invoke(false);
                return;
            }


            CreatePointer();

            Console.WriteLine("Pointer device created");
            DeviceFoundState?.Invoke(true);

        }
        /// <summary>
        /// Allocates pointer structure.
        /// </summary>
        void CreatePointer(){

            //Basic pointer info
            var pointerInfo = new POINTER_INFO
            {
                pointerType = POINTER_INPUT_TYPE.PT_PEN,
                pointerFlags = POINTER_FLAGS.NONE,
                ptPixelLocation = new POINT(),
            };

            //Pen information
            var penInfo = new POINTER_PEN_INFO
            {
                pointerInfo = pointerInfo,
                penFlags = PEN_FLAGS.NONE,
                penMask = PEN_MASK.PRESSURE | PEN_MASK.TILT_X | PEN_MASK.TILT_Y,
                pressure = 0,
                rotation = 0,
                tiltX = 0,
                tiltY = 0
            };
            
            myPointer = new POINTER_TYPE_INFO[]
            {
                new POINTER_TYPE_INFO
                {
                    type = POINTER_INPUT_TYPE.PT_PEN,
                    penInfo = penInfo
                }
            };

        }
        /// <summary>
        /// Injects input from PenData structure using currently allocated pen injection device.
        /// </summary>
        /// <param name="penData"></param>
        public void InjectInput(PenData penData){
            if (penHandle == IntPtr.Zero){
                Console.WriteLine("Can't inject input - pointer device was not created");
                DeviceFoundState?.Invoke(false);
                return;
            }

            myPointer[0].penInfo.pointerInfo.ptPixelLocation.x = (int)(penData.x*AppSettings.Get().ScreenWidth);
            myPointer[0].penInfo.pointerInfo.ptPixelLocation.y = (int)(penData.y*AppSettings.Get().ScreenHeight);
            myPointer[0].penInfo.pointerInfo.pointerFlags = GetPointerFlags(penData.state);

            // Pen buttons are simualted with user32 [SendInput] function.
            //myPointer[0].penInfo.penFlags = penData.firstPressed ? PEN_FLAGS.BARREL : PEN_FLAGS.NONE;

            myPointer[0].penInfo.pressure = (uint)(Mathf.Clamp01(penData.pressure) * 1024);

            // Inject pen input
            InjectSyntheticPointerInput(penHandle, myPointer, 1);
        }

        /// <summary>
        /// Destroys currently allocated pointer injection device.
        /// </summary>
        public void Dispose(){
            if (penHandle != IntPtr.Zero){
                DestroySyntheticPointerDevice(penHandle);

                var destError = Marshal.GetLastWin32Error();
                if (destError < 0)
                    Console.WriteLine("Failed to destroy handle : " + penHandle);
                Console.WriteLine("Pointer Destroyed : " + destError);
            }
        }
        /// <summary>
        /// Maps PEN_STATE to POINTER_FLAGS.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        POINTER_FLAGS GetPointerFlags(PEN_STATE state){
        switch (state)
        {   
            case PEN_STATE.PEN_UP:
            return POINTER_FLAGS.INRANGE | POINTER_FLAGS.UP;
            case PEN_STATE.PEN_DOWN:
            return POINTER_FLAGS.INRANGE | POINTER_FLAGS.UPDATE | POINTER_FLAGS.DOWN;
            case PEN_STATE.PEN_HOVER:
            return POINTER_FLAGS.INRANGE | POINTER_FLAGS.UPDATE;
            case PEN_STATE.PEN_CONTACT:
            return POINTER_FLAGS.INRANGE | POINTER_FLAGS.INCONTACT | POINTER_FLAGS.UPDATE;
            default:
            return POINTER_FLAGS.INRANGE | POINTER_FLAGS.UPDATE;
        }
        }
    }
}
#endif

/*
POINTER_FEEDBACK_DEFAULT
Value: 1
Visual feedback might be suppressed by the user's pen (Settings -> Devices -> Pen & Windows Ink) 
and touch (Settings -> Ease of Access -> Cursor & pointer size) settings.

POINTER_FEEDBACK_INDIRECT
Value: 2
Visual feedback overrides the user's pen and touch settings.
*/