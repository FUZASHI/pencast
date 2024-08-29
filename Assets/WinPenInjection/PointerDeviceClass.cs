#if UNITY_STANDALONE_WIN
namespace PenInjectionWIN32
{
    public enum POINTER_INPUT_TYPE
    {
        PT_POINTER = 0x1,
        PT_TOUCH = 0x2,
        PT_PEN = 0x3,
        PT_MOUSE = 0x4,
        PT_TOUCHPAD = 0x5
    }
    public enum POINTER_FEEDBACK_MODE
    {
        _DEFAULT = 0x1,
        _INDIRECT = 0x2,
        _NONE = 0x3,
    }

    public enum POINTER_DEVICE_TYPE
    {
        INTEGRATED_PEN = 0x1,
        EXTERNAL_PEN = 0x2,
        TOUCH = 0x3,
        TOUCH_PAD = 0x4,
        MAX = unchecked((int)0xffffffff),
    }
}
#endif