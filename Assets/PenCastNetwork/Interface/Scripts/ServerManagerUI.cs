using System;
using System.Net;
using System.Net.Sockets;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ServerManagerUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField]
    TMPro.TMP_Text header;
    [SerializeField]
    TMPro.TMP_Text localIPLabel;

    [Header("Connection indicators")]
    [SerializeField]
    Image outline;
    [SerializeField]
    Image indicator;
    [SerializeField]
    Color onColor = Color.green;
    [SerializeField]
    Color offColor = Color.white;
    [Header("Start/Stop Button")]
    [SerializeField]
    Button serverButton;
    TMPro.TMP_Text serverButtonText;
    
    //Runs before auto-start
    void Awake()
    {
        if (
            localIPLabel == null || 
            indicator == null ||
            outline == null ||
            serverButton == null) 
            return;

        serverButtonText = serverButton.GetComponentInChildren<TMPro.TMP_Text>();

        if (serverButtonText == null) return;

        //Bind the button
        serverButton.onClick.AddListener(()=>{
            if (NetworkManager.singleton.isNetworkActive)
                NetworkManager.singleton.StopServer();
            else
                NetworkManager.singleton.StartServer();
        });

        //Setting initial values
        SetUIValues(offColor);

        //Register events
        RegisterServerEvents();

    }

    void RegisterServerEvents(){
        EventNetworkManager.OnStartServerEvent += OnStartServer;
        EventNetworkManager.OnServerStopEvent += OnStopServer;
    }
    private void OnStartServer()
    {
        SetUIValues(onColor);
        serverButtonText.text = "click to stop";
        header.text = "server : online";
    }

    private void OnStopServer()
    {
        SetUIValues(offColor);
        serverButtonText.text = "click to start";
        header.text = "server : offline";
    }


    public void SetUIValues(Color value){
        localIPLabel.text = "IP : " + NetUtilities.GetLocalIPAddress();

        indicator.color = value;
        outline.color = value;
    }

}
