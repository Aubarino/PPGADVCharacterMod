
using System;
using UnityEngine;

public struct Config
{
    public static bool NSFW_MODE;
    public static bool Debug_Mode;
    public static bool Moan_Happens;
    public static bool Cum_Audio;
    public static bool Furry_Mode;
    public static bool Inflation;

    public static void Config_Setup()
    {
        // By settings this to true YOU AGREE to that you're above the age of 18 or the age of consent at your current location!
        // and take SOLE Responsibility for using the mod in NSFW MODE!!!
        // Otherwise, Set this to false! It will default to false!

        bool I_AGREE_TO_THE_ABOVE_STATEMENT = true; //---- set this to true if you want NSFW Mode, remember to leave the end line character aka ";" at the end, so "true;"

        //NOTE - READ MEEEEEEEEEEEEEEEEEE!!!!!!!!!!!!!! === toggle erections with activate (F Key) on crotch!
        NSFW_MODE = I_AGREE_TO_THE_ABOVE_STATEMENT;

        Debug_Mode = false; //provides debug abilities such as renders, manual checks and stats, recommend this being false.
        


        //==============================
        //IGNORED IF NOT IN NSFW MODE!!!
        //==============================

        Moan_Happens = true; //makes moaning to occur in NSFW mode.

        Cum_Audio = true; //if true, cumming has audio.
        
        Furry_Mode = false; //set to true if you want "Furry Mode", aka Cum is insanely overkill and orgasms last longer

        Inflation = true; //if set to true, adds a context menu option to some parts for "inflation".
        // why did i add this? idk, someone will probably like this feature, knowing you weirdos...
    }
}