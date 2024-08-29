using System;
using Mirror;
using Mirror.Discovery;

public class EventNetworkManager : NetworkManager
{
#region Client Events
    public static event Action OnClientConnectEvent;
    public static event Action OnClientDisconnectEvent;
    public static event Action OnClientConnectServerEvent;
    public static event Action OnClientDisconnectServerEvent;
    public override void OnClientConnect(){
        base.OnClientConnect();

        OnClientConnectEvent?.Invoke();
    }
    public override void OnClientDisconnect(){
        base.OnClientDisconnect();

        OnClientDisconnectEvent?.Invoke();
    }
    public override void OnServerConnect(NetworkConnectionToClient conn){
        base.OnServerConnect(conn);

        OnClientConnectServerEvent?.Invoke();
    }
    
    public override void OnServerDisconnect(NetworkConnectionToClient conn){
        base.OnServerDisconnect(conn);

        OnClientDisconnectServerEvent?.Invoke();
    }
#endregion    

#region Server Events
    public static event Action OnStartServerEvent;
    public static event Action OnServerStopEvent;
    public static event Action OnServerErrorEvent;
    public override void OnStartServer(){
        base.OnStartServer();
        OnStartServerEvent?.Invoke();
    }
    public override void OnStopServer(){
        base.OnStopServer();

        OnServerStopEvent?.Invoke();
    }
    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason){
        base.OnServerError(conn, error, reason);

        OnServerErrorEvent?.Invoke();
    }
#endregion
    public static NetworkDiscovery discovery;
    override public void Awake(){
        base.Awake();

        discovery = GetComponent<NetworkDiscovery>();
    } 
}
