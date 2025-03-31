using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CosmicRenderer : MonoBehaviour {
    public SpriteRenderer spriteRenderer; //auto set, the main sprite renderer
    private bool validRender = false;
    public float scale = 0.43f; //perlin noise scale
    public float noiseIntensity = 1.5f; //intensity of noise effect

    public Color[] pixels; //temp
    public Texture2D tex;
    public Texture2D originalTex;
    public TrailRenderer TrailRend;
    public Rigidbody2D rigi;
    public Color[] pixelsOrigin; //temp
    public Vector2 worldPos;
    public Vector2 pixelOffset;
    public Vector2 rotatedOffset;
    public Vector2 worldPixelPos;
    private int index;
    //private Color pixel;
    public float cos;
    public float sin;
    private int width;
    private int height;
    public float noiseValue;
    // private float noiseValueAltBlue;
    // private float noiseValueAltGreen;
    public float angle;

    private void Awake(){
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null){
            this.validRender = true;
            this.originalTex = Instantiate(this.gameObject.GetComponent<SpriteRenderer>().sprite.texture);
            this.pixelsOrigin = this.originalTex.GetPixels();
        }
        TrailRend = gameObject.GetOrAddComponent<TrailRenderer>();
        SetTrailSizes(1);
        TrailRend.startColor = new Color(1, 1f, 1,0.05f);
        TrailRend.endColor = new Color(0.8f, 0f, 0.8f,0f);
        TrailRend.material = ModAPI.FindMaterial("VeryBright");
        TrailRend.sortingOrder = 0;
        TrailRend.shadowBias = 0.3f;
        Gradient TrailGradient = new Gradient();
        TrailGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f, 0.01f), 0f),
                new GradientColorKey(new Color(0.8f, 0.3f, 1f, 0.05f), 0.5f),
                new GradientColorKey(new Color(0.8f, 0f, 0.8f, 0f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.01f, 0.0f),
                new GradientAlphaKey(0.05f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        TrailRend.colorGradient = TrailGradient;
        TrailRend.enabled = false;
        rigi = this.GetComponent<Rigidbody2D>();
    }
    // private void Start(){
    //     spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    //     if (spriteRenderer != null){
    //         validRender = true;
    //         originalTex = Instantiate(this.gameObject.GetComponent<SpriteRenderer>().sprite.texture);
    //         pixelsOrigin = originalTex.GetPixels();
    //     }
    // }

    private void SetTrailSizes(float trailScaler){
        TrailRend.startWidth = 0.3f * trailScaler;
        TrailRend.endWidth = 0.01f * trailScaler;
        TrailRend.time = 0.225f * (trailScaler * 1.25f);
    }

    private void FixedUpdate(){
        ApplyPerlinNoise();
        if (rigi == null) return;
        if (TrailRend.enabled){
            if (!((Mathf.Abs(rigi.velocity.x) > 3) || (Mathf.Abs(rigi.velocity.y) > 3)))
                TrailRend.enabled = false;
        }
        if ((Mathf.Abs(rigi.velocity.x) > 3) || (Mathf.Abs(rigi.velocity.y) > 3)){
            if (!TrailRend.enabled) TrailRend.enabled = true;
            SetTrailSizes(((Mathf.Abs(rigi.velocity.x)) + (Mathf.Abs(rigi.velocity.y))) * 0.01f);
        }
        rigi.velocity *= 0.95f;
    }

    public void ApplyPerlinNoise(){
        if (!validRender) return;
        if (spriteRenderer.sprite == null){
            Debug.LogError("SpriteRenderer sprite for cosmic renderer thingy is invalid");
            return;
        }
        if (originalTex == null){
            originalTex = spriteRenderer.sprite.texture;
            pixelsOrigin = originalTex.GetPixels();
        }
        if (originalTex == null) return; //if still null, fuck you

        if (tex == null) tex = Instantiate(spriteRenderer.sprite.texture); //so it doesn't contantly flood ram with new textures every tick O_O''

        pixels = tex.GetPixels();
        width = tex.width;
        height = tex.height;

        worldPos = (Vector2)transform.position;
        this.angle = this.transform.eulerAngles.z * Mathf.Deg2Rad; //convert rotation to radians...

        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){

                index = y * width + x;
                //pixel = pixels[index];

                if (pixelsOrigin[index].a == 1){
                    //convert local pixel position to world space
                    pixelOffset = new Vector2((x - (width * 0.5f)), (y - (height * 0.5f)));
                    //rotatedOffset = RotatePoint(pixelOffset, angle);
                    //worldPixelPos = (Vector2)worldPos + rotatedOffset;

                    //generate Perlin noise based on world-relative position, with alternates to mix into it
                    noiseValue = GetNoiseValOfRotation(scale,0);
                    // noiseValueAltBlue = GetNoiseValOfRotation(scale,45);
                    // noiseValueAltGreen = GetNoiseValOfRotation(scale,90);

                    if (pixelsOrigin[index].r < 0.65098f && pixelsOrigin[index].g < 0.65098f && pixelsOrigin[index].b < 0.65098f){
                        pixels[index] = Color.white;
                    }else{
                        pixels[index] = new Color(
                            (noiseValue * noiseIntensity),
                            Mathf.Clamp(
                                ((noiseValue * noiseIntensity) * 0.85f) + Mathf.Clamp(Mathf.Floor(GetNoiseValOfRotation(scale,90) * 1.1f) * 0.5f,0,1)
                                ,0,1),
                            Mathf.Clamp(
                                (noiseValue * noiseIntensity) + Mathf.Clamp(Mathf.Floor(GetNoiseValOfRotation(scale,45) * 1.1f) * 0.5f,0,1)
                                ,0,1),
                            1);
                    }
                }
            }
        }

        // Apply the modified pixels to the texture
        tex.SetPixels(pixels);
        tex.Apply();
        spriteRenderer.sprite = Sprite.Create(tex, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
    }

    float GetNoiseValOfRotation(float scal, float rot){ //generate Perlin noise based on world-relative position, with rotation offset
        rotatedOffset = RotatePoint(pixelOffset, this.angle + rot);
        //worldPixelPos = worldPos + rotatedOffset;
        return Mathf.PerlinNoise((worldPos + rotatedOffset).x * scal, (worldPos + rotatedOffset).y * scal);
    }

    Vector2 RotatePoint(Vector2 point, float angleIn){
        cos = Mathf.Cos(angleIn);
        sin = Mathf.Sin(angleIn);
        return new Vector2(point.x * cos - point.y * sin, point.x * sin + point.y * cos);
    }
}

public class CosmicBody : MonoBehaviour{ //heavy heavy heavy inspiration from another mod, as i got lazy. handles the cosmic state of the whole body of a person
    public bool IsInited = false;
    private PersonBehaviour person;

    public void Awake(){
        if (IsInited == true)
            return;

        IsInited = true;
        person = this.gameObject.GetComponent<PersonBehaviour>();
        foreach (var body in person.Limbs){
            body.gameObject.GetOrAddComponent<CosmicLimbImmortality>();
            body.PhysicalBehaviour.Disintegratable = false;
        }
    }

    private void Update(){
        person.BrainDamaged = false;
        person.Braindead = false;
        person.Consciousness = 1f;
        person.ShockLevel = person.ShockLevel * 0.9f; //faster static shock drain
        person.PainLevel = person.PainLevel * 0.95f; //faster static pain drain

        foreach (var body in person.Limbs)
        {
            body.ImmuneToDamage = true;
            body.CirculationBehaviour.ImmuneToDamage = true;
            body.CirculationBehaviour.AddLiquid(body.GetOriginalBloodType(), (body.CirculationBehaviour.Limits.y - body.CirculationBehaviour.GetAmountOfBlood()));
            body.CirculationBehaviour.BloodFlow = 1f;

            body.Numbness = body.Numbness * 0.95f;
            body.HealBone();
            body.LungsPunctured = false;
            body.CirculationBehaviour.HealBleeding();
            body.CirculationBehaviour.IsPump = body.CirculationBehaviour.WasInitiallyPumping;
            body.Health = body.InitialHealth;
            body.InternalTemperature = body.BodyTemperature;
            
            body.CirculationBehaviour.BloodFlow = 1f;
            body.CirculationBehaviour.BleedingRate = 0;
            body.CirculationBehaviour.InternalBleedingIntensity = 0;
            body.HealBone();
            body.LungsPunctured = false;
            if (body.TryGetComponent(out HingeJoint2D Joint)){
                Joint.breakForce = float.PositiveInfinity;
                Joint.breakTorque = float.PositiveInfinity;
            }
            body.BreakingThreshold = Mathf.Infinity;
            body.PhysicalBehaviour.Extinguish();
            body.gameObject.GetComponent<CosmicLimbImmortality>().DamageVisualHeal(UnityEngine.Time.deltaTime);
        }
    }
}

public class CosmicLimbImmortality : MonoBehaviour{ //handles the individual immortality of cosmic limbs themselves
    public LimbBehaviour limb;
    private float CosmicHealSyncTimer;
    public SkinMaterialHandler SkinMaterialHandler;
    private bool healingState = false;

    private void Start(){
        limb = this.gameObject.GetComponent<LimbBehaviour>();
        SkinMaterialHandler = limb.SkinMaterialHandler;
    }

    private float GetMassStrengthRatio(){
        return this.limb.PhysicalBehaviour.rigidbody.mass / this.limb.PhysicalBehaviour.TrueInitialMass;
    }

    public void DamageVisualHeal(float timeOffset){ //an edit from my terminator mod, to make this be more seamless
        if (UnityEngine.Time.fixedTime > CosmicHealSyncTimer){
            CosmicHealSyncTimer = (UnityEngine.Time.fixedTime + 0.25f);
            healingState = false;
            for (int i = 0; i < 128; ++i){
                if (SkinMaterialHandler.damagePoints[i].z > 0){
                    healingState = true;
                    SkinMaterialHandler.damagePoints[i].z -= timeOffset * (33); //visual heal rate
                }
            }
            SkinMaterialHandler.Sync();
            this.limb.PhysicalBehaviour.BurnProgress = 0;
            if (healingState){
                DoCosmicRegenAudio(6);
                CreateCosmicParticle();
            }
        }
    }

    public void DoCosmicRegenAudio(int chance = 0){
        if (UnityEngine.Random.Range(0,chance) != 0) return;

        int CosmicClipNum = UnityEngine.Random.Range(0,(ADVCassets.CosmicRegenAudio.Length - 1));
        GameObject tempAudioObj = new GameObject("CosmicAudioObj_" + UnityEngine.Random.Range(10000,99999));
        tempAudioObj.transform.position = transform.position;
        AudioSource tempAudioSoure = tempAudioObj.AddComponent<AudioSource>();
        tempAudioSoure.maxDistance = 0.15f;
        tempAudioSoure.volume = 0.2f;
        tempAudioSoure.playOnAwake = false;
        tempAudioSoure.clip = ADVCassets.CosmicRegenAudio[CosmicClipNum];
        tempAudioSoure.loop = false;
        tempAudioSoure.Play();
        Destroy(tempAudioObj,ADVCassets.CosmicRegenAudio[CosmicClipNum].length + 0.05f);
    }

    public void CreateCosmicParticle(){
        GameObject CosmicPartiHolder = new GameObject("Holder");
        Destroy(CosmicPartiHolder,UnityEngine.Random.Range(2.5f, 6f));

        CosmicPartiHolder.transform.position = this.transform.position;
        CosmicPartiHolder.transform.localScale = new Vector2(0.38f, 0.38f);

        ParticleSystem Particles = CosmicPartiHolder.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule particleMain = Particles.main;
        particleMain.duration = 0.02f;
        particleMain.loop = false;
        particleMain.playOnAwake = true;
        particleMain.flipRotation = 0f;
        particleMain.startLifetime = 2f;
        particleMain.startSize = (1f);
        particleMain.simulationSpace = ParticleSystemSimulationSpace.World;
        particleMain.scalingMode = ParticleSystemScalingMode.Shape;
        particleMain.gravityModifier = 0f;
        particleMain.startSpeedMultiplier = 1f;

        var limitVelocityOverLifetime = Particles.limitVelocityOverLifetime;
        limitVelocityOverLifetime.dragMultiplier = 0.1f;
        limitVelocityOverLifetime.multiplyDragByParticleVelocity = true;

        var shape = Particles.shape;
        shape.enabled = true;

        var particleEmis = Particles.emission;
        particleEmis.enabled = true;
        particleEmis.rateOverTime = 500;
        particleEmis.rateOverDistance = 0;

        var size = Particles.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(0.005f, 0.03f);

        var col = Particles.collision;
        col.enabled = true;
        col.mode = ParticleSystemCollisionMode.Collision2D;
        col.type = ParticleSystemCollisionType.World;
        col.bounce = 0;

        var col2 = Particles.colorOverLifetime;
        col2.enabled = true;
        col2.color = new ParticleSystem.MinMaxGradient(new Color(1, 1f, 1,0.05f), new Color(0.8f, 0f, 0.8f,0f));

        var rend = Particles.GetComponent<ParticleSystemRenderer>();
        rend.material = UnityEngine.Object.Instantiate<Material>(ModAPI.FindMaterial("VeryBright"));
        rend.material.mainTexture = ADVCassets.ParticleTextures[4];
        rend.renderMode = ParticleSystemRenderMode.Billboard;
    }

    public void Shot(Shot shot){
        shot.damage /= this.GetMassStrengthRatio();
        shot.damage *= UserPreferenceManager.Current.FragilityMultiplier;
        if (this.limb.IsAndroid){
            if (shot.damage < 40f)
            {
                return;
            }
            shot.damage *= 0.2f;
        }

        bool flag = this.limb.IsWorldPointInVitalPart(shot.point, 0.114285715f) && UnityEngine.Random.value > 0.05f;
        float num = (flag ? 7f : 0.1f) * shot.damage;
        if (!UserPreferenceManager.Current.GorelessMode && UserPreferenceManager.Current.ChunkyShotParticles && this.limb.Person.PoolableImpactEffect && (double)this.limb.PhysicalBehaviour.BurnProgress < 0.6 && this.limb.SkinMaterialHandler.AcidProgress < 0.7f && num > this.limb.Person.ImpactEffectShotDamageThreshold && UnityEngine.Random.value > 0.8f)
        {
            GameObject gameObject = PoolGenerator.Instance.RequestPrefab(this.limb.Person.PoolableImpactEffect, shot.point);
            if (gameObject)
            {
                gameObject.transform.right = shot.normal;
            }
        }
        if (this.limb.ConnectedLimbs != null)
        {
            for (int i = 0; i < this.limb.ConnectedLimbs.Count; i++)
            {
                LimbBehaviour limbBehaviour = this.limb.ConnectedLimbs[i];
                if (limbBehaviour)
                {
                    limbBehaviour.Numbness += 0.5f;
                }
            }
        }
        if (this.limb.NodeBehaviour.IsConnectedToRoot)
        {
            this.limb.Person.AdrenalineLevel += UnityEngine.Random.value;
            this.limb.Person.Consciousness -= UnityEngine.Random.Range(0.02f, 0.1f);
            if (!this.limb.IsAndroid && !this.limb.IsZombie)
            {
                this.limb.Person.ShockLevel += num * UnityEngine.Random.value * 0.0025f;
                this.limb.Person.Wince(300f);
                this.limb.Numbness = 1f;
                if (UnityEngine.Random.value * this.limb.Vitality > 0.5f && UnityEngine.Random.value > 0.6f && !this.limb.IsParalysed)
                {
                    this.limb.Person.AddPain(UnityEngine.Random.value * 2f);
                }
                if (num > 2f * UnityEngine.Random.value)
                {
                    if (shot.normal.x > 0f == this.limb.Person.transform.localScale.x > 0f)
                    {
                        this.limb.Person.DesiredWalkingDirection -= UnityEngine.Random.value * 3f;
                    }
                    else
                    {
                        this.limb.Person.DesiredWalkingDirection += UnityEngine.Random.value * 3f;
                    }
                }
                this.limb.Person.SendMessage("Shot", shot);
            }
            if (this.limb.HasBrain && !this.limb.IsAndroid)
            {
                if (this.limb.IsZombie && UnityEngine.Random.value > 0.2f)
                {
                    return;
                }
                float num2 = flag ? 0.1f : 0.8f;

            }
        }
        if (shot.CanCrush && UserPreferenceManager.Current.LimbCrushing && shot.damage > 149f && Mathf.Clamp((shot.damage - 149f) * 0.005f, 0.3f, 0.8f) > UnityEngine.Random.value)
        {
            if (UserPreferenceManager.Current.StopAnimationOnDamage)
            {
                this.limb.Person.OverridePoseIndex = -1;
            }
        
        }

        float b = shot.damage * 0.1f;
        this.limb.SkinMaterialHandler.AddDamagePoint(DamageType.Bullet, shot.point, Mathf.Max(50f, b));
    }

    public void ExitShot(Shot shot)
    {

        shot.damage /= this.GetMassStrengthRatio();
        shot.damage *= UserPreferenceManager.Current.FragilityMultiplier;
        this.limb.SkinMaterialHandler.AddDamagePoint(DamageType.Bullet, shot.point, Mathf.Max(60f, shot.damage * 0.4f));
        if (!UserPreferenceManager.Current.GorelessMode && UserPreferenceManager.Current.ChunkyShotParticles && this.limb.Person.PoolableImpactEffect && (double)this.limb.PhysicalBehaviour.BurnProgress < 0.6 && this.limb.SkinMaterialHandler.AcidProgress < 0.7f && shot.damage > this.limb.Person.ImpactEffectShotDamageThreshold && UnityEngine.Random.value > 0.6f)
        {
            GameObject gameObject = PoolGenerator.Instance.RequestPrefab(this.limb.Person.PoolableImpactEffect, shot.point);
            if (gameObject)
            {
                gameObject.transform.right = shot.normal;
            }
        }
        if (this.limb.HasLungs && !this.limb.IsAndroid && UnityEngine.Random.value > 0.9f)
        {
            this.limb.LungsPunctured = true;
        }
        if (UserPreferenceManager.Current.StopAnimationOnDamage && this.limb.NodeBehaviour.IsConnectedToRoot)
        {
            this.limb.Person.OverridePoseIndex = -1;
        }

        Color computedColor = this.limb.CirculationBehaviour.GetComputedColor(this.limb.GetOriginalBloodType().Color);
        for (int i = 0; i < UnityEngine.Random.Range(1, 4); i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(shot.point, shot.normal + UnityEngine.Random.insideUnitCircle * 0.4f, 3f);
            if (hit && hit.transform)
            {
                hit.transform.gameObject.SendMessage("Decal", new DecalInstruction(this.limb.BloodDecal, hit.point, computedColor, 1f), SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    public void Stabbed(Stabbing stab)
    {

        if (!stab.stabber.StabCausesWound)
        {
            return;
        }
        if (this.limb.CirculationBehaviour.GetAmountOfBlood() > 0.2f)
        {
            stab.stabber.SendMessage("Decal", new DecalInstruction(this.limb.BloodDecal, stab.point, this.limb.CirculationBehaviour.GetComputedColor(this.limb.GetOriginalBloodType().Color), 1f), SendMessageOptions.DontRequireReceiver);
        }
        if (this.limb.HasLungs && !this.limb.IsAndroid && UnityEngine.Random.value > 0.9f)
        {
            this.limb.LungsPunctured = true;
        }
        bool flag = this.limb.IsWorldPointInVitalPart(stab.point, 0.114285715f) && UnityEngine.Random.value > 0.05f;
        this.limb.Damage(this.limb.Health * 0.5f * (this.limb.IsZombie ? 0.1f : 1f) * (float)(flag ? 2 : 1) * UserPreferenceManager.Current.FragilityMultiplier);

        this.limb.Wince(165f);


        if (this.limb.HasBrain && flag)
        {
            if (this.limb.IsZombie && UnityEngine.Random.value > 0.5f)
            {
                return;
            }
            this.limb.Person.AddPain(90f);
            this.limb.CirculationBehaviour.InternalBleedingIntensity += 5f * UnityEngine.Random.value;

        }
    }

}
