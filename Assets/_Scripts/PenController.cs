using InputInjectionWIN32;
using PenInjectionWIN32;
using UnityEngine;

public class PenController : MonoBehaviour
{   
#region Singleton
    private static PenController Instance;
    private PenController(){}
    public static PenController Get() => Instance;
#endregion
    public PenDevice penDevice = new PenDevice();
    [Header("Button bindings")]
    [SerializeField]
    public PenButton pen1Button = new PenButton();
    [SerializeField]
    public PenButton pen2Button = new PenButton();
    void Awake(){
        if (Instance != null) {Destroy(gameObject); return;}
        Instance = this;

        penDevice.CreateDevice();

        pen1Button.ChangeKey(KEYCODE_WIN.MOUSE_LEFT);
        pen2Button.ChangeKey(KEYCODE_WIN.MOUSE_RIGHT);

        LoadPrefs();
    }
    void LoadPrefs(){
        pen1Button.LoadSavedKey();
        pen2Button.LoadSavedKey();
    }
    void Start()
    {
        PenCastClient.OnInjectCommand += InjectInput;
    }

    public void InjectInput(PenData data)
    {
        // Process pen injection
        penDevice.InjectInput(data);

        // Process key input injection
        pen1Button.ProcessInput(data, data.firstPressed);
        pen2Button.ProcessInput(data, data.secPressed);
    }
}
