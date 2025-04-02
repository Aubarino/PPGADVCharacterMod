using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
//using System.IO;
//using System.Reflection;

public class PopupEntry : MonoBehaviour
{
    // public SteamWorkshopController SteamWorkshopController;

    // public void Start(){
    //     SteamWorkshopController = gameObject.AddComponent<SteamWorkshopController>();
    // }

    public void Invite(){
        //string Woot = "https://discord.gg/brxg5zPz6S";
        //PopupSetup.DoMethodIMPO("OpenURL","Utils",new object[1]{Woot});
        // SteamWorkshopController.Invoke("OpenLocal",0);
        //SteamWorkshopController.OpenLocal(Woot);
        //Utils.OpenURL("https://discord.gg/brxg5zPz6S");
        UnityEngine.Object.Destroy(this.gameObject);
    }
}

public struct PopupSetup{
    public static bool IsInstanceHost = false;
    public static GameObject InviteHost;
    public static GameObject InviteButton;
    public static bool DoneLink = false;

    public static void Execute(){
        // InviteHost = UnityEngine.GameObject.Find("AubInvitePopupHost");
        // if (InviteHost == null){
        //     IsInstanceHost = true;
        //     InviteHost = new GameObject("AubInvitePopupHost");
        // }
        // if (IsInstanceHost) CreateInviteInstance("Abyssal Studio(s)");
    }

    // public static void CreateInviteInstance(string Name){
    //     GameObject ClearButton = null;

    //     ClearButtonBehaviour[] AllPossibleCButtoons = UnityEngine.GameObject.FindObjectsOfType<ClearButtonBehaviour>(); //was finding the wrong one, fixed it
    //     foreach(ClearButtonBehaviour ThisButtonObj in AllPossibleCButtoons){
    //         if (ThisButtonObj.gameObject.name == "Clear Everything")
    //         ClearButton = ThisButtonObj.gameObject;
    //     }

    //     GameObject InviteButton = UnityEngine.GameObject.Instantiate(ClearButton, ClearButton.transform.root); //ClearButton.transform.parent)
    //     InviteButton.name = Name;

    //     Vector2 pos = ClearButton.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
    //     InviteButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + 2300,pos.y - 15);

    //     UnityEngine.GameObject.Destroy(InviteButton.GetComponent<ClearLivingBehaviour>());
    //     UnityEngine.UI.Button ButtonBehav = InviteButton.GetComponent<Button>();
    //     PopupEntry PopupEntryVar = InviteButton.AddComponent<PopupEntry>();

    //     ButtonBehav.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
    //     ButtonBehav.onClick.AddListener(PopupEntryVar.Invite);

    //     ColorBlock colours = ButtonBehav.colors;
    //     colours.normalColor = new Color32(65, 115, 114, 80);
    //     colours.highlightedColor = new Color32(120, 168, 167, 200);
    //     ButtonBehav.colors = colours;
        
    //     (InviteButton.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI).text = "Join "+ Name +" Discord";

    //     HasTooltipBehaviour TooltipBehaviour = InviteButton.GetComponentInChildren(typeof(HasTooltipBehaviour)) as HasTooltipBehaviour;
    //     TooltipBehaviour.Text = Name +" Discord Server Prompt";
    // }

    // public static void DoMethodIMPO(string TheMethod, string TheClass, object[] ImpoValues){ //inside the backrooms level class too, just put here so i could make levels Regist automatically
    //     object[] methValues = ImpoValues;

    //     var ClassType = Type.GetType(TheClass);
    //     ConstructorInfo MethConstructor = ClassType.GetConstructor(Type.EmptyTypes);
    //     var levelClassObject = MethConstructor.Invoke(new object[]{});

    //     MethodInfo LevMethodInf = ClassType.GetMethod(TheMethod);
    //     object MethValue = LevMethodInf.Invoke(levelClassObject, methValues);
    // }
}