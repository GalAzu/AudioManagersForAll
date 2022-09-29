using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource bgmSource, ambianceSource, staticSFX, dynamicSFX;
    public List<AudioUnitBGM> bgmList = new List<AudioUnitBGM>();
    public List<AudioUnitSFX> sfxList = new List<AudioUnitSFX>();
    private Dictionary<AudioSource, Transform> sfxSourceDictionary; // In the future it will hold multiple audio sources for control and dynamic positions.
    public static AudioManager instance;
    public AudioUnitBGM.BgmToPlay bgmToPlay;
    public bool playBgmOnStart;
    
    //Awake happens once when the game is starting
    private void Awake()
    {
        instance = this;
        foreach (AudioUnitSFX sfx in sfxList)
        {
            if (sfx.source == null) sfx.source = dynamicSFX;
        }
    }

    //Start happens at the first frame
    private void Start()
    {
        if(bgmSource != null && bgmSource.clip != null)
     //   sfxSourceDictionary.Add(dynamicSFX, dynamicSFX.transform);
        if (playBgmOnStart) PlayBGM(bgmToPlay);
    }
    //OnValidate happens every time Unity Editor's refreshing itself.
    private void OnValidate() 
    {
        foreach (var obj in bgmList)
        {
            obj.name = obj.bgmToPlay.ToString();
        }
        foreach (var obj in sfxList)
        {
            obj.name = obj.sfxToPlay.ToString();
        }
    }

    #region bgmHandler
    //Get the correct audio clip per enum.
    public AudioUnitBGM GetBGM(AudioUnit.BgmToPlay bgm)
    {
        var audioToPlay = bgmList.Find(num => num.bgmToPlay == bgm);
        return audioToPlay;
    }
    public void PlayBGM(AudioUnitBGM.BgmToPlay bgm)
    {
        var bgmToPlay = GetBGM(bgm);
        bgmSource.clip = bgmToPlay.clip;

    }

    //Stops BGM using fadeOut corutine.
    public void StopBGM()
    {
        StartCoroutine(FadeOut(bgmSource, 2));
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    #endregion

    #region sfxHandler

    //Get audio clip using enums.
    public AudioUnitSFX GetSFX(AudioUnit.SfxToPlay sfx)
    {
        var audioToPlay = sfxList.Find(num => num.sfxToPlay == sfx);
        return audioToPlay;
    }

    //AttachAndPlay sfx for various ways to instantiate sound effect, optionaly with random values of pitch and volume, respectively.
    public void AttachAndPlaySFX(Transform transform, AudioUnitSFX.SfxToPlay sfx, bool randomPitch, bool randomVol)
    {
        var sfxToPlay = GetSFX(sfx);
        sfxToPlay.source.transform.position = transform.position;

        if (randomPitch)
            sfxToPlay.source.pitch = Random.Range(0.8f, 1.2f);

        if (randomVol) 
            sfxToPlay.source.volume = Random.Range(0.6f, 1.1f);
        sfxToPlay.source.PlayOneShot(sfxToPlay.clip);
    }

    public void AttachAndPlaySFX(Transform transform, AudioUnitSFX.SfxToPlay sfx, bool randomPitch, bool randomVol,float delay)
    {
        var sfxToPlay = GetSFX(sfx);
        sfxToPlay.source.transform.position = transform.position;
        if (randomPitch)
            sfxToPlay.source.pitch = Random.Range(0.7f, 1.2f);
        sfxToPlay.source.clip = sfxToPlay.clip;
        if (randomVol)
            sfxToPlay.source.volume = Random.Range(0.6f,1);
        sfxToPlay.source.PlayDelayed(delay);
    }


    //via audio clip
    public void AttachAndPlaySFX(Transform transform, AudioClip clip)
    {
        dynamicSFX.transform.position = transform.position;
        dynamicSFX.PlayOneShot(clip);
    }
    public void AttachAndPlaySFX(Transform transform, AudioClip clip, bool randomPitch)
    {
        if (randomPitch)
            dynamicSFX.pitch = Random.Range(0.7f, 1.2f);
        else
            dynamicSFX.pitch = 1;
        dynamicSFX.transform.position = transform.position;
        dynamicSFX.PlayOneShot(clip);
    }



    //Attach with pre-defining source
    public void AttachAndPlaySFX(Transform transform, AudioUnitSFX.SfxToPlay sfx, AudioSource source , bool randomPitch , bool randomVol)
    {
        var sfxToPlay = GetSFX(sfx); //Find sfx via enum
        source.transform.position = transform.position; // position in world space the audio source
        if (randomVol)
            source.volume = Random.Range(0.8f, 1.1f); // set volume
        source.volume = sfxToPlay.volume;
        if (randomPitch)
            source.pitch = Random.Range(0.8f, 1.1f); // set pitch
        source.PlayOneShot(sfxToPlay.clip); // play one shot
    }
    public void AttachAndPlaySFX(Transform transform, AudioUnitSFX.SfxToPlay sfx, AudioSource source, bool randomPitch, bool randomVol , float delay)
    {
        if(source != null)
        {
            var sfxToPlay = GetSFX(sfx); //Find sfx via enum
            source.transform.position = transform.position; // position in world space the audio source
            if (randomVol)
                source.volume = Random.Range(0.8f, 1.1f); // set volume
            if (randomPitch)
                source.pitch = Random.Range(0.8f, 1.1f); // set pitch
            source.clip = sfxToPlay.clip;
            source.PlayDelayed(delay); // play one shot
        }
    }
    #endregion
}

