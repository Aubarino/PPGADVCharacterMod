
//created at 6am on a tuesday one night, originally..
//yo coder hours?

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Events;

// +++++++++++++#######+++++++++++++++++
// ++++++++++###############+++++++++++++
// ++++++++###++++++++++++###+++++++++++
// +++++++##+++++++++++++++####+++++++++
// ++++++##+++++++++++++++++####++++++++
// +++++##++++++++++++############+++++++
// +++++##++++#####+++#++#####+####++++++
// +++++##+++++#++#+++#+++++++++##++++++
// +++++#+++++++++++++++#++++++##+#+++++
// ++++++##+++++++++++####++++####++++++
// +++++##+#+++++++++++++++++++##+#+++++
// +++++#+++++++++++ +######++++##+++++++
// ++++++#++#++++++##+++###++++##+++++++
// +++++++++#+++++++++++#++++#####++++++
// ++++++++++++#+++++++++++#############
// ++++++++++++++##++++########+########
// +++++++++++###+++#########++#########
// ++++++#########+++++++##++++#########
// +++############+++++#++++++##########
// ++##############++++++++++###########
//send help, what am i doing with my life..
//why did i make this
//why did i over-engineer it so much.
//i fucking give up.

namespace Mod
{
    public class Mod
    {

        public static bool Debug = false;
        public const float PixelSize = Global.MetricMultiplier / 35f;

        public static void Main()
        {
            //Debug.Log("sex mod loaded");
            AAS.Boot();
            if(AAS.BootCore){
                Config.Config_Setup(); //sets up the config!
                classicCB.Create("ACM", Config.NSFW_MODE ? "THE SEX MOD" : "Advanced Character Mod", ModAPI.LoadSprite(Config.NSFW_MODE ? "caticon.png" : "caticon_normal.png", 4));
                ADVCassets.Init(); //loads up all the assets!
                RegisterHumans(); //human objects
                RegisterObjs(); //non-human objects
                ModAPI.Register<LimbInspectOverhaul>(); //inspect overhaul code
            }
            //Debug.Log("sex mod load end");
        }

        public static void RegisterObjs(){//non-human objects
            if(AAS.BootCore){
            //fleshlight 1
                //Debug.Log("sex mod registering stuff");
                if (Config.NSFW_MODE){
                    ModAPI.Register(
                        new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Brick"),
                            NameOverride = "Fleshlight",
                            NameToOrderByOverride = "!!!!!objs_fleshlight_1",
                            DescriptionOverride = "A Textured Rubber Vagina.",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ADVCassets.FleshLightIcons[0],
                            AfterSpawn = (Instance) =>
                            {
                                Instance.GetComponent<SpriteRenderer>().sprite = ADVCassets.FleshLights[0]; 
                                PhysicalBehaviour PhysBehav = Instance.GetComponent<PhysicalBehaviour>();
                                PhysBehav.InitialMass = 0.2f;
                                PhysBehav.TrueInitialMass = 0.2f;
                                PhysBehav.Properties = ModAPI.FindPhysicalProperties("Rubber");

                                LimbAddon Fleshlight = Instance.AddComponent<LimbAddon>();
                                Fleshlight.HasPussy = true;
                                MakeFuckable(Instance);
                            }
                        }
                    );
                    RegistDildo(1,"Dildo","A rubber penis",7);
                    RegistDildo(2,"Large Dildo","A large rubber penis",10);
                    RegistDildo(3,"Canine Dildo","A canine dildo with a knot",5,true,ADVCassets.DickADV_Canine,1);
                    RegistDildo(4,"Fish Dildo","A anthropomorphic fish dildo",7,true,ADVCassets.DickADV_Fish,3);
                    RegistDildo(5,"Biosynthetic Penis","A biosynthetic penis",4,true,ADVCassets.DickADV_Proto,4);
                    RegistDildo(6,"Snake Dildo","A anthropomorphic snake dildo",6,true,ADVCassets.DickADV_Snake,5);
                    RegistDildo(7,"Goblin Dildo","A dildo based on a goblin",5,false,null,6,3);
                }
                RegistAllClothing();
            }
        }

        public static void RegisterHumans() //human objects
        {
            if(AAS.BootCore){
            //female human
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Female Human",
                            NameToOrderByOverride = "!!!!!humans_female_s1",
                            DescriptionOverride = "A Female Human",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_female.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Tits = true;
                                        Chest.TitsModel = 1;
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.Instance = limb.gameObject;
                                        Crotch.HasPussy = true;
                                        Crotch.HasAss = true;

                                        MakeFuckable(limb.gameObject,"female");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 1;

                                        Head.MoanSource = true;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["FemaleSkin"], ADVCassets.Skins["FemaleFlesh"], ADVCassets.Skins["FemaleBone"]);
                            }
                        }
                );
            //male human code
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Human (s2)",
                            NameToOrderByOverride = "!!!!!humans_male_s1",
                            DescriptionOverride = "A Male Human",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DickLength = 2;
                                        Crotch.DoDickAudio = true;

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 1;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 2;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["MaleSkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            //male human normal dick
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Human (s4)",
                            NameToOrderByOverride = "!!!!!humans_male_s2",
                            DescriptionOverride = "A Male Human",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DickLength = 4;
                                        Crotch.DoDickAudio = true;

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 1;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 2;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["MaleSkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            //male human large dick
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Human (s9)",
                            NameToOrderByOverride = "!!!!!humans_male_s3",
                            DescriptionOverride = "A Male Human",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DickLength = 9;
                                        Crotch.DoDickAudio = true;

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 1;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 2;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["MaleSkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            //anthro canine male
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Anthro Canine",
                            NameToOrderByOverride = "!!!!!anthro_canine_male_s1",
                            DescriptionOverride = "Anthropomorphic Male Canine Character",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/furry_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Canine";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DModelAdv = true;
                                        Crotch.DModelAdvSprites = ADVCassets.DickADV_Canine;
                                        Crotch.DickLength = 5; //1 less then the total dick length of the adv dick, as we're ignoring the Dick Base part of it.
                                        Crotch.DoDickAudio = true;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 1;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[0];

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 1;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 2;

                                        Head.Ears = true;
                                        Head.EarModel = 1;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 1;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["MaleFurrySkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            //anthro canine female
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Female Anthro Canine",
                            NameToOrderByOverride = "!!!!!anthro_canine_female_s1",
                            DescriptionOverride = "Anthropomorphic Female Canine Character",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/furry_female.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Canine";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Tits = true;
                                        Chest.TitsModel = 1;
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.Instance = limb.gameObject;
                                        Crotch.HasPussy = true;
                                        Crotch.HasAss = true;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 1;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[0];

                                        MakeFuckable(limb.gameObject,"female");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 1;
                                        Head.MoanSource = true; //experimental

                                        Head.Ears = true;
                                        Head.EarModel = 1;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 1;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["FemaleFurrySkin"], ADVCassets.Skins["FemaleFlesh"], ADVCassets.Skins["FemaleBone"]);
                            }
                        }
                );
            //chad human
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Chad Human",
                            NameToOrderByOverride = "!!!!!humans_male_chad_s1",
                            DescriptionOverride = "A Superior Human who gets all the bitches and Reddit Gold without using Reddit. Will Ratio you on Twitter",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_chad.png"),
                            AfterSpawn = (Instance) =>
                            {
                                AchievementJoke.UnlockAchievement("CHAD");
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.transform.root.localScale *= 1.01f;
                                    limb.RegenerationSpeed += 2f;
                                    limb.ShotDamageMultiplier *= 0.95f;
                                    limb.Numbness = 0f;
                                    limb.ImpactPainMultiplier = 0.1f;
                                    limb.BaseStrength = 8;
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 2;
                                        Crotch.DickLength = 10;
                                        Crotch.DoDickAudio = true;

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 2;

                                        Crotch.CumAmt = 100; //damnnnnnnnnnnnnnnnnnnnnnn
                                        Crotch.OrgasmPeak = 150;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.Overlay = true;
                                        Head.OverlayModel = 5;
                                        Head.DmgVari_Overlay = ADVCassets.ChadDmgHairOverlay;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    case "UpperLegFront":
                                        var UpperLegFront = limb.gameObject.AddComponent<LimbAddon>();
                                        //UpperLegFront.transform.localScale = new Vector3(1.1f, 1.1f);
                                        UpperLegFront.Instance = limb.gameObject;
                                        UpperLegFront.Overlay = true;
                                        UpperLegFront.OverlayModel = 3;
                                        UpperLegFront.OverlaySortNum = 1;
                                        break;
                                    case "UpperLeg":
                                        var UpperLeg = limb.gameObject.AddComponent<LimbAddon>();
                                        UpperLeg.Instance = limb.gameObject;
                                        UpperLeg.Overlay = true;
                                        UpperLeg.OverlayModel = 3;
                                        UpperLeg.OverlayLayer = "Background";
                                        UpperLeg.OverlaySortNum = 2;
                                        break;
                                    case "UpperArmFront":
                                        var UpperArmFront = limb.gameObject.AddComponent<LimbAddon>();
                                        UpperArmFront.Instance = limb.gameObject;
                                        UpperArmFront.Overlay = true;
                                        UpperArmFront.OverlayModel = 4;
                                        UpperArmFront.OverlaySortNum = 2;
                                        break;
                                    case "UpperArm":
                                        var UpperArm = limb.gameObject.AddComponent<LimbAddon>();
                                        UpperArm.Instance = limb.gameObject;
                                        UpperArm.Overlay = true;
                                        UpperArm.OverlayModel = 4;
                                        UpperArm.OverlayLayer = "Background";
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["ChadSkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            //anthro fish male
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Anthro Fish",
                            NameToOrderByOverride = "!!!!!anthro_fish_male_s1",
                            DescriptionOverride = "Anthropomorphic Male Fish Character",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/fish_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                GenderHuman.SetBruiseColor(95, 49, 13);
                                GenderHuman.SetSecondBruiseColor(120, 62, 20);
                                GenderHuman.SetThirdBruiseColor(158, 103, 36);
                                GenderHuman.SetBloodColour(247, 119, 67);
                                GenderHuman.SetRottenColour(99, 76, 43);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Fish";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Overlay = true;
                                        Chest.OverlayModel = 2;
                                        //Head.OverlaySortNum = 1;
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DModelAdv = true;
                                        Crotch.DModelAdvSprites = ADVCassets.DickADV_Fish;
                                        Crotch.DickLength = 7; //1 less then the total dick length of the adv dick, as we're ignoring the Dick Base part of it.
                                        Crotch.DoDickAudio = true;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 2;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[1];

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 3;

                                        Crotch.CumColour = new Color32(252, 252, 181, 110);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 3;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 7;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["FishSkinMale"], ADVCassets.Skins["FishFlesh"], ADVCassets.Skins["FishBone"]);
                            }
                        }
                );
            //anthro fish female
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Female Anthro Fish",
                            NameToOrderByOverride = "!!!!!anthro_fish_female_s1",
                            DescriptionOverride = "Anthropomorphic Female Fish Character",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/fish_female.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                GenderHuman.SetBruiseColor(95, 49, 13);
                                GenderHuman.SetSecondBruiseColor(120, 62, 20);
                                GenderHuman.SetThirdBruiseColor(158, 103, 36);
                                GenderHuman.SetBloodColour(247, 119, 67);
                                GenderHuman.SetRottenColour(99, 76, 43);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Fish";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Overlay = true;
                                        Chest.OverlayModel = 2;
                                        
                                        Chest.Tits = true;
                                        Chest.TitsModel = 2;
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        Crotch.HasPussy = true;
                                        MakeFuckable(limb.gameObject,"female");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 2;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[1];

                                        Crotch.CumColour = new Color32(252, 252, 181, 110);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 1;
                                        Head.MoanSource = true; //experimental

                                        Head.Overlay = true;
                                        Head.OverlayModel = 8;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["FishSkinFemale"], ADVCassets.Skins["FishFlesh"], ADVCassets.Skins["FishBone"]);
                            }
                        }
                );
            //protogem male
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Protogen",
                            NameToOrderByOverride = "!!!!!anthro_protogen_male_s1",
                            DescriptionOverride = "Male Protogen Character with Biosynthetic male parts",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/proto_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                // GenderHuman.SetBruiseColor(79, 27, 27);
                                // GenderHuman.SetSecondBruiseColor(115, 77, 69);
                                // GenderHuman.SetThirdBruiseColor(165, 135, 119);
                                // GenderHuman.SetBloodColour(219, 37, 37);
                                GenderHuman.SetRottenColour(79, 63, 63);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.RegenerationSpeed += 1f;
                                    limb.ShotDamageMultiplier *= 0.8f;
                                    limb.ImpactPainMultiplier *= 0.6f;
                                    limb.Health *= 4f;
                                    limb.InitialHealth *= 4f;
                                    limb.BaseStrength *= 2.5f;
                                    limb.SpeciesIdentity = "Protogen";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Overlay = true;
                                        Chest.OverlayModel = 11;
                                        Chest.DmgVari_Overlay = ADVCassets.ProtoChestDmg;
                                        SetLimbPhys(Chest.gameObject, "AndroidArmour");
                                        //Head.OverlaySortNum = 1;
                                        break;
                                    case "UpperLegFront":
                                        var UpperLegFront = limb.gameObject.AddComponent<LimbAddon>();
                                        //UpperLegFront.transform.localScale = new Vector3(1.1f, 1.1f);
                                        UpperLegFront.Instance = limb.gameObject;
                                        UpperLegFront.Overlay = true;
                                        UpperLegFront.OverlayModel = 10;
                                        UpperLegFront.OverlaySortNum = 1;
                                        UpperLegFront.DmgVari_Overlay = ADVCassets.ProtoThighDmg;
                                        SetLimbPhys(UpperLegFront.gameObject, "AndroidArmour");
                                        break;
                                    case "UpperLeg":
                                        var UpperLeg = limb.gameObject.AddComponent<LimbAddon>();
                                        UpperLeg.Instance = limb.gameObject;
                                        UpperLeg.Overlay = true;
                                        UpperLeg.OverlayModel = 10;
                                        UpperLeg.OverlayLayer = "Background";
                                        UpperLeg.OverlaySortNum = 2;
                                        UpperLeg.DmgVari_Overlay = ADVCassets.ProtoThighDmg;
                                        SetLimbPhys(UpperLeg.gameObject, "AndroidArmour");
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"synthmale");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DModelAdv = true;
                                        Crotch.DModelAdvSprites = ADVCassets.DickADV_Proto;
                                        Crotch.DickLength = 4; //1 less then the total dick length of the adv dick, as we're ignoring the Dick Base part of it.
                                        Crotch.DoDickAudio = true;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 3;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[2];

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 4;

                                        Crotch.CumColour = new Color32(163, 246, 240, 160);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushSortOrder = 2;
                                        Head.BlushModel = 5;
                                        Head.DmgVari_Overlay = ADVCassets.ProtoVisorDmg;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 9;
                                        SetLimbPhys(Head.gameObject, "Glass");
                                        Head.OverlaySortNum = 1;
                                        Head.LargeEar = true;
                                        Head.LargeEarModel = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["ProtoSkinMale"], ADVCassets.Skins["ProtoFlesh"], ADVCassets.Skins["ProtoBone"]);
                            }
                        }
                );
            //protogem female
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Female Protogen",
                            NameToOrderByOverride = "!!!!!anthro_protogen_female_s1",
                            DescriptionOverride = "Female Protogen Character with Biosynthetic female parts",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/proto_female.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                GenderHuman.SetRottenColour(79, 63, 63);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.RegenerationSpeed += 1f;
                                    limb.ShotDamageMultiplier *= 0.8f;
                                    limb.ImpactPainMultiplier *= 0.6f;
                                    limb.Health *= 3.8f;
                                    limb.InitialHealth *= 3.8f;
                                    limb.BaseStrength *= 2.4f;
                                    limb.SpeciesIdentity = "Protogen";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Overlay = true;
                                        Chest.OverlayModel = 14;
                                        Chest.DmgVari_Overlay = ADVCassets.ProtoChestDmg;
                                        SetLimbPhys(Chest.gameObject, "AndroidArmour");
                                        //Head.OverlaySortNum = 1;
                                        break;
                                    case "UpperLegFront":
                                        var UpperLegFront = limb.gameObject.AddComponent<LimbAddon>();
                                        //UpperLegFront.transform.localScale = new Vector3(1.1f, 1.1f);
                                        UpperLegFront.Instance = limb.gameObject;
                                        UpperLegFront.Overlay = true;
                                        UpperLegFront.OverlayModel = 13;
                                        UpperLegFront.OverlaySortNum = 1;
                                        UpperLegFront.DmgVari_Overlay = ADVCassets.ProtoThighDmg;
                                        SetLimbPhys(UpperLegFront.gameObject, "AndroidArmour");
                                        break;
                                    case "UpperLeg":
                                        var UpperLeg = limb.gameObject.AddComponent<LimbAddon>();
                                        UpperLeg.Instance = limb.gameObject;
                                        UpperLeg.Overlay = true;
                                        UpperLeg.OverlayModel = 13;
                                        UpperLeg.OverlayLayer = "Background";
                                        UpperLeg.OverlaySortNum = 2;
                                        UpperLeg.DmgVari_Overlay = ADVCassets.ProtoThighDmg;
                                        SetLimbPhys(UpperLeg.gameObject, "AndroidArmour");
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        Crotch.HasPussy = true;
                                        MakeFuckable(limb.gameObject,"synthfemale");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 3;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[2];

                                        Crotch.CumColour = new Color32(163, 246, 240, 160);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushSortOrder = 2;
                                        Head.BlushModel = 4;
                                        Head.DmgVari_Overlay = ADVCassets.ProtoVisorDmg;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 12;
                                        SetLimbPhys(Head.gameObject, "Glass");
                                        Head.OverlaySortNum = 1;
                                        Head.LargeEar = true;
                                        Head.LargeEarModel = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["ProtoSkinFemale"], ADVCassets.Skins["ProtoFlesh"], ADVCassets.Skins["ProtoBone"]);
                            }
                        }
                );
            //anthro snake male
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Male Anthro Snake",
                            NameToOrderByOverride = "!!!!!anthro_snake_male_s1",
                            DescriptionOverride = "Anthropomorphic Male Snake Character",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/snake_male.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                GenderHuman.SetBruiseColor(61, 17, 28);
                                GenderHuman.SetSecondBruiseColor(120, 20, 47);
                                GenderHuman.SetThirdBruiseColor(158, 36, 85);
                                GenderHuman.SetBloodColour(247, 67, 91);
                                GenderHuman.SetRottenColour(82, 42, 62);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Snake";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        MakeFuckable(limb.gameObject,"snake");
                                        Crotch.HasAss = true;
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DModelAdv = true;
                                        Crotch.DModelAdvSprites = ADVCassets.DickADV_Snake;
                                        Crotch.DickLength = 6; //1 less then the total dick length of the adv dick, as we're ignoring the Dick Base part of it.
                                        Crotch.DoDickAudio = true;

                                        Crotch.Tail = true;
                                        Crotch.TailModel = 4;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[3];

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 5;

                                        Crotch.CumColour = new Color32(242, 225, 245, 100);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 6;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 15;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["SnakeSkinMale"], ADVCassets.Skins["SnakeFlesh"], ADVCassets.Skins["SnakeBones"]);
                            }
                        }
                );

            //anthro snake female
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Female Anthro Snake",
                            NameToOrderByOverride = "!!!!!anthro_snake_female_s1",
                            DescriptionOverride = "Anthropomorphic Female Snake Character (The Developer)",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/snake_female.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                GenderHuman.SetBruiseColor(61, 17, 28);
                                GenderHuman.SetSecondBruiseColor(120, 20, 47);
                                GenderHuman.SetThirdBruiseColor(158, 36, 85);
                                GenderHuman.SetBloodColour(247, 67, 91);
                                GenderHuman.SetRottenColour(82, 42, 62);
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    limb.SpeciesIdentity = "Snake";
                                    switch(limb.gameObject.name) 
                                    {
                                    case "UpperBody":
                                        var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                        Chest.Instance = limb.gameObject;
                                        Chest.Tits = true;
                                        Chest.TitsModel = 3;
                                        break;
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        MakeFuckable(limb.gameObject,"snake");
                                        Crotch.HasAss = true;
                                        Crotch.HasPussy = true;
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Tail = true;
                                        Crotch.TailModel = 5;
                                        Crotch.DmgVari_Tail = ADVCassets.TailDmg[4];

                                        Crotch.CumColour = new Color32(255, 217, 253, 150);
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 7;

                                        Head.Overlay = true;
                                        Head.OverlayModel = 16;
                                        Head.OverlaySortNum = 1;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["SnakeSkinFemale"], ADVCassets.Skins["SnakeFlesh"], ADVCassets.Skins["SnakeBones"]);
                            }
                        }
                );
            //cool male human
                ModAPI.Register(
                    new Modification()
                        {
                            OriginalItem = ModAPI.FindSpawnable("Human"),
                            NameOverride = "Cool Dude",
                            NameToOrderByOverride = "!!!!!humans_cool",
                            DescriptionOverride = "A Cool Guy",
                            CategoryOverride = ModAPI.FindCategory("ACM"),
                            ThumbnailOverride = ModAPI.LoadSprite("assets/icons/human_cool.png"),
                            AfterSpawn = (Instance) =>
                            {
                                var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                                foreach (var limb in GenderHuman.Limbs)
                                {
                                    switch(limb.gameObject.name) 
                                    {
                                    case "LowerBody":
                                        var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                        Crotch.HasAss = true;
                                        MakeFuckable(limb.gameObject,"male");
                                        Crotch.IsOrgasmSource = true;
                                        Crotch.OrgasmSourceFuckable = true;

                                        Crotch.Instance = limb.gameObject;
                                        Crotch.Dick = true;
                                        Crotch.DickModel = 1;
                                        Crotch.DickLength = 7;
                                        Crotch.DoDickAudio = true;

                                        Crotch.Balls = true;
                                        Crotch.BallsModel = 1;
                                        break;
                                    case "Head":
                                        var Head = limb.gameObject.AddComponent<LimbAddon>();
                                        Head.Instance = limb.gameObject;
                                        Head.CanBlush = true;
                                        Head.BlushModel = 8;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                                SetArousalNets(GetArousalNets(GenderHuman));
                                GenderHuman.SetBodyTextures(ADVCassets.Skins["CoolGuySkin"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                            }
                        }
                );
            }

            //cosmic entity code - female
            ModAPI.Register(
                new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Human"),
                        NameOverride = "Female Cosmic Entity",
                        NameToOrderByOverride = "!!!!!humans_cosmic_female",
                        DescriptionOverride = "Cosmic Entity of unknown origin",
                        CategoryOverride = ModAPI.FindCategory("ACM"),
                        ThumbnailOverride = ModAPI.LoadSprite("assets/icons/cosmic_female.png"),
                        AfterSpawn = (Instance) =>
                        {
                            var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                            GenderHuman.gameObject.AddComponent<CosmicBody>(); // make it a cosmic being

                            GenderHuman.SetBruiseColor(158, 85, 200);
                            GenderHuman.SetSecondBruiseColor(71, 20, 106);
                            GenderHuman.SetThirdBruiseColor(156, 127, 249);
                            GenderHuman.SetBloodColour(74, 19, 123);
                            GenderHuman.SetRottenColour(70, 158, 179);
                            foreach (var limb in GenderHuman.Limbs)
                            {
                                if (limb.gameObject.GetComponent<SpriteRenderer>() != null) limb.gameObject.AddComponent<CosmicRenderer>();
                                switch(limb.gameObject.name) 
                                {
                                case "UpperBody":
                                    var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                    Chest.Instance = limb.gameObject;
                                    Chest.Tits = true;
                                    Chest.TitsModel = 1;
                                    Chest.IsCosmic = true;
                                    break;
                                case "LowerBody":
                                    var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                    Crotch.Instance = limb.gameObject;
                                    Crotch.HasPussy = true;
                                    Crotch.HasAss = true;
                                    Crotch.IsCosmic = true;
                                    Crotch.CumColour = new Color32(134, 34, 255, 72);

                                    MakeFuckable(limb.gameObject,"cosmicfemale");
                                    Crotch.IsOrgasmSource = true;
                                    Crotch.OrgasmSourceFuckable = true;
                                    break;
                                case "Head":
                                    var Head = limb.gameObject.AddComponent<LimbAddon>();
                                    Head.Instance = limb.gameObject;
                                    Head.CanBlush = true;
                                    Head.BlushModel = 1;
                                    Head.IsCosmic = true;

                                    Head.MoanSource = true;
                                    break;
                                default:
                                    break;
                                }
                            }
                            SetArousalNets(GetArousalNets(GenderHuman));
                            GenderHuman.SetBodyTextures(ADVCassets.Skins["FemaleSkin"], ADVCassets.Skins["CosmicFleshFemale"], ADVCassets.Skins["CosmicBone"]);
                        }
                    }
            );
        //cosmic entity code - male
            ModAPI.Register(
                new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Human"),
                        NameOverride = "Male Cosmic Entity",
                        NameToOrderByOverride = "!!!!!humans_cosmic_male",
                        DescriptionOverride = "Cosmic Entity of unknown origin",
                        CategoryOverride = ModAPI.FindCategory("ACM"),
                        ThumbnailOverride = ModAPI.LoadSprite("assets/icons/cosmic_male.png"),
                        AfterSpawn = (Instance) =>
                        {
                            var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                            GenderHuman.gameObject.AddComponent<CosmicBody>(); // make it a cosmic being
                            
                            GenderHuman.SetBruiseColor(158, 85, 200);
                            GenderHuman.SetSecondBruiseColor(71, 20, 106);
                            GenderHuman.SetThirdBruiseColor(156, 127, 249);
                            GenderHuman.SetBloodColour(74, 19, 123);
                            GenderHuman.SetRottenColour(70, 158, 179);
                            foreach (var limb in GenderHuman.Limbs)
                            {
                                if (limb.gameObject.GetComponent<SpriteRenderer>() != null) limb.gameObject.AddComponent<CosmicRenderer>();
                                switch(limb.gameObject.name) 
                                {
                                case "LowerBody":
                                    var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                    Crotch.HasAss = true;
                                    MakeFuckable(limb.gameObject,"cosmicmale");
                                    Crotch.IsOrgasmSource = true;
                                    Crotch.OrgasmSourceFuckable = true;

                                    Crotch.Instance = limb.gameObject;
                                    Crotch.Dick = true;
                                    Crotch.DickModel = 1;
                                    Crotch.DickLength = 5;
                                    Crotch.DoDickAudio = true;

                                    Crotch.Balls = true;
                                    Crotch.BallsModel = 1;
                                    Crotch.IsCosmic = true;
                                    Crotch.CumColour = new Color32(134, 34, 255, 72);
                                    break;
                                case "Head":
                                    var Head = limb.gameObject.AddComponent<LimbAddon>();
                                    Head.Instance = limb.gameObject;
                                    Head.CanBlush = true;
                                    Head.BlushModel = 2;
                                    Head.IsCosmic = true;
                                    break;
                                default:
                                    break;
                                }
                            }
                            SetArousalNets(GetArousalNets(GenderHuman));
                            GenderHuman.SetBodyTextures(ADVCassets.Skins["MaleSkin"], ADVCassets.Skins["CosmicFleshMale"], ADVCassets.Skins["CosmicBone"]);
                        }
                    }
            );
        //goblin male
            ModAPI.Register(
                new Modification(){
                    OriginalItem = ModAPI.FindSpawnable("Human"),
                    NameOverride = "Male Goblin",
                    NameToOrderByOverride = "!!!!!goblin_male",
                    DescriptionOverride = "Male Goblinoid Character, classic high-fantasy goblin",
                    CategoryOverride = ModAPI.FindCategory("ACM"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/icons/goblin_male.png"),
                    AfterSpawn = (Instance) =>
                    {
                        var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                        GenderHuman.SetBruiseColor(82, 84, 14);
                        GenderHuman.SetSecondBruiseColor(126, 133, 29);
                        GenderHuman.SetThirdBruiseColor(148, 181, 65);
                        GenderHuman.SetBloodColour(198, 23, 23);
                        GenderHuman.SetRottenColour(68, 105, 87);
                        Instance.transform.localScale *= 0.75f;

                        foreach (var limb in GenderHuman.Limbs){
                            limb.SpeciesIdentity = "Goblin";
                            switch(limb.gameObject.name){
                            case "LowerBody":
                                var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                Crotch.HasAss = true;
                                MakeFuckable(limb.gameObject,"goblinmale");
                                Crotch.IsOrgasmSource = true;
                                Crotch.OrgasmSourceFuckable = true;

                                Crotch.Instance = limb.gameObject;
                                Crotch.Dick = true;
                                Crotch.DickModel = 3;
                                Crotch.DickLength = 5;
                                Crotch.DoDickAudio = true;

                                Crotch.Balls = true;
                                Crotch.BallsModel = 6;

                                Crotch.CumColour = new Color32(205, 227, 172, 110);
                                break;
                            case "Head":
                                var Head = limb.gameObject.AddComponent<LimbAddon>();
                                Head.Instance = limb.gameObject;
                                Head.CanBlush = true;
                                Head.BlushModel = 9;

                                Head.Overlay = true;
                                Head.OverlayModel = 18;
                                Head.OverlaySortNum = 1;

                                Head.LargeEar = true;
                                Head.LargeEarModel = 2;
                                break;
                            default:
                                break;
                            }
                        }
                        SetArousalNets(GetArousalNets(GenderHuman));
                        GenderHuman.SetBodyTextures(ADVCassets.Skins["GoblinSkinMale"], ADVCassets.Skins["MaleFlesh"], ADVCassets.Skins["MaleBone"]);
                    }
                }
            );
        //goblin female
            ModAPI.Register(
                new Modification(){
                    OriginalItem = ModAPI.FindSpawnable("Human"),
                    NameOverride = "Female Goblin",
                    NameToOrderByOverride = "!!!!!goblin_female",
                    DescriptionOverride = "Female Shortstack Goblin Character, classic high-fantasy female goblin",
                    CategoryOverride = ModAPI.FindCategory("ACM"),
                    ThumbnailOverride = ModAPI.LoadSprite("assets/icons/goblin_female.png"),
                    AfterSpawn = (Instance) =>
                    {
                        var GenderHuman = Instance.GetComponent<PersonBehaviour>();
                        GenderHuman.SetBruiseColor(82, 84, 14);
                        GenderHuman.SetSecondBruiseColor(126, 133, 29);
                        GenderHuman.SetThirdBruiseColor(148, 181, 65);
                        GenderHuman.SetBloodColour(198, 23, 23);
                        GenderHuman.SetRottenColour(68, 105, 87);
                        Instance.transform.localScale *= 0.75f;

                        foreach (var limb in GenderHuman.Limbs)
                        {
                            limb.SpeciesIdentity = "Goblin";
                            switch(limb.gameObject.name) 
                            {
                            case "UpperBody":
                                var Chest = limb.gameObject.AddComponent<LimbAddon>();
                                Chest.Instance = limb.gameObject;
                                
                                Chest.Tits = true;
                                Chest.TitsJiggleAmt = 2.5f;
                                if (Config.NSFW_MODE) Chest.TitsModel = 5;
                                else Chest.TitsModel = 4;
                                break;
                            case "LowerBody":
                                var Crotch = limb.gameObject.AddComponent<LimbAddon>();
                                Crotch.HasAss = true;
                                Crotch.HasPussy = true;
                                MakeFuckable(limb.gameObject,"goblinfemale");
                                Crotch.IsOrgasmSource = true;
                                Crotch.OrgasmSourceFuckable = true;
                                Crotch.Instance = limb.gameObject;

                                Crotch.CumColour = new Color32(205, 227, 172, 110);
                                break;
                            case "Head":
                                var Head = limb.gameObject.AddComponent<LimbAddon>();
                                Head.Instance = limb.gameObject;
                                Head.CanBlush = true;
                                Head.BlushModel = 10;
                                Head.MoanSource = true; //experimental

                                Head.Overlay = true;
                                Head.OverlayModel = 17;
                                Head.OverlaySortNum = 1;

                                Head.LargeEar = true;
                                Head.LargeEarModel = 2;
                                break;
                            default:
                                break;
                            }
                        }
                        SetArousalNets(GetArousalNets(GenderHuman));
                        GenderHuman.SetBodyTextures(ADVCassets.Skins["GoblinSkinFemale"], ADVCassets.Skins["FemaleFlesh"], ADVCassets.Skins["FemaleBone"]);
                    }
                }
            );
        }

        public static void SetLimbPhys(GameObject PhysGO, string Mat) //shortcut method used to easily set Phys Properties of a limb 'n such.
        {
            PhysGO.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties(Mat);
        }

        public static void MakeFuckable(GameObject theObjlimb) //sets it to be fuckable
        {
            theObjlimb.name = ("Fuckable" + theObjlimb.name);
        }
        public static void MakeFuckable(GameObject theObjlimb,string sexType) //sets it to be fuckable, with a sex
        {
            theObjlimb.name = ("Fuckable_" + sexType + theObjlimb.name);
            theObjlimb.GetComponent<LimbAddon>().CustomSexInfo = sexType;
        }

        public static GameObject[] GetArousalNets(PersonBehaviour GenderHuman){
            GameObject[] NetTemp = new GameObject[] {};
            foreach (var limb in GenderHuman.Limbs)
            {
                if (!(limb.gameObject.GetComponent<LimbAddon>() == null)){
                    NetTemp = NetTemp.Concat(new[] {limb.gameObject}).ToArray();
                }
            }
            return(NetTemp);
        }

        public static void SetArousalNets(GameObject[] ArousalNet){
            for(int i = 0; i < ArousalNet.Length; i++)
            {
                LimbAddon TempGenit = ArousalNet[i].GetComponent<LimbAddon>();
                TempGenit.ArousalNetwork = ArousalNet;
                //UnityEngine.Debug.Log("SetArousalNets : " + TempGenit.ArousalNetwork[0].name);
            }
        }

        public static void RegistAllClothing(){
            for(int i = 0; i < ADVCassets.ClothingNameIds.Length; i++){
                if (ADVCassets.ClothingNsfwStatus[i] && !Config.NSFW_MODE) continue; //cannot register if in sfw mode and it is a nsfw clothing item, skips it to next.
                RegistClothing(i);
            }
        }

        public static void RegistClothing(int ClothingID){
            ModAPI.Register(
                new Modification(){
                    OriginalItem = ModAPI.FindSpawnable("Brick"),
                    NameOverride = ADVCassets.ClothingNameIds[ClothingID],
                    NameToOrderByOverride = "!!!!!objs_clothing_" + ClothingID,
                    DescriptionOverride = ADVCassets.ClothingDescs[ClothingID],
                    CategoryOverride = ModAPI.FindCategory("ACM"),
                    ThumbnailOverride = ADVCassets.ClothingIconTextures[ClothingID],
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ADVCassets.ClothingObjTextures[ClothingID]; 
                        PhysicalBehaviour PhysBehav = Instance.GetComponent<PhysicalBehaviour>();
                        PhysBehav.InitialMass = 0.25f;
                        PhysBehav.TrueInitialMass = 0.25f;
                        PhysBehav.Properties = ModAPI.FindPhysicalProperties("Plastic");
                        Utils.FixColliders(Instance.gameObject);

                        AubClothingObj ClothingObj = Instance.AddComponent<AubClothingObj>();
                        ClothingObj.clothingVisibleName = ADVCassets.ClothingNameIds[ClothingID];
                        ClothingObj.clothingIndex = ClothingID;
                        ClothingObj.clothingSlot = ADVCassets.ClothingSlotIds[ClothingID];
                        ClothingObj.texBody = ADVCassets.ClothingTexTextures[ClothingID];
                        ClothingObj.texIcon = ADVCassets.ClothingIconTextures[ClothingID];
                        ClothingObj.texObj = ADVCassets.ClothingObjTextures[ClothingID];
                        ClothingObj.setup = true;
                    }
                }
            );
        }

        public static void RegistDildo(int DildoID, string Name, string Description, int Length = 7, bool ADVOverride = false, Sprite[] DickADV = null, int ballsID = 5, int dickModelOverride = -1){
            ModAPI.Register(
                new Modification(){
                    OriginalItem = ModAPI.FindSpawnable("Brick"),
                    NameOverride = Name,
                    NameToOrderByOverride = "!!!!!objs_dildo_" + DildoID,
                    DescriptionOverride = Description,
                    CategoryOverride = ModAPI.FindCategory("ACM"),
                    ThumbnailOverride = ADVCassets.DildoIcons[DildoID - 1],
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ADVCassets.DildoMain[0]; 
                        PhysicalBehaviour PhysBehav = Instance.GetComponent<PhysicalBehaviour>();
                        PhysBehav.InitialMass = 0.25f;
                        PhysBehav.TrueInitialMass = 0.25f;
                        PhysBehav.Properties = ModAPI.FindPhysicalProperties("Rubber");

                        LimbAddon Dildo = Instance.AddComponent<LimbAddon>();
                        Dildo.Instance = Instance;
                        Dildo.Dick = true;
                        Dildo.DoDickAudio = true;
                        Dildo.IsDildo = true;
                        if (ADVOverride){//adv dick
                            Dildo.DickModel = 1;
                            Dildo.DModelAdv = true;
                            Dildo.DModelAdvSprites = DickADV;
                            Dildo.DickLength = (DickADV.Length - 1);

                            Dildo.Balls = true;
                            Dildo.BallsModel = ballsID;
                        }else{//normal non-adv dick
                            if (dickModelOverride != -1) Dildo.DickModel = dickModelOverride;
                            else Dildo.DickModel = DildoID;
                            Dildo.DickLength = Length;
                            Dildo.Balls = true;
                            Dildo.BallsModel = DildoID;
                            if (ballsID != 5) Dildo.BallsModel = ballsID;
                        }
                    }
                }
            );
        }
	}
}
