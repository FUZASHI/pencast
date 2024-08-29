public enum PEN_STATE{
    PEN_HOVER,
    PEN_CONTACT,
    PEN_DOWN,
    PEN_UP
}
public struct PenData{
    public float x;
    public float y;
    public float pressure;
    public PEN_STATE state;
    public bool firstPressed;
    public bool secPressed;

    public PenData(float _pressure, float _x, float _y, PEN_STATE _state, bool _firstPressed, bool _secPressed){
        //Pen pressure
        pressure = _pressure;

        //Position
        x = _x;
        y = _y;

        //Pointer state
        state = _state;

        //Button state
        firstPressed = _firstPressed;
        secPressed = _secPressed;
    }
}