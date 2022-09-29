using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioUnit 
{
    public string name;
    [Range(0, 1)]
    public float volume = 1;
    public float pitch;
    public enum SfxToPlay {explosion , collect }
    public AudioClip clip;
    public enum BgmToPlay { level1, level2, level3 }
    public bool randomPitch;
    public bool randomVol;
}
[System.Serializable]

public class AudioUnitBGM : AudioUnit
{
    public BgmToPlay bgmToPlay;
}
[System.Serializable]
public class AudioUnitSFX : AudioUnit
{
    public AudioSource source;
    public SfxToPlay sfxToPlay;


}







