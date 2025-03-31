
using System;
using UnityEngine;

public struct Config
{
    public static bool NSFW_MODE;
    public static bool Debug_Mode;

    public static void Config_Setup()
    {
        // By settings this to true YOU AGREE to that you're above the age of 18 or the age of consent at your current location!
        // and take SOLE Responsibility for using the mod in NSFW MODE!!!
        // Otherwise, Set this to false! It will default to false!

        bool I_AGREE_TO_THE_ABOVE_STATEMENT = false; //---- set this to true if you want NSFW Mode, remember to leave the end line character aka ";" at the end, so "true;"

        NSFW_MODE = I_AGREE_TO_THE_ABOVE_STATEMENT;

        Debug_Mode = false; //provides debug abilities such as renders, manual checks and stats, recommend this being false.
    }
}