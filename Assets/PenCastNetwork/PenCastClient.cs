using System;
using Mirror;
public class PenCastClient : NetworkBehaviour
{
/* Unused
    [SyncVar(hook = nameof(SetPenData))]
    PenData penData = new PenData(1, 0, 0, PEN_STATE.PEN_HOVER, false, false);

    //Only runs on server
    private void SetPenData(PenData oldVar, PenData newVar){
    }
*/
    
    public static event Action<PenData> OnInjectCommand;

//Client code (Android or IOS devices in this case)
#if UNITY_ANDROID || UNITY_IOS
    void Start(){
        PenInputManager.Get().PenStateUpdated += SendPenInput;
        PenInputManager.Get().TapStateUpdated += SendTapInput;
    }

    private void SendPenInput()
    {
        if (isLocalPlayer){
            //penData = PenInterface.Get().GetPenData();
            InjectCmd(PenInterface.Get().GetPenData());
        }
    }
    private void SendTapInput()
    {
        if (isLocalPlayer){
            InjectCmd(TapInput.Get().GetPenData());
        }
    } 
    void OnDestroy(){
        PenInputManager.Get().PenStateUpdated -= SendPenInput;
        PenInputManager.Get().TapStateUpdated -= SendTapInput;
    }
#endif

    //Runs on server
    [Command]
    void InjectCmd(PenData penData){
        OnInjectCommand?.Invoke(penData);
    }
}
