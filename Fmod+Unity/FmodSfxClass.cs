using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
[System.Serializable]

//Defining each sfx.
public class FmodSfxClass

{
    public string name;
    public enum sfxEnums { uiClick, uiStart, Jump, Interact, Collect , ShootLight , ShootHeavy }
    public  sfxEnums sfx;
    public EventReference path;
    [HideInInspector]public Vector3 posInWorld;
}


