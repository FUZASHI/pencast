using System;
using InputInjectionWIN32;
using UnityEngine;

[Serializable]
public class PenButton{
    bool isDown = false;
    [SerializeField]
    KEYCODE_WIN keyCode = KEYCODE_WIN.MOUSE_MIDDLE;
    public KEYCODE_WIN KeyCode {get => keyCode;}
    [SerializeField]
    string saveName = "";
    public PenButton(){
    }

    public void ProcessInput(PenData penData, bool isPressed){
        // Return if keyboard key was pressed again
        if (isPressed && isDown) return;

        // Button up
        if (isDown && !isPressed){
            InputDevice.Get().ProcessInput(keyCode, penData.x, penData.y, false);
        }

        // Button down
        if (isPressed){
            InputDevice.Get().ProcessInput(keyCode, penData.x, penData.y, true);
        }

        isDown = isPressed;
    }

    public void ChangeKey(KEYCODE_WIN changeCode){
        keyCode = changeCode;
    }

    public void LoadSavedKey(){
        string res = PlayerPrefs.GetString(saveName);
        if (string.IsNullOrEmpty(res)) return;

        KEYCODE_WIN keycode;
        try{
            keycode = Enum.Parse<KEYCODE_WIN>(res); 
            ChangeKey(keycode);
        } catch(Exception ex){Debug.Log(ex.Message);}
    }
    public void SavePrefs(){
        PlayerPrefs.SetString(saveName, keyCode.ToString());
        PlayerPrefs.Save();
    }
}