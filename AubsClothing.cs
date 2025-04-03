using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Linq;

public class AubClothingObj : MonoBehaviour //add to a clothing object
{
    //handles the clothing object itself

    public string clothingVisibleName = "undefined";
    public int clothingIndex = 0;
    public int clothingSlot = 0;
    public bool setup = false;
    public Texture2D texBody;
    public Sprite texIcon;
    public Sprite texObj;

    private void Awake(){

    }
    private void Start()
    {

    }
    
    public void OnCollisionEnter2D(Collision2D collision){
        //Debug.Log("Trying clothing ("+clothingVisibleName+") on ("+collision.gameObject.name+")");
        if (collision.gameObject.GetComponent<LimbBehaviour>() == null) return;
        if (!setup){
            Debug.Log("[X] Clothing not fully spawned in yet?? cannot apply");
            return;
        }
        Debug.Log("Trying to apply clothing ("+clothingVisibleName+")");
        AubClothing clothingBehav = collision.transform.root.gameObject.GetComponent<AubClothing>();

        if (clothingBehav == null){
            clothingBehav = collision.transform.root.gameObject.AddComponent<AubClothing>();
            if (collision.gameObject.GetComponent<CosmicRenderer>() != null) clothingBehav.CosmicMode = true;
            LimbBehaviour[] EveryLimb = collision.gameObject.GetComponent<LimbBehaviour>().Person.Limbs;

            for(int i = 0; i < EveryLimb.Length; i++){
                if (EveryLimb[i].GetComponent<SpriteRenderer>() == null) continue;
                if (EveryLimb[i].GetComponent<SpriteRenderer>().sprite == null) continue;
                clothingBehav.spriteRenderer = EveryLimb[i].GetComponent<SpriteRenderer>();
                break;
            }
        }
        if (clothingBehav.CosmicMode) return; //if cosmic, skips the rest, no clothing for you lol
        if (clothingBehav.ApplyClothing(texBody,clothingIndex,clothingSlot)) Destroy(this.gameObject);
    }
}

public class AubClothing : MonoBehaviour //add to a human's main object- not limb, gives them clothing
{
    //handles the clothing object itself

    public bool Digitigrade = false; //applied if a advc character this is on- has standard alternate digitigrade legs instead of human legs
    public int[] clothingActiveIDs;
    public int[] clothingActiveSlots;
    public Texture2D[] clothingActiveTex;
    public Texture2D originalTex;
    public SpriteRenderer spriteRenderer; //assign this
    public bool Setup = false;
    public bool arraysReady = false;

    public bool CosmicMode = false; //for if this is on a cosmic entity from the adv character mod.

    public Color[] pixels; //temp
    public Color[] pixelsClothing; //temp
    public Texture2D texTemp;
    public Color[] pixelsOrigin; //temp
    private int width;
    private int height;
    private int index;

    public bool ApplyClothing(Texture2D clothingTex,int clothingID,int clothingSlot){
        if (!arraysReady){
            arraysReady = true;
            clothingActiveIDs = new int[]{};
            clothingActiveSlots = new int[]{};
            clothingActiveTex = new Texture2D[]{};
        }
        Setup = true;
        //Debug.Log("Attempting to apply clothing. =====");
        for(int i = 0; i < clothingActiveSlots.Length; i++){
            if (clothingActiveIDs[i] == clothingID || clothingActiveSlots[i] == clothingSlot){
                Setup = false;
                break;
            }
        }
        if (!Setup){
            Debug.Log("[X] Clothing slot "+clothingSlot+" already in use, cannot apply clothing");
            return false;
        }

        clothingActiveIDs = clothingActiveIDs.Concat(new []{clothingID}).ToArray();
        clothingActiveSlots = clothingActiveSlots.Concat(new [] {clothingSlot}).ToArray();
        clothingActiveTex = clothingActiveTex.Concat(new [] {clothingTex}).ToArray();
        Debug.Log("[^] Applied clothing! slot("+clothingSlot+") id("+clothingID+"), proceeding to render...");
        RegenerateVisuals();
        return true;
    }
    public void RegenerateVisuals(){
        if (CosmicMode) return; //currently no support for cosmic beings sadly. may add later

        if (originalTex == null){
            originalTex = Instantiate(spriteRenderer.sprite.texture);
            //pixelsOrigin = originalTex.GetPixels();
        }
        if (originalTex == null) return; //if still null, fuck you

        texTemp = Instantiate(originalTex);

        pixels = texTemp.GetPixels();
        width = texTemp.width;
        height = texTemp.height;

        for(int clothingTexIn = 0; clothingTexIn < clothingActiveTex.Length; clothingTexIn++){
            pixelsClothing = clothingActiveTex[clothingTexIn].GetPixels();

            for (int y = 0; y < height; y++){
                for (int x = 0; x < width; x++){
                    index = y * width + x;
                    if (pixelsClothing[index].a <= 0.2f) continue;
                    pixels[index] = lerpColor(pixels[index],pixelsClothing[index],pixelsClothing[index].a);
                }
            }
        }
        texTemp.SetPixels(pixels);
        texTemp.Apply();

        LimbBehaviour[] EveryLimb = GetComponent<PersonBehaviour>().Limbs;
        foreach (LimbBehaviour LimbIn in EveryLimb){
            if (LimbIn.GetComponent<SpriteRenderer>() == null) continue;
            if (LimbIn.GetComponent<SpriteRenderer>().sprite == null) continue;
            LimbIn.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texTemp, LimbIn.GetComponent<SpriteRenderer>().sprite.rect, new Vector2(0.5f, 0.5f), LimbIn.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }

    private Color lerpColor(Color colorIn, Color colorOut, float TimeIn){
        return(new Color(
            Mathf.Lerp(colorIn.r,colorOut.r,TimeIn),
            Mathf.Lerp(colorIn.g,colorOut.g,TimeIn),
            Mathf.Lerp(colorIn.b,colorOut.b,TimeIn),
            Mathf.Lerp(colorIn.a,colorOut.a,TimeIn)
        ));
    }
}