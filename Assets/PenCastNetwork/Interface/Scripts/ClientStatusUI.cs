using UnityEngine;
using UnityEngine.UI;
public class ClientStatusUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text clientConnLabel;
    [Header("Connection indicators")]
    [SerializeField]
    Image outline;
    [SerializeField]
    Image indicator;
    [SerializeField]
    Color onColor = Color.green;
    [SerializeField]
    Color offColor = Color.white;
    void Awake(){
        if (
            clientConnLabel == null || 
            indicator == null ||
            outline == null)
            return;

        clientConnLabel.text = "not connected";
        SetUIValues(offColor);
    }
    void Start()
    {
        //Runs on client
        EventNetworkManager.OnClientConnectEvent += ClientConnect;
        EventNetworkManager.OnClientDisconnectEvent += ClientDisconnect;
        //Runs on server
        EventNetworkManager.OnClientConnectServerEvent += ClientConnect;
        EventNetworkManager.OnClientDisconnectServerEvent += ClientDisconnect;
    }

    private void ClientDisconnect()
    {
        Debug.Log("Client disconnected");
        clientConnLabel.text = "not connected";
        SetUIValues(offColor);
    }

    private void ClientConnect()
    {
        Debug.Log("Client connected");
        clientConnLabel.text = "connected";
        SetUIValues(onColor);
    }

    public void SetUIValues(Color value){
        indicator.color = value;
        outline.color = value;
    }

}
