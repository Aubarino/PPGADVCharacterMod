using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public static class AAS //fuck you ppg workshop, for reuploading shit for no reason
{
    public static bool BootCore = true;
    internal static bool RCore = true;
    internal static void ProdM()
    {
        //RCore = ((ModAPI.Metadata.Author == "Aubarino") && ((ModAPI.Metadata.UGCIdentity == null) && (ModAPI.Metadata.CreatorUGCIdentity == null)));
        if (ModAPI.Metadata.Author != "Aubarino" && ModAPI.Metadata.Author != "Abyssal Studio" && ModAPI.Metadata.Author != "Aub" && ModAPI.Metadata.Author != "AbyssalStudio"){
            RCore = false;
            //Utils.OpenURL("https://discord.gg/" + ADVCassets.Astring);
        }
        //if (!RCore)Utils.OpenURL("https://discord.gg/" + ADVCassets.Astring);
        BootCore = RCore;
        Debug.Log("ProdLoaded SKIP");
    }
    public static void Boot()
    {
        ProdM();
        PopupSetup.Execute();
    }
}
