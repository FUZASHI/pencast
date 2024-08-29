using Mirror;
using UnityEngine;

public class AppSettings : MonoBehaviour
{
#region Singleton
    private static AppSettings Instance = null;
    private AppSettings(){}
    public static AppSettings Get() => Instance;
#endregion

#region Screen resolution
    int scrWidth = 0;
    int scrHeight = 0;
    public int ScreenWidth {get => scrWidth;}
    public int ScreenHeight {get => scrHeight;}

    void SetDefaultSettings(){
        scrWidth = Screen.currentResolution.width;
        scrHeight = Screen.currentResolution.height;
    }
#endregion

#region Server
    [SerializeField]
    [Tooltip("Server auto-start option")]
    bool serverAutoStart = true;
    void RegisterServerEvents(){
        EventNetworkManager.OnStartServerEvent += OnStartServer;
        EventNetworkManager.OnServerStopEvent += OnStopServer;
    }
    private void OnStopServer()
    {
        EventNetworkManager.discovery.StopDiscovery();
    }

    private void OnStartServer()
    {
        EventNetworkManager.discovery.AdvertiseServer();
    }
#endregion
    void Awake(){
        if (Instance != null) {Destroy(gameObject); return;}
        Instance = this;

        RegisterServerEvents();
        SetDefaultSettings();
    }
    void Start()
    {
        if (serverAutoStart)
        NetworkManager.singleton.StartServer();   
    }
}
