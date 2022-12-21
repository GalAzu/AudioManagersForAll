using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
public class FmodSoundGenerator : MonoBehaviour
{
    public EventReference soundToPlayPath;
    public UnityEvent eventToPlay;

    public void PlaySound() => FmodAudioManager.instance.PlayAndAttachOneShot(soundToPlayPath, transform.position);
}
