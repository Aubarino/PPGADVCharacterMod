using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class LimbAddon : MonoBehaviour //add to a human limb.
{
    //What Genital Parts exist on this human limb, and their stats.
    //This Only affects the original creation stage.

    public string CustomSexInfo = "undefined"; //the value to define the sex, male, female, snake?, male_synth, female_synth 

    public bool GenitalsDestroyed = false; //internal variable used for checking if the LimbAddon are gone or not.
    private bool SpawnCounter = false; //debug stuff
    private bool DupeSpawned = false; //if this isn't the first time it was spawned
    public bool Dick = false; //has dick or not
    public int DickModel = 1; //the dick model
    public int DickLength = 2; //dick segment size
    public bool IsDildo = false; //dildo functionality 'n removed forced limb checks
    public bool DModelAdv = false;
    public Sprite[] DModelAdvSprites = new Sprite[] {};
    public float ErectionSpeed = 0.05f; //can be editted in runtime, Used for the Speed of an erection

    //dick Audio! variables
    public bool DoDickAudio = false; //if you want audio, enable this. Do Not change in runtime.
    public AudioClip[] DickSfxNormal = ADVCassets.WetSlapNormal;
    public AudioClip[] DickSfxWeak = ADVCassets.WetSlapWeak;
    public AudioSource DickAudio; //internal variable used to store AudioSource component for easy access.

    public bool CockErect = false;//do not touch in runtime
    public float ErectionAmp = -1;
    
    //read these if you want in a script- do not edit.
    public float DickDeflectFactor = 0f; //internal variable used for dick reaction to objects
    public int DickInsertDist = 0;
    public bool DickIn = false; //internal variable used for penetration

    //random useful variable of cancel-out or change dick offsets and balls offsets
    public Vector3 DickOffsetVari = new Vector3(0,0); //adds onto the offset of the dick base and balls!

    //tit variables
    public bool Tits = false;
    public int TitsModel = 1;
    public float TitsJiggleAmt = 1; //the jiggle multi of the tits

    //balls variables
    public bool Balls = false;
    public int BallsModel = 1;

    //anthro stuff
    public bool Tail = false;
    public int TailModel = 1;

    public bool Ears = false;
    public int EarModel = 1;

    public bool LargeEar = false;
    public int LargeEarModel = 1;
    
    //damage overlays 'n such
    public Sprite DmgVari_Tail = null;
    public Sprite DmgVari_Overlay = null;
    public bool Overlay = false;
    public int OverlayModel = 1;
    public Vector3 OverlayOffset = new Vector3(0,0);
    public string OverlayLayer = "Foreground";
    public int OverlaySortNum = 999; //999 is bassically not used aka NULL!, Use this if ya wanna ignore this
    public Color OverlayColour = UnityEngine.Color.white;
    public string OverlayMaterial = "Sprites-Default";

    //other lower body variables
    public bool HasAss = false;
    public bool HasPussy = false;
    public bool IsCosmic = false;
    public bool IsOrgasmSource = false; //does this limb orgasm, very general variable used with others to control stuff
    public bool OrgasmSourceFuckable = false; //does this limb orgasm from fucking, needs IsOrgasmSource also though.
    public bool MoanSource = false; //if this limb moans, experimental and may never be used

    //expression elements (using head)
    public bool CanBlush;
    public int BlushModel = 1; //NEED to define this by-hand if CanBlush is true.
    public int BlushSortOrder = 1;

    //freaky stuff
    public float Inflation = 0; //the total inflation size bias
    public float InflationEnergy = 0; //the amount of inflation left to go into the size
    public float InflationRateDelay = 0.03f; //this value gets cascadingly faster, so be careful
    public bool Growing = false;

    //arousal variables
    public GameObject[] ArousalNetwork = new GameObject[] {}; //used for pleasure expression such as blushing, shares Arousal variable across multiple objects.
    public int Arousal = 0; //general variable for all limbs that have genital code attached, DO NOT EDIT BY-HAND, this desyncs as-is with the timer per-limb sometimes too.
    public float ArousalDrain = 1.7f; //this limb's specific Arousal drain speed in seconds.
    public int OrgasmPeak = 175; //the cap of Arousal that triggers orgasm, used in certain things only
    public float OrgasmMultiplier = 5f; //makes orgasms more powerful, aka drains faster
    private float LastOrgasmTickTime = 0; //used for time calc relating to orgasm ending time
    public float LastCumTickTime = 0; //used for time calc relating to orgasm cum ticks
    private float LastADrainTime = 0; //used for time calc relating to drain speed
    public bool DoingOrgasm = false; //READ ONLY PLEASE, this is used for checking if the orgasm is happening on a limb.
    public int CumAmt = 60;
    public Color CumColour = new Color32(244, 245, 241, 140);
    

    public PhysicalBehaviour Phys;
    //object carry-over variables
    public GameObject Instance;
    public GameObject ObjectPassOut; //used to carry-over gameobjects from "CreatePart", earlier this was Manually Set but now it just returns to a variable.
    private bool HasBeenSpawnedAlready;

    private void Awake(){
        if (this.gameObject.transform.childCount > 10){
            DupeSpawned = true;
            SpawnCounter = true;
        }
    }
    private void Start()
    {
        if (Phys == null) Phys = this.gameObject.GetComponent<PhysicalBehaviour>(); //unused?
        if (!DupeSpawned){
            if (IsDildo){
                Utils.FixColliders(this.gameObject);
            }
            if (HasBeenSpawnedAlready != true){
                HasBeenSpawnedAlready = true;
                if (DoDickAudio) DickAudio = Instance.AddComponent<AudioSource>();
                InitCreationChecks();
            }
        }
    }
    
    public void InitCreationChecks() //all the checks that trigger the creation of BodyPartInit things.
    {
        if (!DupeSpawned){

        if (IsDildo) DickOffsetVari = new Vector3(-0.16f,0.10f);
        if (Dick) BodyPartInit("Dick", true);
        if (Tits) BodyPartInit("Tits", false);
        if (Balls) BodyPartInit("Balls", true);
        if (CanBlush) BodyPartInit("Blush", false);
        if (Tail) BodyPartInit("Tail", false);
        if (Ears) BodyPartInit("Ears", false);
        if (Overlay) BodyPartInit("Overlay", false);
        if (HasPussy) BodyPartInit("Pussy", true);
        if (LargeEar) BodyPartInit("LargeEar", false);
        }
        CreateContextOptions();
    }

    private void CreateContextOptions(){
        List<ContextMenuButton> contexbuttonlist = Phys.ContextMenuOptions.Buttons;
        if (!IsDildo){
            DefinePoses();
            ContextMenuButton contextMenuButtonHumping = new ContextMenuButton((Func<bool>) (() => (GetComponent<LimbBehaviour>().Person.OverridePoseIndex != 9)), "startHump", "Humping pose", "Forces the humping animation override", new UnityAction[1]{
                (UnityAction) (() => GetComponent<LimbBehaviour>().Person.OverridePoseIndex = 9)
            });
            contextMenuButtonHumping.LabelWhenMultipleAreSelected = "Humping pose";
            contexbuttonlist.Add(contextMenuButtonHumping);
        }

        if (Config.NSFW_MODE && Config.Inflation){
            ContextMenuButton menuButtonInflation = new ContextMenuButton((Func<bool>) (() => (true)),
            "inflate", "Inflation", "Inflates the Limb Addon", new UnityAction[1]{
                (UnityAction) (() => {
                    GetComponent<LimbAddon>().InflationEnergy += 0.5f;
                    GetComponent<LimbAddon>().Growing = true;
                })
            });

            menuButtonInflation.LabelWhenMultipleAreSelected = "Inflation";
            contexbuttonlist.Add(menuButtonInflation);
        }
    }

    public void DefinePoses(){
        // private RagdollPose GenerateBasePose() => new RagdollPose()
        // {
        //     Name = "Humping pose",
        //     Rigidity = 5f,
        //     Angles = new List<RagdollPose.LimbPose>(((IEnumerable<LimbBehaviour>) this.GetComponentsInChildren<LimbBehaviour>())
        //     .Where<LimbBehaviour>((Func<LimbBehaviour, bool>)
        //     (l => (bool) (UnityEngine.Object) l.GetComponent<HingeJoint2D>())).Select<LimbBehaviour, RagdollPose.LimbPose>((Func<LimbBehaviour, RagdollPose.LimbPose>) (l => new RagdollPose.LimbPose(l, l.GetComponent<HingeJoint2D>().jointAngle))))
        // };

        // this.Poses.Clear();
        // foreach (PoseState poseState in Enum.GetValues(typeof (PoseState)))
        // {
        // RagdollPose basePose = this.GenerateBasePose();
        // basePose.Name = poseState.ToString();
        // basePose.State = poseState;
        // this.Poses.Add(basePose);
        // }

        RagdollPose HumpingPose = new RagdollPose();
        HumpingPose.ShouldStandUpright = true;
        HumpingPose.ShouldStumble = true;
        HumpingPose.UprightForceMultiplier = 2f;
        HumpingPose.AnimationSpeedMultiplier = 2f;
        LimbBehaviour[] EveryPoseLimb = GetComponent<LimbBehaviour>().Person.Limbs;

        List<RagdollPose.LimbPose> Angles = new List<RagdollPose.LimbPose>();
        foreach (LimbBehaviour PoseyLimb in EveryPoseLimb){
            float Angle = 0;
            switch(PoseyLimb.gameObject.name) {//pose limb(s) angle information
                case "Head": Angle = 10; break;
                case "UpperBody": Angle = 0; break; //pos is inwards? neg is backwards?
                case "LowerBody": Angle = 30; break;
                case "UpperLegFront": Angle = -30; break;
                case "LowerLegFront": Angle = -30; break;
                case "FootFront": Angle = 30; break;
                case "UpperLeg": Angle = -30; break;
                case "LowerLeg": Angle = -30; break;
                case "Foot": Angle = 30; break;
                case "UpperArmFront": Angle = -40; break;
                case "UpperArm": Angle = -40; break;
                default: Angle = 0; break;
            }
            RagdollPose.LimbPose NewPose = new RagdollPose.LimbPose(PoseyLimb, Angle){};
            NewPose.Angle = Angle;
            NewPose.StartAngle = Angle;

            switch(PoseyLimb.gameObject.name) {//pose limb(s) angle information
                case "Head": NewPose.EndAngle = -20; break;
                case "UpperBody": NewPose.EndAngle = 0; break; //pos is inwards? neg is backwards?
                case "LowerBody": NewPose.EndAngle = -30; break;
                case "UpperLegFront": NewPose.EndAngle = 30; break;
                case "LowerLegFront": NewPose.EndAngle = 30; break;
                case "FootFront": NewPose.EndAngle = -30; break;
                case "UpperLeg": NewPose.EndAngle = 30; break;
                case "LowerLeg": NewPose.EndAngle = 30; break;
                case "Foot": NewPose.EndAngle = -30; break;
                case "UpperArmFront": NewPose.EndAngle = 60; break;
                case "UpperArm": NewPose.EndAngle = 60; break;
                default: NewPose.EndAngle = Angle; break;
            }
            NewPose.RandomInfluence = 0.01f;
            NewPose.RandomSpeed = 9240f;
            //NewPose.AnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1f));
            NewPose.Limb = PoseyLimb;
            NewPose.Animated = true;
            NewPose.AnimationDuration = 0.5f;

            Angles.Add(NewPose);
        }
        HumpingPose.Angles = Angles;

        GetComponent<LimbBehaviour>().Person.Poses.Add(HumpingPose);
        foreach (RagdollPose pose in GetComponent<LimbBehaviour>().Person.Poses){
            pose.ConstructDictionary();
        }
    }
    
    public void Use(ActivationPropagation activation)
    {
        if(AAS.BootCore && !IsDildo){
        if (CockErect) {CockErect = false;
        }else{CockErect = true;
        };
        }
    }

    public void FixedUpdate() {
        if ((!CockErect) && IsDildo) CockErect = true;
        if (CockErect){
            if (ErectionAmp < 0.225f) ErectionAmp += ErectionSpeed;
        }else{
            if (ErectionAmp > -1) ErectionAmp -= ErectionSpeed;
        }

        if (Inflation < (InflationEnergy - 0.02f)){
            Inflation += (InflationEnergy - Inflation) * InflationRateDelay;
        }else Growing = false;

        //if the human is healed after LimbAddon are destroyed- bring them back.
        if (!IsDildo && !DupeSpawned){
            if (GenitalsDestroyed && !(Instance.GetComponent<LimbBehaviour>().Health < 0.0001f)) {
                Arousal = 0;
                InitCreationChecks();
                GenitalsDestroyed = false;
            }
        }

        if (Arousal > 3) { //every (whatever arousal drain is) in seconds, remove 1 from Local Limb Arousal.
            if (UnityEngine.Time.fixedTime > (LastADrainTime + ArousalDrain)){
                LastADrainTime = UnityEngine.Time.fixedTime;
                Arousal -= 4;
            }
        }
        if (IsOrgasmSource) OrgasmCheck();
    }

    public void BodyPartInit(string ThePart, bool nsfw) //first creation of body parts. this is where all of 'em are defined.
    {
        if ((Config.NSFW_MODE && nsfw) || !nsfw){ //check for nsfw configs
            switch(ThePart) 
            {
            case "Dick": //dick code
                Sprite DickBaseSprite = ADVCassets.DickBases[DickModel - 1];
                if (DModelAdv) DickBaseSprite = DModelAdvSprites[0];
                Vector3 DickSpawnPos = new Vector3(0.12f,-0.10f);
                //Debug.Log("trying to create dick - start 1");
                GameObject DickBase = CreatePart("DickBase", DickBaseSprite, "Default", (DickSpawnPos + DickOffsetVari), Instance, Instance, 999);
                ObjectPassOut = DickBase;
                DickBase.GetComponent<LimbAddonPart>().CanGrow = true;
                var GenitPartTemp = DickBase.GetComponent<LimbAddonPart>();
                //Debug.Log("trying to create dick - start 2");
                GenitPartTemp.JiggleRange = 10f;
                GenitPartTemp.JiggleGravity = 5f;
                GenitPartTemp.JiggleAmbpli = 0.8f;
                //Debug.Log("trying to create dick");
                for(int i = 1; i <= DickLength; i++){
                    Sprite DickTip = ADVCassets.DickTips[DickModel - 1];
                    Sprite DickMid = ADVCassets.DickMids[DickModel - 1]; 
                    if (DModelAdv) {
                        DickMid = DModelAdvSprites[i];
                        DickTip = DModelAdvSprites[DModelAdvSprites.Length - 1];
                    }
                    //DModelAdv
                    if (i == DickLength) {ObjectPassOut = CreatePart("DickTip", DickTip, "Background", new Vector3(0.055f,0), ObjectPassOut, Instance, 999,DickBase,true);
                    }else{
                        ObjectPassOut = CreatePart("DickMid", DickMid, "Background", new Vector3(0.05f,0), ObjectPassOut, Instance, 999,DickBase,true);
                    }//if the tip of a dick, then it's the tip
                    var GenitPartTemp2 = ObjectPassOut.GetComponent<LimbAddonPart>();
                    GenitPartTemp2.CanErect = true;
                    GenitPartTemp2.DickPartNum = i;
                    GenitPartTemp2.JiggleRange = 30f;
                    GenitPartTemp2.JiggleGravity = 5f;
                    GenitPartTemp2.JiggleAmbpli = 0.5f;
                }
                if (IsDildo) this.gameObject.GetComponent<SpriteRenderer>().sprite = ADVCassets.DildoMain[1]; 
                //Debug.Log("trying to create dick!! DONE");
                break;
            case "Tits": //tits code
                ObjectPassOut = CreatePart("TitBody", ADVCassets.TitBases[TitsModel - 1], "Foreground", new Vector3(0,0), Instance, Instance, 999);
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleAmbpli = 0.18f * (0.8f + (TitsJiggleAmt * 0.2f));
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleRange = 140 * (TitsJiggleAmt * 0.25f); //should be 35 if you multi
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleGravity = (TitsJiggleAmt - 1) * 0.5f;
                ObjectPassOut.GetComponent<LimbAddonPart>().CanGrow = true;
                ObjectPassOut.GetComponent<LimbAddonPart>().IsTits = true;
                ObjectPassOut = CreatePart("TitTip", ADVCassets.TitTips[TitsModel - 1], "Foreground", new Vector3(0,0), Instance, Instance, 1);
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleAmbpli = 0.18f * (0.8f + (TitsJiggleAmt * 0.2f));
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleRange = 140 * (TitsJiggleAmt * 0.25f); //should be 35 if you multi
                ObjectPassOut.GetComponent<LimbAddonPart>().JiggleGravity = (TitsJiggleAmt - 1) * 0.5f;
                ObjectPassOut.GetComponent<LimbAddonPart>().CanGrow = true;
                ObjectPassOut.GetComponent<LimbAddonPart>().IsTits = true;
                break;
            case "Balls": //balls code
                ObjectPassOut = CreatePart("Balls", ADVCassets.DickBalls[BallsModel - 1], "Background", (new Vector3(0.12f,-0.15f) + DickOffsetVari), Instance, Instance, 999);
                ObjectPassOut.GetComponent<LimbAddonPart>().CanGrow = true;
                break;
            case "Blush": //blush code
                ObjectPassOut = CreatePart("Blush", ADVCassets.Blush[BlushModel - 1], "Foreground", new Vector3(0,0), Instance, Instance, BlushSortOrder);
                var BlushTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                BlushTemp.ArousalRequired = 15;
                BlushTemp.JiggleRange = 0;
                BlushTemp.JiggleAmbpli = 0;
                break;
            case "Tail": //tail code
                LimbAddonPart TailTemp;
                ObjectPassOut = CreatePart("TailStart", ADVCassets.TailStart[TailModel - 1], "Default", new Vector3(-0.04f,-0.11f), Instance, Instance, 999);
                TailTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                if (DmgVari_Tail != null) {
                    TailTemp.DmgVariSprite = DmgVari_Tail;
                    TailTemp.DoDmgVari = true;
                }
                TailTemp.JiggleRange = 25;
                TailTemp.JiggleAmbpli = 0.25f;
                TailTemp.JiggleGravity = 1;
                TailTemp.RotationalMultiplier = -2;
                ObjectPassOut = CreatePart("TailMid", ADVCassets.TailMid[TailModel - 1], "Default", new Vector3(-0.06f,-0.12f), ObjectPassOut, Instance, 999);
                TailTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                TailTemp.JiggleRange = 25;
                TailTemp.JiggleAmbpli = 0.2f;
                TailTemp.JiggleGravity = 1;
                TailTemp.RotationalMultiplier = -2;
                ObjectPassOut = CreatePart("TailTip", ADVCassets.TailTip[TailModel - 1], "Default", new Vector3(-0.06f,-0.14f), ObjectPassOut, Instance, 999);
                TailTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                TailTemp.JiggleRange = 25;
                TailTemp.JiggleAmbpli = 0.15f;
                TailTemp.JiggleGravity = 1;
                TailTemp.RotationalMultiplier = -2;
                break;
            case "Ears": //anthro ear set code
                ObjectPassOut = CreatePart("EarLeft", ADVCassets.EarLeft[EarModel - 1], "Default", new Vector3(-0.07f,0.16f), Instance, Instance, 999);
                var EarTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                EarTemp.JiggleRange = 45;
                EarTemp.JiggleAmbpli = 0.12f;
                ObjectPassOut = CreatePart("EarRight", ADVCassets.EarRight[EarModel - 1], "Default", new Vector3(0.07f,0.16f), Instance, Instance, 999);
                EarTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                EarTemp.JiggleRange = 45;
                EarTemp.JiggleAmbpli = 0.12f;
                break;
            case "Overlay": //very useful general-usage overlay! with Many customisation options per-limb
                ObjectPassOut = CreatePart("Overlay", ADVCassets.Overlays[OverlayModel - 1], OverlayLayer, OverlayOffset, Instance, Instance, OverlaySortNum);
                var OverlayTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                OverlayTemp.JiggleRange = 0;
                OverlayTemp.JiggleAmbpli = 0;
                ObjectPassOut.GetComponent<SpriteRenderer>().material = ModAPI.FindMaterial(OverlayMaterial);
                ObjectPassOut.GetComponent<SpriteRenderer>().material.color = OverlayColour;
                if (DmgVari_Overlay != null) {
                    OverlayTemp.DmgVariSprite = DmgVari_Overlay;
                    OverlayTemp.DoDmgVari = true;
                }
                break;
            case "Pussy": //pussy code, doesn't really render anything- just acts as CUM functionality 'n etc O//_//O damn
                ObjectPassOut = CreatePart("Pussy", ADVCassets.DevNullSprite, "Default", new Vector3(0.13f,-0.08f), Instance, Instance, 1);
                var PussyTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                //CumAmt = 35;
                PussyTemp.JiggleRange = 0;
                PussyTemp.JiggleAmbpli = 0;
                break;
            case "LargeEar": //large single anthro ear code, simular to ear code but single and based off the center of the object / human limb.
                ObjectPassOut = CreatePart("LargeEar", ADVCassets.EarLarge[LargeEarModel - 1], "Foreground", new Vector3(0,0), Instance, Instance, 999);
                var LargeEarTemp = ObjectPassOut.GetComponent<LimbAddonPart>();
                LargeEarTemp.JiggleRange = 45;
                LargeEarTemp.JiggleAmbpli = 0.12f;
                break;
            default:
                break;
            }
        }
    }

    public GameObject CreatePart(string Name, Sprite PartSprite, string Layer, Vector3 Objoffset, GameObject PartHost, GameObject PartChainHost, int SortingNum, GameObject PartChainSemiHost = null, bool doChainSemi = false)
    { //used to create limb attachments that use the "LimbAddonPart" script, this can be used for more then just Genital, such as ears and cool overlays!
        var NewPart = ModAPI.CreatePhysicalObject(Name, PartSprite);
        // ^ ^ ^ ^ i'm lazy, ignore this- works just as fine honestly- also means less conflictions with ppg updates i gotta patch.

        Destroy(NewPart.GetComponent<PhysicalBehaviour>());
        Destroy(NewPart.GetComponent<BoxCollider2D>());
        Destroy(NewPart.GetComponent<Rigidbody2D>());

        NewPart.transform.SetParent(PartHost.transform, false);
        NewPart.transform.localPosition = Objoffset;
        NewPart.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        NewPart.transform.localScale = new Vector3(1f, 1f);

        NewPart.GetComponent<SpriteRenderer>().sortingLayerName = Layer;
        if (SortingNum != 999) NewPart.GetComponent<SpriteRenderer>().sortingOrder = SortingNum; //999 is bassically null here.

        if (IsCosmic) NewPart.AddComponent<CosmicRenderer>(); //cosmic effect

        NewPart.AddComponent<LimbAddonPart>();
        var GenitPartCom = NewPart.GetComponent<LimbAddonPart>();
        GenitPartCom.PartHost = PartHost;
        GenitPartCom.PartChainHost = PartChainHost;
        if (!doChainSemi) GenitPartCom.PartChainSemiHost = PartChainHost;
        else {
            GenitPartCom.PartChainSemiHost = PartChainSemiHost;
        }
        GenitPartCom.PartObj = NewPart;
        GenitPartCom.Objoffset = Objoffset;
        GenitPartCom.PartSprite = PartSprite;
        return(NewPart);
    }

    public void OrgasmCheck(){
        if (Config.Furry_Mode){
            OrgasmPeak = 175; //the cap of Arousal that triggers orgasm, used in certain things only
            OrgasmMultiplier = 5f; //makes orgasms more powerful, aka drains faster
        }else{
            OrgasmPeak = 100; //the cap of Arousal that triggers orgasm, used in certain things only
            OrgasmMultiplier = 8f; //makes orgasms more powerful, aka drains faster
        }

        if (Arousal > OrgasmPeak) DoingOrgasm = true;
        if (DoingOrgasm){
            if (Arousal > 0){
                if (LastOrgasmTickTime < UnityEngine.Time.fixedTime){
                    LastOrgasmTickTime = UnityEngine.Time.fixedTime + 0.1f;
                    AddArousal((int) Mathf.Round(-1 * OrgasmMultiplier));
                }
            }else{
                Debug.Log("ending orgasm");
                DoingOrgasm = false;
            }
        }
    }

    public void AddArousal(int AmtAdding){ //do not manually edit Arousal, always use this Method.
        if (!IsDildo){
            for(int i = 0; i < ArousalNetwork.Length; i++) //adds Arousal or removes, to every linked part on the arousal network linked, including itself!
            {
                ArousalNetwork[i].GetComponent<LimbAddon>().Arousal += AmtAdding;
                ArousalNetwork[i].GetComponent<LimbAddon>().Arousal = Mathf.Clamp(ArousalNetwork[i].GetComponent<LimbAddon>().Arousal,0,999999); //FUCK YOU!
            }
        }
    }
}

public class LimbAddonPart : MonoBehaviour //individual BodyPart
{
    public bool CanErect = false; //used internally in the body part init creation, do not change manually- atleast here.
    public GameObject PartChainHost; //the source of all parents in a chain or as-is what it is mainly attached to.
    public GameObject PartChainSemiHost; //the source of almost all parents, so the base of a dick- for example
    public GameObject PartHost; //what this is Personally parented to - not used much but still important to define.
    public GameObject PartObj; //THIS PART
    public bool CanGrow = false;
    public bool IsTits = false; //used to make the grow feature offset it if its tits
    public bool Growing = false;
    public Vector2 StartScale; //set auto on start
    public float JiggleRange = 35f; //should define this, otherwise it defaults.
    public float JiggleAmbpli = 0.12f; //should define this, otherwise it defaults.
    public float JiggleGravity = 0f;
    public Vector3 Objoffset = new Vector3(0,0); //not easy to edit in runtime besides original creation

    public Sprite PartSprite;
    public GameObject GenitalParticlesObj;
    private ParticleSystem GPObjParticlesElement;

    //internal variables, do not set in runtime by hand outside of this script
    private Rigidbody2D HostRB;
    public float jiggle;
    public float jiggle_change = 0;
    private float jiggle_rotation;
    private float velocity_y; //internal velocity used for jiggle mechanics
    private LimbAddon HostGenitals; //used internally, ignore this
    public float RotationalMultiplier = 1f;
    private float TotalDistance;
    private float HitDistance;
    private bool ContainsFuckable;
    private Vector2 semiScale;

    public int ArousalRequired = 0; //set to anything above 0 to limit this to render Only Above a certain Arousal amount.
    private bool RenderCheck = true; //internal variable used to toggle between rendering.
    private bool DmgVariActive = false;
    private bool DmbDelNext = false; //used to remove the damage texture thing after its been active once
    public bool DoDmgVari = false;
    public Sprite DmgVariSprite;
    public int DickPartNum = 0; //used to determine what part of a dick this genital part is- if it is indeed a dick part.

    private RaycastHit2D HitSurface; //temp, ignore this
    private int testCast;
    private ContactFilter2D genitalHitFilter; //ignore this

    private Vector4 DickDebugPosStuff;
    private void Awake(){
    }

    void Start() {
        genitalHitFilter = new ContactFilter2D();
        genitalHitFilter.layerMask = 1 << LayerMask.NameToLayer("Objects");
        HostRB = PartChainHost.GetComponent<Rigidbody2D>();
        HostGenitals = PartChainHost.GetComponent<LimbAddon>();
        if ((PartObj.name == "DickTip") || (PartObj.name == "Pussy")){ //particle definition stuff for cumming
            GenitalParticlesObj = new GameObject("GenitalParticles");
            GPObjParticlesElement = GenitalParticlesObj.AddComponent<ParticleSystem>();
            GenitalParticlesObj.transform.SetParent(PartObj.transform, false);
            GenitalParticlesObj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (PartObj.name == "DickTip"){
                GenitalParticlesObj.transform.localScale = new Vector2(0.05f, 0.05f);
                GenitalParticlesObj.transform.localPosition = new Vector3(0.085f,0);
                DefineParticles(ADVCassets.ParticleTextures[0]);
            }else{
                GenitalParticlesObj.transform.localScale = new Vector2(0.05f, 0.05f);
                GenitalParticlesObj.transform.localPosition = new Vector3(0,0);
                DefineParticles(ADVCassets.ParticleTextures[0]);
            }
        }
        StartScale = PartObj.transform.localScale;
    }

    void FixedUpdate() {

        bool DickPartRenderOverride = true; //handles the fancy rendering of dicks disappearing inside something.
        if (((PartObj.name == "DickTip") || (PartObj.name == "DickMid")) && (HostGenitals.DickInsertDist < DickPartNum)) {
        DickPartRenderOverride = false;
        }

        if ((HostGenitals.Arousal >= ArousalRequired) && DickPartRenderOverride){ //rendering
            if (!RenderCheck){
                PartObj.GetComponent<SpriteRenderer>().sprite = PartSprite;
                RenderCheck = true;
            }
        }else{
            if (RenderCheck){
                PartObj.GetComponent<SpriteRenderer>().sprite = ADVCassets.DevNullSprite;
                RenderCheck = false;
            }
        }

        bool DDNoSkip = true;
        if (!HostGenitals.IsDildo){
            if ((PartChainHost.GetComponent<LimbBehaviour>().Health < 0.0001f)){ //destruction code for when health hits 0
                if (DoDmgVari){
                    DmbDelNext = true;
                    if (!DmgVariActive){
                        DmgVariActive = true;
                        PartObj.GetComponent<SpriteRenderer>().sprite = DmgVariSprite;
                    }
                }else{
                    Destroy(PartObj);
                    HostGenitals.GenitalsDestroyed = true;
                }
                DDNoSkip = false;
            }
        }
        if (DDNoSkip){
            if (DmgVariActive){
                DmgVariActive = false;
                PartObj.GetComponent<SpriteRenderer>().sprite = PartSprite;

                if (DmbDelNext == true){ //removes the damage texture object so a new one can take its place on creation of a new set, so parenting works right instead of bullshit carry-over
                    Destroy(PartObj);
                }
            }
        }

        float DInteractionFactor = Mathf.Clamp((Mathf.Clamp(HostGenitals.ErectionAmp + 1.4f, 0f, 1f) - HostGenitals.DickDeflectFactor),0f,1f);
        if (CanErect){ //if it can erect, it can.
            PartObj.transform.localPosition = new Vector3((Objoffset.x * DInteractionFactor), PartObj.transform.localPosition.y);
        }

        velocity_y = (-HostRB.velocity.y * PartObj.transform.up.y);
        //main jiggle physics algorithm
        jiggle_change = (Mathf.Clamp(jiggle_change + (velocity_y * 0.25f), -1.6f, 1.6f));
        float jiggle_rng = UnityEngine.Random.Range(0.0f, 0.999f);
        if (jiggle > 0) {
            jiggle_change -= (((jiggle * JiggleAmbpli) + (jiggle_change * 0.2f)) * jiggle_rng);
        }else{
            jiggle_change -= (((jiggle * JiggleAmbpli) + (jiggle_change * 0.2f)) * jiggle_rng);
        }
        jiggle += jiggle_change;

        jiggle_rotation = (jiggle + GravityAlgorithm());
        jiggle = Mathf.Clamp(jiggle, (-JiggleRange + GravityAlgorithm()), (JiggleRange - GravityAlgorithm()));

        PartObj.transform.localRotation = Quaternion.Euler(0, 0, (jiggle_rotation * RotationalMultiplier));

        Vector2 PredictEndRescaleRef = PartChainHost.transform.root.transform.localScale; //relative scale in x, for later

        if (CanGrow){
            PartObj.transform.localScale = (this.StartScale * (HostGenitals.Inflation + 1));
            if (IsTits) PartObj.transform.localPosition = Objoffset - Vector3.right * ((PredictEndRescaleRef.x) * (HostGenitals.Inflation) * 0.1f); 

            if (HostGenitals.Growing != Growing){
                Growing = HostGenitals.Growing;
                if (Growing) DoMoanEvent(11,0);
            }
        }

        if (PartObj.name == "Pussy"){
            if (HostGenitals.DoingOrgasm) SpawnCumParticles();
        }

        if (PartObj.name == "DickTip"){ //if the tip of a d, then do a check for penetration
            //penetration max-length location math
            float PredictMaxLength = (((0.05f * HostGenitals.DickLength) * (Mathf.Clamp(HostGenitals.ErectionAmp + 1.4f, 0.35f, 1f))) + 0.155f);
            semiScale = Vector2.one;
            if (PartChainSemiHost != PartChainHost && PartChainSemiHost != null) semiScale = PartChainSemiHost.transform.localScale;
            float InverseCorrection = 1; //code shortening
            if (PredictEndRescaleRef.x < 0){
            InverseCorrection = -1;
            }
            float PredictEndX = (PartChainHost.transform.position.x + (((PredictMaxLength * PartChainHost.transform.right.x * semiScale.x * PredictEndRescaleRef.x) + ((-(PartChainHost.transform.up.x) * 0.14f)))));
            float PredictEndY = (PartChainHost.transform.position.y + (((PredictMaxLength * PartChainHost.transform.right.y * semiScale.x * PredictEndRescaleRef.x) + ((-(PartChainHost.transform.up.y) * 0.14f)))));

            if (HostGenitals.DoingOrgasm) SpawnCumParticles();

            DickDebugPosStuff = new Vector4((PartChainHost.transform.position.x), (PartChainHost.transform.position.y),PredictEndX,PredictEndY);
            PenetrationCheck(PartChainHost.transform.position, new Vector3(PredictEndX,PredictEndY));
        }
    }
    private void OnWillRenderObject()
    {
        if (Config.Debug_Mode){ //debug renders
            if ((PartObj.name == "DickTip")){
            ModAPI.Draw.Line(new Vector3(DickDebugPosStuff.x, DickDebugPosStuff.y), new Vector3(DickDebugPosStuff.z, DickDebugPosStuff.w));
            ModAPI.Draw.Circle(new Vector3(DickDebugPosStuff.z, DickDebugPosStuff.w), 0.1f);
            }
        }
    }

    public void PenetrationCheck(Vector2 Base, Vector2 End){ //used for penetration stuff
        bool hitValid = false;
        HitSurface = new RaycastHit2D();
        //HitSurface = Physics2D.Linecast(Base, End, 1 << LayerMask.NameToLayer("Objects")); //idk why but FUCK you ppg, this makes no sense 
        RaycastHit2D[] results = new RaycastHit2D[24];
        testCast = Physics2D.Linecast(Base, End, genitalHitFilter, results);
        if (testCast > 0){
            for (int r = 0; r < results.Length; r++){
                if (results[r].transform != null){
                    if (results[r].transform.GetComponent<LimbBehaviour>() == null){
                        HitSurface = results[r];
                        break;
                    }
                    if (results[r].transform.gameObject.name.Contains("LowerBody") || results[r].transform.gameObject.name.Contains("Fuckable")){
                        HitSurface = results[r];
                        hitValid = true;
                        break;
                    }
                }
            }
        }
        if (HitSurface){
            TotalDistance = Vector2.Distance(Base,End);
            HitDistance = HitSurface.distance;
            if (!hitValid){
                if (HostGenitals.DickIn) DickExitEvent();
                //HostGenitals.DickDeflectFactor = 0f;
                //HostGenitals.DickInsertDist = HostGenitals.DickLength;
                HostGenitals.DickDeflectFactor = (((TotalDistance - HitDistance) / TotalDistance) * 1.5f);
                HostGenitals.DickInsertDist = HostGenitals.DickLength;
                if (HostGenitals.DickIn) DickExitEvent();
                return;
            }

            if (!(HitSurface.transform.root.gameObject == PartChainHost.transform.root.gameObject)){
                ContainsFuckable = HitSurface.transform.gameObject.name.Contains("Fuckable");
                //makes them able to fuck anything in the lower body
                //bool ContainsSudoFuckable = true; //HitSurface.transform.gameObject.name.Contains("LowerBody");

                int sexVal = 0; //SUPER CURSED CODE, fuck you >:)
                if (HitSurface.transform.gameObject.name.Contains("_male")) sexVal = 1;
                if (HitSurface.transform.gameObject.name.Contains("_female")) sexVal = 2;
                if (HitSurface.transform.gameObject.name.Contains("_snake")) sexVal = 4;
                if (HitSurface.transform.gameObject.name.Contains("_synthmale")) sexVal = 5;
                if (HitSurface.transform.gameObject.name.Contains("_synthfemale")) sexVal = 6;
                if (HitSurface.transform.gameObject.name.Contains("_cosmicmale")) sexVal = 7;
                if (HitSurface.transform.gameObject.name.Contains("_cosmicfemale")) sexVal = 8;
                if (HitSurface.transform.gameObject.name.Contains("_goblinmale")) sexVal = 9;
                if (HitSurface.transform.gameObject.name.Contains("_goblinfemale")) sexVal = 10;
                if (HitSurface.transform.GetComponent<LimbBehaviour>() != null && sexVal == 0){
                    if (HitSurface.transform.GetComponent<LimbBehaviour>().SpeciesIdentity == "Android"){ //is default non-advc human an android
                        if (HitSurface.transform.root.gameObject.name.Contains("female")) sexVal = 6;
                        else sexVal = 5;
                    }else{ //if is default non-advc human
                        if (HitSurface.transform.root.gameObject.name.Contains("female")) sexVal = 2;
                        else if (HitSurface.transform.root.gameObject.name.Contains("male")) sexVal = 1;
                    }
                }
                //defaults sexval to 0 if no checks match

                if (!(HostGenitals.DickIn)) DickEnterEvent(HitSurface.transform.gameObject,sexVal);

                HostGenitals.DickDeflectFactor = 0f;
                HostGenitals.DickInsertDist = (int) Mathf.Round((HitDistance / TotalDistance) * HostGenitals.DickLength);
            }else{
                if (HostGenitals.DickIn) DickExitEvent();
                HostGenitals.DickDeflectFactor = 0f;
                HostGenitals.DickInsertDist = HostGenitals.DickLength;
            }
        }else{
            if (HostGenitals.DickIn) DickExitEvent();
            HostGenitals.DickDeflectFactor = 0f;
            HostGenitals.DickInsertDist = HostGenitals.DickLength;
        }
    }

    //these are used for when a dick enters something or exists something accordingly.
    public void DickEnterEvent(GameObject Receiver, int sexVal = 0){
        HostGenitals.DickIn = true;
        ModAPI.Notify("Dick Entered Hole");
        if(HostGenitals.DoDickAudio) PlayDickAudio(HostGenitals.DickSfxNormal);
        if (!HostGenitals.IsDildo){
            if (Config.Furry_Mode){
                HostGenitals.AddArousal(10);
            }else{
                HostGenitals.AddArousal(6);
            }
            DoMoanEvent(HostGenitals.CustomSexInfo,4);
        }
        if (Config.Furry_Mode){
            Receiver.SendMessage("AddArousal", 10); //used for communication with maybe other instances of the same code via other mods!?
        }else{
            Receiver.SendMessage("AddArousal", 6); //used for communication with maybe other instances of the same code via other mods!?
        }
        DoMoanEvent(sexVal,4);
        //Receiver.
        //yo, addon support!??!?!?!?!??!
    }
    public void DickExitEvent(){
        HostGenitals.DickIn = false;
        ModAPI.Notify("Dick Exited Hole");
        if(HostGenitals.DoDickAudio) PlayDickAudio(HostGenitals.DickSfxWeak);
    }

    //the 2 methods below this comment, define the moan audio and sexes

    public void DoMoanEvent(int moanType = 0,int chance = 0){ //plays moan audio and then deletes its audio source, like a play at location one shot but for moan, with a chance
        if (HostGenitals.GetComponent<LimbBehaviour>() != null) if (HostGenitals.GetComponent<LimbBehaviour>().Health < 0.0001f) return;
        if (!Config.Moan_Happens && moanType != 3 && moanType != 11) return;
        
        if (UnityEngine.Random.Range(0,chance) != 0) return;

        AudioClip[] audioToPlay;
        switch(moanType) 
            {
            case 0:
                audioToPlay = ADVCassets.MoanPerson;
                break;
            case 1:
                audioToPlay = ADVCassets.MoanMale;
                break;
            case 2:
                audioToPlay = ADVCassets.MoanFemale;
                break;
            case 3:
                audioToPlay = ADVCassets.CumAudio;
                break;
            case 4:
                audioToPlay = ADVCassets.MoanSnake;
                break;
            case 5:
                audioToPlay = ADVCassets.MoanSynthMale;
                break;
            case 6:
                audioToPlay = ADVCassets.MoanSynthFemale;
                break;
            case 7:
                audioToPlay = ADVCassets.MoanCosmicMale;
                break;
            case 8:
                audioToPlay = ADVCassets.MoanCosmicFemale;
                break;
            case 9:
                audioToPlay = ADVCassets.MoanGoblinMale;
                break;
            case 10:
                audioToPlay = ADVCassets.MoanGoblinFemale;
                break;
            case 11:
                audioToPlay = ADVCassets.InflationAudio;
                break;
            default:
                audioToPlay = ADVCassets.MoanPerson;
                break;
        }
        int MoanClipNum = UnityEngine.Random.Range(0,(audioToPlay.Length - 1));
        GameObject tempAudioObj = new GameObject("MoanAudioObj_" + UnityEngine.Random.Range(10000,99999));
        tempAudioObj.transform.position = transform.position;
        AudioSource tempAudioSoure = tempAudioObj.AddComponent<AudioSource>();
        tempAudioSoure.maxDistance = 0.5f;
        tempAudioSoure.playOnAwake = false;
        tempAudioSoure.clip = audioToPlay[MoanClipNum];
        tempAudioSoure.loop = false;
        tempAudioSoure.Play();
        Destroy(tempAudioObj,audioToPlay[MoanClipNum].length + 0.05f);
    }

    public void DoMoanEvent(string moanType = "person",int chance = 0){ //shortcut method for string input if lazy
        int valOut = 0;
        switch(moanType) 
            {
            case "person":
                valOut = 0;
                break;
            case "male":
                valOut = 1;
                break;
            case "female":
                valOut = 2;
                break;
            case "cum":
                valOut = 3;
                break;
            case "snake":
                valOut = 4;
                break;
            case "synthmale":
                valOut = 5;
                break;
            case "synthfemale":
                valOut = 6;
                break;
            case "cosmicmale":
                valOut = 7;
                break;
            case "cosmicfemale":
                valOut = 8;
                break;
            case "goblinmale":
                valOut = 9;
                break;
            case "goblinfemale":
                valOut = 10;
                break;
            case "inflation":
                valOut = 11;
                break;
            default:
                break;
        }
        DoMoanEvent(valOut,chance);
    }

    private void PlayDickAudio(AudioClip[] DickSfxUsed){
        int DickSfxClipNum = UnityEngine.Random.Range(0, (DickSfxUsed.Length - 1));
        HostGenitals.DickAudio.maxDistance = 0.5f;
        HostGenitals.DickAudio.playOnAwake = false;
        HostGenitals.DickAudio.clip = DickSfxUsed[DickSfxClipNum];
        HostGenitals.DickAudio.loop = false;
        HostGenitals.DickAudio.Play();
    }

    private float GravityAlgorithm(){
        float RotationTempErectionAmp = HostGenitals.ErectionAmp - Mathf.Clamp(HostGenitals.DickDeflectFactor, 0.0f, 1.5f);
        return (PartObj.transform.right.x * ((JiggleGravity * -1) * -RotationTempErectionAmp));
    }

    public void DefineParticles(Texture2D ParticleTexture){
        if (Config.Furry_Mode) HostGenitals.CumAmt = 60;
        else HostGenitals.CumAmt = 50;

        var ParticleMain = GPObjParticlesElement.main;
        var VelocityLimitOT = GPObjParticlesElement.limitVelocityOverLifetime;
        var ParticleShape = GPObjParticlesElement.shape;
        var ParticleEmission = GPObjParticlesElement.emission;
        var ParticleROLT = GPObjParticlesElement.rotationOverLifetime;
        var ParticleSize = GPObjParticlesElement.sizeOverLifetime;
        var ParticleCol = GPObjParticlesElement.collision;
        var ParticleTrails = GPObjParticlesElement.trails;

        if (Config.Furry_Mode){ //THE NEW INSANE CUM CODE
            ParticleMain.duration = 0.3f;
            ParticleMain.loop = false;
            ParticleMain.playOnAwake = false;
            ParticleMain.flipRotation = 50f;
            ParticleMain.startLifetime = 50f;
            ParticleMain.startSize = (0.22f);
            ParticleMain.simulationSpace = ParticleSystemSimulationSpace.World;
            ParticleMain.scalingMode = ParticleSystemScalingMode.Shape;
            ParticleMain.gravityModifier = 0.6f;
            ParticleMain.startSpeedMultiplier = 0.3f;

            ParticleMain.startColor = HostGenitals.CumColour; 
            
            VelocityLimitOT.dragMultiplier = 0.1f;
            VelocityLimitOT.multiplyDragByParticleVelocity = true;

            ParticleShape.enabled = true;
            ParticleShape.spriteRenderer = PartObj.GetComponent<SpriteRenderer>();

            ParticleEmission.enabled = true;
            ParticleEmission.rateOverTime = HostGenitals.CumAmt;
            ParticleEmission.rateOverDistance = 7;

            ParticleROLT.enabled = true;
            ParticleROLT.z = UnityEngine.Random.Range(-3f, 3f);

            ParticleSize.enabled = false;
            ParticleSize.size = new ParticleSystem.MinMaxCurve(0.22f, 0.22f);
            
            ParticleCol.mode = ParticleSystemCollisionMode.Collision2D;
            ParticleCol.type = ParticleSystemCollisionType.World;
            ParticleCol.enabled = true;
            ParticleCol.bounce = 0.55f;

            ParticleTrails.enabled = true;
            ParticleTrails.ratio = 1f;
            ParticleTrails.inheritParticleColor = true;
            ParticleTrails.lifetime = 0.01f;
            ParticleTrails.widthOverTrail = 0.21f;
        }else{ //THE ORIGINAL CUM CODE
            ParticleMain.duration = 0.11f;
            ParticleMain.loop = false;
            ParticleMain.playOnAwake = false;
            ParticleMain.flipRotation = 50f;
            ParticleMain.startLifetime = 10f;
            ParticleMain.startSize = (0.2f);
            ParticleMain.simulationSpace = ParticleSystemSimulationSpace.World;
            ParticleMain.scalingMode = ParticleSystemScalingMode.Shape;
            ParticleMain.gravityModifier = 0.75f;
            ParticleMain.startSpeedMultiplier = 0.2f;

            ParticleMain.startColor = HostGenitals.CumColour; 
            
            VelocityLimitOT.dragMultiplier = 0.1f;
            VelocityLimitOT.multiplyDragByParticleVelocity = true;

            ParticleShape.enabled = true;
            ParticleShape.spriteRenderer = PartObj.GetComponent<SpriteRenderer>();

            ParticleEmission.enabled = true;
            ParticleEmission.rateOverTime = HostGenitals.CumAmt;
            ParticleEmission.rateOverDistance = 5;

            ParticleROLT.enabled = true;
            ParticleROLT.z = UnityEngine.Random.Range(-3f, 3f);

            ParticleSize.enabled = false;
            ParticleSize.size = new ParticleSystem.MinMaxCurve(0.22f, 0.22f);
            
            ParticleCol.mode = ParticleSystemCollisionMode.Collision2D;
            ParticleCol.type = ParticleSystemCollisionType.World;
            ParticleCol.enabled = true;
            ParticleCol.bounce = 0.45f;

            ParticleTrails.enabled = true;
            ParticleTrails.ratio = 1f;
            ParticleTrails.inheritParticleColor = true;
            ParticleTrails.lifetime = 0.02f;
            ParticleTrails.widthOverTrail = 0.2f;
        }

        var ParticleRenderer = GPObjParticlesElement.GetComponent<ParticleSystemRenderer>();
        ParticleRenderer.trailMaterial = ModAPI.FindMaterial("Sprites-Default");
        ParticleRenderer.material = UnityEngine.Object.Instantiate<Material>(ModAPI.FindMaterial("Sprites-Default"));
        ParticleRenderer.material.mainTexture = ParticleTexture;
        ParticleRenderer.renderMode = ParticleSystemRenderMode.VerticalBillboard;
        GPObjParticlesElement.Stop();
    }

    public void SpawnCumParticles(){
        if (HostGenitals.LastCumTickTime < UnityEngine.Time.fixedTime){
            HostGenitals.LastCumTickTime = UnityEngine.Time.fixedTime + 0.23f;
            GPObjParticlesElement.Play();

            if (Config.Cum_Audio) DoMoanEvent(3,2);

            if (HostGenitals.IsCosmic){
                HostGenitals.gameObject.GetComponent<CosmicLimbImmortality>().CreateCosmicParticle();
            }
        }
    }
}
