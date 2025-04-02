using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class LimbInspectOverhaul : MonoBehaviour
{
    private float Timer = Time.unscaledDeltaTime;

    public void Update(){
        if ((float) Timer > LimbStatusViewBehaviour.Main.UpdateInterval){
            InspectRefresh(GetTx());
        }
    }

    public void InspectRefresh(string AdditionTx){
        if (!(LimbStatusViewBehaviour.Main.Limbs.Any<LimbBehaviour>((Func<LimbBehaviour, bool>) (c => !(bool) (UnityEngine.Object) c)))){
            LimbStatusViewBehaviour.Main.LimbDamageSourceStats.text = LimbStatusViewBehaviour.Main.LimbDamageSourceStats.text + AdditionTx + "";;
        }
    }

    public string GetTx(){
        return("TEST TEST 123!!!\n");
    }
}
