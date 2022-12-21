using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System;

public class FmodAudioManager : MonoBehaviour
{
    [Header("Level audio")]
    [SerializeField] private EventReference ambianceRef;
    [SerializeField] private EventReference bgmRef;
    [SerializeField] List<FmodSfxClass> sfxList = new List<FmodSfxClass>();
    [SerializeField] List<FmodSnapshots> sceneSnapShots = new List<FmodSnapshots>();
    [Header("Dependencies")]
    [SerializeField] private StudioListener mainListener;
    [SerializeField] private ParamRef bgm_dynamics;
    public Transform ambianceTransform;
    [SerializeField]private EventInstance bgm;
    [SerializeField] private EventInstance ambiance;
    [HideInInspector]public PLAYBACK_STATE bgmState;
    [SerializeField] private bool playOnAwake;
    public static FmodAudioManager instance;

    private void Awake()
    {
        instance = this;
        bgm = RuntimeManager.CreateInstance(bgmRef);
        ambiance = RuntimeManager.CreateInstance(ambianceRef);
        PopulateSceneSnapshots();
    }
    private void Start()
    {
        if(playOnAwake)
        {
            bgm.start();
            ambiance.start();
        }
    }

    private void PopulateSceneSnapshots()
    {
        foreach (var snap in sceneSnapShots)
        {
            snap.instance = RuntimeManager.CreateInstance(snap.path);
        }
    }

    internal void PlayAndAttachOneShot(FmodSfxClass.sfxEnums footsteps, object material, string v, Vector3 position)
    {
        throw new NotImplementedException();
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
    #region sfxHandler

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
    public void PlayAndAttachOneShot(FmodSfxClass.sfxEnums sfxEnum,  Vector3 position, ParamRef param , int paramValue)
    {
        param.Value = paramValue;
        var sfxElement = sfxEnum;
        FmodSfxClass sfxToPlay = sfxList.Find(num => num.sfx == sfxEnum);
        var instance = RuntimeManager.CreateInstance(sfxToPlay.path);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
    public void PlayAndAttachOneShot(EventReference eventRef, Vector3 position = new Vector3())
    {
        var instance = RuntimeManager.CreateInstance(eventRef);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
    public void PlayAndAttachOneShot(EventReference eventRef, ParamRef param, int paramValue, Vector3 position)
    {
        param.Value = paramValue;
        var instance = RuntimeManager.CreateInstance(eventRef);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }
   
    #endregion
    #region eventsHandler
    public void PlayEvent(EventInstance fmodEvent, Vector3 posInWorld)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    public void PlayEvent(EventInstance fmodEvent, Vector3 posInWorld, string paramName, int paramValue)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.setParameterByName(paramName, paramValue);
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    public void StopEvent(EventInstance fmodEvent)
    {
        fmodEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fmodEvent.release();
    }
    //Set event parameter by passing your event, your param name and value.
    public void SetEventParameter(EventInstance fmodEvent, string paramName , int paramValue) => fmodEvent.setParameterByName(paramName, paramValue);

    public void PlayEvent(EventInstance fmodEvent)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.start();
        }
    }

    //Get playback state of every event playing by passing it in.
    private PLAYBACK_STATE PlaybackState(EventInstance instance)
    {
        instance.getPlaybackState(out PLAYBACK_STATE state);
        return state;
    }
    #endregion


}
