using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class FmodAudioManager : MonoBehaviour
{
    [SerializeField] private EventReference bgmRef;
    [SerializeField][ParamRef] string param;
    [SerializeField] List<FmodSfxClass> sfxList = new List<FmodSfxClass>();
    public EventInstance bgm;
    public EventInstance ambiance;
    public PLAYBACK_STATE bgmState;
    public static FmodAudioManager instance;
    private int sfxListIndex;
    [SerializeField] private bool PlayBgmOnStart;


    private void Awake()
    {
        bgm = RuntimeManager.CreateInstance(bgmRef);
        instance = this;
        bgm.start();
    }

    //Update inspector UI to match enum to name
    private void OnValidate()
    {
        foreach(FmodSfxClass sfx in sfxList)
        {
            if (sfx != null)
            sfx.name = sfx.sfx.ToString();
        }
    }
    private FmodSfxClass GetSFX(FmodSfxClass.sfxEnums sfxEnum)
    {
        FmodSfxClass sfxElement = sfxList.Find(num => num.sfx == sfxEnum);
        return sfxElement;
    }
    //Play oneshot via your sfx enum, include gameObject world position.
    public void PlayAndAttachOneShot(FmodSfxClass.sfxEnums sfxEnum , Vector3 position = new Vector3())
    {
        var sfx = GetSFX(sfxEnum);
        var instance = RuntimeManager.CreateInstance(sfx.path);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
    //OneShot overload with parameter Control.
    public void PlayAndAttachOneShot(FmodSfxClass.sfxEnums sfxEnum , string ParamName, int paramValue ,Vector3 position)  
    {
        var sfxElement = sfxEnum;
        FmodSfxClass sfxToPlay = sfxList.Find(num => num.sfx == sfxEnum);
        sfxToPlay.posInWorld = position;
        var instance = RuntimeManager.CreateInstance(sfxToPlay.path);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
    //Set event parameter by passing your event, your param name and value.
    public  void SetEventParameter(EventInstance fmodEvent, string paramName , int paramValue) => fmodEvent.setParameterByName(paramName, paramValue);

    public void PlayEvent(EventInstance fmodEvent)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.start();
        }
    }
    public void StopEvent(EventInstance fmodEvent)
    {
        fmodEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fmodEvent.release();
    }
    //Get playback state of every event playing by passing it in.
    private PLAYBACK_STATE PlaybackState(EventInstance instance)
    {
        instance.getPlaybackState(out PLAYBACK_STATE state);
        return state;
    }
}
