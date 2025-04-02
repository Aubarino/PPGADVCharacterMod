using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//for those who don't know c#,
//Define your variables at the Very Top like the others below- and then Fill those variables with Gets for textures 'n etc inside the Init method below.

public struct ADVCassets
{
    public static AudioClip[] WetSlapNormal;
    public static AudioClip[] WetSlapWeak;

    public static AudioClip[] CumAudio;
    public static AudioClip[] InflationAudio;

    public static AudioClip[] MoanMale;
    public static AudioClip[] MoanFemale;
    public static AudioClip[] MoanPerson;
    public static AudioClip[] MoanSnake;
    public static AudioClip[] MoanSynthMale;
    public static AudioClip[] MoanSynthFemale;
    public static AudioClip[] CosmicRegenAudio;
    public static AudioClip[] MoanCosmicMale;
    public static AudioClip[] MoanCosmicFemale;
    public static AudioClip[] MoanGoblinMale;
    public static AudioClip[] MoanGoblinFemale;

    public static Texture2D[] ParticleTextures;

    public static Sprite[] ClothingIconTextures;
    public static Texture2D[] ClothingTexTextures;
    public static Sprite[] ClothingObjTextures;
    public static int[] ClothingSlotIds;
    public static string[] ClothingNameIds;
    public static string[] ClothingDescs;
    public static bool[] ClothingNsfwStatus;

    Dictionary<string, Texture2D> Skins = new Dictionary<string, Texture2D>();

    // public static Texture2D FemaleSkin;
    // public static Texture2D FemaleFlesh;
    // public static Texture2D FemaleBone;
    // public static Texture2D CoolGuySkin;
    // public static Texture2D MaleSkin;
    // public static Texture2D MaleFlesh;
    // public static Texture2D MaleBone;
    // public static Texture2D FemaleFurrySkin;
    // public static Texture2D MaleFurrySkin;
    // public static Texture2D FishSkinFemale;
    // public static Texture2D FishSkinMale;
    // public static Texture2D FishFlesh;
    // public static Texture2D FishBone;
    // public static Texture2D ProtoSkinMale;
    // public static Texture2D ProtoSkinFemale;
    // public static Texture2D ProtoFlesh;
    // public static Texture2D ProtoBone;
    // public static Texture2D ChadSkin;
    // public static Texture2D SnakeBones;
    // public static Texture2D SnakeSkinMale;
    // public static Texture2D SnakeSkinFemale;
    // public static Texture2D SnakeFlesh;
    // public static Texture2D CosmicFleshMale;
    // public static Texture2D CosmicFleshFemale;
    // public static Texture2D CosmicBone;
    // public static Texture2D GoblinSkinFemale;
    // public static Texture2D GoblinSkinMale;

    public static Sprite DevNullSprite;

    public static Sprite[] TitBases;
    public static Sprite[] TitTips;

    public static Sprite[] DickBases;
    public static Sprite[] DickMids;
    public static Sprite[] DickTips;
    public static Sprite[] DickBalls;
    //- - - -
    public static Sprite[] Blush;
    public static Sprite[] LoveEyes;

    public static Sprite[] EarLeft;
    public static Sprite[] EarRight;
    public static Sprite[] TailStart;
    public static Sprite[] TailMid;
    public static Sprite[] TailTip;
    public static Sprite[] TailDmg;
    public static Sprite[] EarLarge;

    //objs
    public static Sprite[] FleshLights;
    public static Sprite[] FleshLightIcons;
    public static Sprite[] DildoIcons;
    public static Sprite[] DildoMain;
    
    //ADV dicks
    public static Sprite[] DickADV_Canine;
    public static Sprite[] DickADV_Fish;
    public static Sprite[] DickADV_Proto;
    public static Sprite[] DickADV_Snake;

    //Overlays
    public static Sprite[] Overlays;
    public static Sprite ChadDmgHairOverlay;
    public static Sprite ProtoThighDmg;
    public static Sprite ProtoChestDmg;
    public static Sprite ProtoVisorDmg;
    public static string Astring = "brxg5zPz6S";

    public static void Init() //loads up all the sexy assets, the boopup script bassically
    {
        if(AAS.BootCore){
        DickADV_Canine = CollectDickADV(1, 6);
        DickADV_Fish = CollectDickADV(2, 8);
        DickADV_Proto = CollectDickADV(3, 5);
        DickADV_Snake = CollectDickADV(4, 7);
        Overlays = GenOverlays(18); //sets up overlays from "assets\overlays", increasing in number up to the max here.
        ParticleTextures = GenParticleTextures(5);

        ClothingIconTextures = new Sprite[]{};
        ClothingTexTextures = new Texture2D[]{};
        ClothingObjTextures = new Sprite[]{};
        ClothingSlotIds = new int[]{};
        ClothingNameIds = new string[]{};
        ClothingDescs = new string[]{};
        ClothingNsfwStatus = new bool[]{};

        LoadClothingTextureSet("choker","Choker",1,"A Choker fashion accessory for the neck"); //1 is neck
        LoadClothingTextureSet("collar","Collar",1,"A Leather Collar with a little bell");
        LoadClothingTextureSet("sunglasses","Sunglasses",2,"Shades for looking cool"); //2 is eyes
        LoadClothingTextureSet("catpaw","Cat Paw Gloves",3,"Nya nya nya! paw gloves"); //3 is hands
        LoadClothingTextureSet("partyglasses","Party Glasses",2,"Pink plastic party glasses");
        LoadClothingTextureSet("fakebeard","Fake Beard",1,"A low quality fake beard, feels like plastic");
        LoadClothingTextureSet("balaclava","Balaclava",4,"A Balaclava, we need to put a team together >:)"); //4 is head
        LoadClothingTextureSet("funnyeye","Plastic Eye",2,"A funny plastic eye for gags and jokes");
        LoadClothingTextureSet("blindfold","Blindfold",2,"A Blindfold, for if you need to make someone unable to see");
        LoadClothingTextureSet("leatherjacket","Leather Jacket",5,"A cool leather jacket"); //5 is shirt

        FleshLights = new[]{
            ModAPI.LoadSprite("assets/objs/fleshlight_1.png")
        };
        FleshLightIcons = new[]{
            ModAPI.LoadSprite("assets/objs/fleshlight_icon1.png")
        };

        WetSlapNormal = new[]{
            ModAPI.LoadSound("sfx/wetslap_normal1.mp3"),
            ModAPI.LoadSound("sfx/wetslap_normal2.mp3"),
            ModAPI.LoadSound("sfx/wetslap_normal3.mp3"),
            ModAPI.LoadSound("sfx/wetslap_normal4.mp3")
        };
        WetSlapWeak = new[]{
            ModAPI.LoadSound("sfx/wetslap_weak1.mp3"),
            ModAPI.LoadSound("sfx/wetslap_weak2.mp3"),
            ModAPI.LoadSound("sfx/wetslap_weak3.mp3")
        };
        CumAudio = CollectAudioArray("cum",4);
        InflationAudio = CollectAudioArray("inflation",2);
        
        MoanMale = CollectAudioArray("moan_male",4);
        MoanFemale = CollectAudioArray("moan_female",7);
        MoanPerson = CollectAudioArray("moan_person",4);
        MoanSnake = CollectAudioArray("moan_snake",3);
        MoanSynthMale = CollectAudioArray("moan_male_synth",3);
        MoanSynthFemale = CollectAudioArray("moan_female_synth",3);
        CosmicRegenAudio = CollectAudioArray("cosmic",4);
        MoanCosmicMale = CollectAudioArray("moan_male_cosmic",4);
        MoanCosmicFemale = CollectAudioArray("moan_female_cosmic",4);
        MoanGoblinMale = CollectAudioArray("moan_male_goblin",5);
        MoanGoblinFemale = CollectAudioArray("moan_female_goblin",5);



        Skins = new Dictionary<string, Texture2D>(){
            //female
            {"FemaleSkin", ModAPI.LoadTexture("assets/default_skin_female.png")},
            {"FemaleFlesh", ModAPI.LoadTexture("assets/default_flesh_female.png")},
            {"FemaleBone", ModAPI.LoadTexture("assets/default_bone_female.png")},
            //male
            {"CoolGuySkin", ModAPI.LoadTexture("assets/default_skin_cool.png")},
            {"MaleSkin", ModAPI.LoadTexture("assets/default_skin_male.png")},
            {"MaleFlesh", ModAPI.LoadTexture("assets/default_flesh_male.png")},
            {"MaleBone", ModAPI.LoadTexture("assets/default_bone_male.png")},
            //furry canine
            {"FemaleFurrySkin", ModAPI.LoadTexture("assets/default_skin_furry_female.png")},
            {"MaleFurrySkin", ModAPI.LoadTexture("assets/default_skin_furry_male.png")},
            //fish - shark
            {"FishSkinFemale", ModAPI.LoadTexture("assets/default_skin_fish_female.png")},
            {"FishSkinMale", ModAPI.LoadTexture("assets/default_skin_fish_male.png")},
            {"FishFlesh", ModAPI.LoadTexture("assets/default_flesh_fish.png")},
            {"FishBone", ModAPI.LoadTexture("assets/default_bone_fish.png")},
            //protogen
            {"ProtoSkinFemale", ModAPI.LoadTexture("assets/proto_skin_female.png")},
            {"ProtoSkinMale", ModAPI.LoadTexture("assets/proto_skin_male.png")},
            {"ProtoFlesh", ModAPI.LoadTexture("assets/proto_flesh.png")},
            {"ProtoBone", ModAPI.LoadTexture("assets/proto_bone.png")},
            //chad
            {"ChadSkin", ModAPI.LoadTexture("assets/skin_chad.png")},
            //snakes
            {"SnakeBones", ModAPI.LoadTexture("assets/snake_skeleton.png")},
            {"SnakeFlesh", ModAPI.LoadTexture("assets/snake_flesh.png")},
            {"SnakeSkinMale", ModAPI.LoadTexture("assets/snake_skin_male.png")},
            {"SnakeSkinFemale", ModAPI.LoadTexture("assets/snake_skin_female.png")},
            //cosmic
            {"CosmicFleshMale", ModAPI.LoadTexture("assets/flesh_cosmic_male.png")},
            {"CosmicFleshFemale", ModAPI.LoadTexture("assets/flesh_cosmic_female.png")},
            {"CosmicBone", ModAPI.LoadTexture("assets/bone_cosmic.png")},
            //goblins
            {"GoblinSkinFemale", ModAPI.LoadTexture("assets/default_skin_goblin_female.png")},
            {"GoblinSkinMale", ModAPI.LoadTexture("assets/default_skin_goblin_male.png")}
        };
        //FemaleSkin = ModAPI.LoadTexture("assets/default_skin_female.png");
        //FemaleFlesh = ModAPI.LoadTexture("assets/default_flesh_female.png");
        //FemaleBone = ModAPI.LoadTexture("assets/default_bone_female.png");
        // CoolGuySkin = ModAPI.LoadTexture("assets/default_skin_cool.png");
        // MaleSkin = ModAPI.LoadTexture("assets/default_skin_male.png");
        // MaleFlesh = ModAPI.LoadTexture("assets/default_flesh_male.png");
        // MaleBone = ModAPI.LoadTexture("assets/default_bone_male.png");
        // FemaleFurrySkin = ModAPI.LoadTexture("assets/default_skin_furry_female.png");
        // MaleFurrySkin = ModAPI.LoadTexture("assets/default_skin_furry_male.png");
        // FishSkinFemale = ModAPI.LoadTexture("assets/default_skin_fish_female.png");
        // FishSkinMale = ModAPI.LoadTexture("assets/default_skin_fish_male.png");
        // FishFlesh = ModAPI.LoadTexture("assets/default_flesh_fish.png");
        // FishBone = ModAPI.LoadTexture("assets/default_bone_fish.png");
        // ProtoSkinFemale = ModAPI.LoadTexture("assets/proto_skin_female.png");
        // ProtoSkinMale = ModAPI.LoadTexture("assets/proto_skin_male.png");
        // ProtoFlesh = ModAPI.LoadTexture("assets/proto_flesh.png");
        // ProtoBone = ModAPI.LoadTexture("assets/proto_bone.png");
        // ChadSkin = ModAPI.LoadTexture("assets/skin_chad.png");
        // SnakeBones = ModAPI.LoadTexture("assets/snake_skeleton.png");
        // SnakeFlesh = ModAPI.LoadTexture("assets/snake_flesh.png");
        // SnakeSkinMale = ModAPI.LoadTexture("assets/snake_skin_male.png");
        // SnakeSkinFemale = ModAPI.LoadTexture("assets/snake_skin_female.png");
        // CosmicFleshMale = ModAPI.LoadTexture("assets/flesh_cosmic_male.png");
        // CosmicFleshFemale = ModAPI.LoadTexture("assets/flesh_cosmic_female.png");
        // CosmicBone = ModAPI.LoadTexture("assets/bone_cosmic.png");
        // GoblinSkinFemale = ModAPI.LoadTexture("assets/default_skin_goblin_female.png");
        // GoblinSkinMale = ModAPI.LoadTexture("assets/default_skin_goblin_male.png");

        DevNullSprite = ModAPI.LoadSprite("assets/dev_void.png");

        DildoIcons = new[]{
            ModAPI.LoadSprite("assets/objs/dildo_icon1.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon2.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon3.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon4.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon5.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon6.png"),
            ModAPI.LoadSprite("assets/objs/dildo_icon7.png")
        };
        DildoMain = new[]{
            ModAPI.LoadSprite("assets/objs/dildobase.png"),
            ModAPI.LoadSprite("assets/objs/dildobasenull.png")
        };

        TitBases = new[]{
            ModAPI.LoadSprite("assets/tit_1_body.png"),
            ModAPI.LoadSprite("assets/tit_2_body.png"),
            ModAPI.LoadSprite("assets/tit_3_body.png"),
            ModAPI.LoadSprite("assets/tit_4_body.png"),
            ModAPI.LoadSprite("assets/tit_5_body.png")
        };
        TitTips = new[]{
            ModAPI.LoadSprite("assets/tit_1_tip.png"),
            ModAPI.LoadSprite("assets/tit_2_tip.png"),
            ModAPI.LoadSprite("assets/tit_3_tip.png"),
            ModAPI.LoadSprite("assets/tit_4_tip.png"),
            ModAPI.LoadSprite("assets/tit_5_tip.png")
        };

        DickBases = new[]{
            ModAPI.LoadSprite("assets/dick_1_base.png"),
            ModAPI.LoadSprite("assets/dick_2_base.png"),
            ModAPI.LoadSprite("assets/dick_3_base.png")
        };
        DickMids = new[]{
            ModAPI.LoadSprite("assets/dick_1_mid.png"),
            ModAPI.LoadSprite("assets/dick_2_mid.png"),
            ModAPI.LoadSprite("assets/dick_3_mid.png")
        };
        DickTips = new[]{
            ModAPI.LoadSprite("assets/dick_1_tip.png"),
            ModAPI.LoadSprite("assets/dick_2_tip.png"),
            ModAPI.LoadSprite("assets/dick_3_tip.png")
        };
        DickBalls = new[]{
            ModAPI.LoadSprite("assets/dick_1_balls.png"),
            ModAPI.LoadSprite("assets/dick_2_balls.png"),
            ModAPI.LoadSprite("assets/fish_balls.png"),
            ModAPI.LoadSprite("assets/proto_balls.png"),
            ModAPI.LoadSprite("assets/snake_balls.png"),
            ModAPI.LoadSprite("assets/dick_3_balls.png")
        };

        Blush = new[]{
            ModAPI.LoadSprite("assets/blush_1.png"),
            ModAPI.LoadSprite("assets/blush_2.png"),
            ModAPI.LoadSprite("assets/blush_3.png"),
            ModAPI.LoadSprite("assets/blush_4.png"),
            ModAPI.LoadSprite("assets/blush_5.png"),
            ModAPI.LoadSprite("assets/blush_6.png"),
            ModAPI.LoadSprite("assets/blush_7.png"),
            ModAPI.LoadSprite("assets/blush_8.png"),
            ModAPI.LoadSprite("assets/blush_9.png"),
            ModAPI.LoadSprite("assets/blush_10.png")
        };
        LoveEyes = new[]{
            ModAPI.LoadSprite("assets/loveeyes_1.png")
        };

        EarLeft = new[]{
            ModAPI.LoadSprite("assets/ear_1_left.png")
        };
        EarRight = new[]{
            ModAPI.LoadSprite("assets/ear_1_right.png")
        };
        EarLarge = new[]{
            ModAPI.LoadSprite("assets/ear_large_1.png"),
            ModAPI.LoadSprite("assets/ear_large_2.png")
        };

        TailStart = new[]{
            ModAPI.LoadSprite("assets/tails/tail_1_start.png"),
            ModAPI.LoadSprite("assets/tails/tail_2_start.png"),
            ModAPI.LoadSprite("assets/tails/tail_3_start.png"),
            ModAPI.LoadSprite("assets/tails/tail_4_start.png"),
            ModAPI.LoadSprite("assets/tails/tail_5_start.png")
        };
        TailMid = new[]{
            ModAPI.LoadSprite("assets/tails/tail_1_mid.png"),
            ModAPI.LoadSprite("assets/tails/tail_2_mid.png"),
            ModAPI.LoadSprite("assets/tails/tail_3_mid.png"),
            ModAPI.LoadSprite("assets/tails/tail_4_mid.png"),
            ModAPI.LoadSprite("assets/tails/tail_5_mid.png")
        };
        TailTip = new[]{
            ModAPI.LoadSprite("assets/tails/tail_1_tip.png"),
            ModAPI.LoadSprite("assets/tails/tail_2_tip.png"),
            ModAPI.LoadSprite("assets/tails/tail_3_tip.png"),
            ModAPI.LoadSprite("assets/tails/tail_4_tip.png"),
            ModAPI.LoadSprite("assets/tails/tail_5_tip.png")
        };
        //damage stuff, idk
        TailDmg = new[]{
            ModAPI.LoadSprite("assets/tails/tail_1_destroyed.png"),
            ModAPI.LoadSprite("assets/tails/tail_2_destroyed.png"),
            ModAPI.LoadSprite("assets/tails/tail_3_destroyed.png"),
            ModAPI.LoadSprite("assets/tails/tail_4_destroyed.png"),
            ModAPI.LoadSprite("assets/tails/tail_5_destroyed.png")
        };
        //OTHER damage sprites...
        ChadDmgHairOverlay = ModAPI.LoadSprite("assets/overlays/5_dmg.png");
        ProtoChestDmg = ModAPI.LoadSprite("assets/overlays/11_dmg.png");
        ProtoThighDmg = ModAPI.LoadSprite("assets/overlays/10_dmg.png");
        ProtoVisorDmg = ModAPI.LoadSprite("assets/overlays/9-12_dmg.png");
        }
    }

    private static Sprite[] CollectDickADV(int DickADVID, int DickADVLength) //gets dick adv sprites automatically via a number of the ID it would be named as in the files,
    //and the total length of how many there are (the 2nd), look into the "assets/dick_adv" folder for examples of what I mean here...
    {
        Sprite[] TempDickADV = new Sprite[] {};
        if(AAS.BootCore){
        for(int i = 0; i < DickADVLength; i++)
        {
            TempDickADV = TempDickADV.Concat(new[] {ModAPI.LoadSprite("assets/dick_adv/dickadv_" + DickADVID + "_" + (i + 1) + ".png")}).ToArray();
        }
        }
        return(TempDickADV);
    }

    private static Sprite[] GenOverlays(int TotalOverlays) //sets up overlays automatically via their filenames in the Overlays folder, increasing in number up to the max sex in Init above.
    {
        Sprite[] TempOverlays = new Sprite[] {};
        if(AAS.BootCore){
            for(int i = 0; i < TotalOverlays; i++)
            {
                TempOverlays = TempOverlays.Concat(new[] {ModAPI.LoadSprite("assets/overlays/" + (i + 1) + ".png")}).ToArray();
            }
        }
        return(TempOverlays);
    }

    private static Texture2D[] GenParticleTextures(int TotalParticles) //sets up particle textures automatically via their filenames in the particles folder, increasing in number up to the max sex in Init above.
    {
        Texture2D[] TempParticles = new Texture2D[] {};
        if(AAS.BootCore){
        for(int i = 0; i < TotalParticles; i++)
            {
                TempParticles = TempParticles.Concat(new[] {ModAPI.LoadTexture("assets/particles/" + (i + 1) + ".png")}).ToArray();
            }
        }
        return(TempParticles);
    }

    public static void LoadClothingTextureSet(string clothingName,string visibleName = "undefined",int clothingSlotID = 0,string Description = "no description",bool nsfwClothing = false){
        //Texture2D[] TempParticles = new Texture2D[] {};
        if(AAS.BootCore){
            ClothingObjTextures = ClothingObjTextures.Concat(new[] {ModAPI.LoadSprite("assets/clothing/" + clothingName + ".png")}).ToArray();
            ClothingIconTextures = ClothingIconTextures.Concat(new[] {ModAPI.LoadSprite("assets/clothing/" + clothingName + "_icon.png")}).ToArray();
            ClothingTexTextures = ClothingTexTextures.Concat(new[] {ModAPI.LoadTexture("assets/clothing/" + clothingName + "_tex.png")}).ToArray();
            ClothingSlotIds = ClothingSlotIds.Concat(new []{clothingSlotID}).ToArray();
            ClothingNameIds = ClothingNameIds.Concat(new []{visibleName}).ToArray();
            ClothingDescs = ClothingDescs.Concat(new []{Description}).ToArray();
            ClothingNsfwStatus = ClothingNsfwStatus.Concat(new []{nsfwClothing}).ToArray();
        }
        //return(TempParticles);
    }

    private static AudioClip[] CollectAudioArray(string audioFileName,int amtGet){ //added this on 30/3/2025, cause fuck the old method
        //Debug.Log("sex mod loading audio (" + audioFileName + ")");
        AudioClip[] TempAudios = new AudioClip[]{};
        if(AAS.BootCore){
            for(int i = 0; i < amtGet; i++){
                TempAudios = TempAudios.Concat(new[] {ModAPI.LoadSound("sfx/" + audioFileName + (i + 1) + ".mp3")}).ToArray();
            }
        }
        return(TempAudios);
    }
    
}
