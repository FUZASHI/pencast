using UnityEngine;
using TMPro;
using System;
using InputInjectionWIN32;
using System.Linq;

public class KeyBindingUI : MonoBehaviour
{
    [Header("Dropdown menus")]
    [SerializeField]
    TMP_Dropdown penButton1Selector;
    [SerializeField]
    TMP_Dropdown penButton2Selector;
    void Start(){
        //Initialize first selector
        InitializeSelector(penButton1Selector, PenController.Get().pen1Button);

        //Initialize second selector
        InitializeSelector(penButton2Selector, PenController.Get().pen2Button);
    }
    void InitializeSelector(TMP_Dropdown dropdown, PenButton penButton){
        var values = Enum.GetNames(typeof(KEYCODE_WIN)).ToList();
        dropdown.AddOptions(values);

        dropdown.value = dropdown.options.FindIndex(option => option.text == penButton.KeyCode.ToString());

        dropdown.onValueChanged.AddListener((v)=>{
            var selected = dropdown.options[v].text;
            
            KEYCODE_WIN keycode;
            try{
                keycode = Enum.Parse<KEYCODE_WIN>(selected); 
                penButton.ChangeKey(keycode);
                penButton.SavePrefs();
            } catch(Exception ex){Debug.Log(ex.Message);}
        });
    }

}
