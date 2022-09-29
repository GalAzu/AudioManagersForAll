using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodOcclusion : MonoBehaviour
{
    [Header("Fmod Event")]  //Declaring on Fmod Event.
    [FMODUnity.EventRef]
    public string bgm;
    FMOD.Studio.EventInstance _bgmEvent;
    Transform playerLocation;  //reference to our player transform to shoot raycast.
    [Header("Occlusion Options")] //Occlusion, Fmod parameters values.
    [Range(0f, 1f)]
    public float VolumeValue = 0.5f;
    [Range(10f, 22000f)]
    public float LPCutoff = 10000f;
    public LayerMask OcclusionLayer = 1;
    private void Awake()
    {
        playerLocation = GameObject.FindObjectOfType<StudioListener>().transform;
        _bgmEvent = RuntimeManager.CreateInstance(bgm);
    }
    void Start()
    {
        FMOD.Studio.PLAYBACK_STATE pbState;
        _bgmEvent.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            _bgmEvent.start();
        }
    }

    void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(_bgmEvent, GetComponent<Transform>(), GetComponent<Rigidbody>()); //Getting player position.
        RaycastHit hit;
        Physics.Linecast(transform.position, playerLocation.position, out hit, OcclusionLayer); //Shooting raycast towards the player to see if there is an object on the way to occlude sound.
        if (hit.collider != null)
        {
            if (hit.collider.name == "Player")
            {
                NotOccluded();
                Debug.DrawLine(transform.position, playerLocation.position, Color.blue);

            }
            else
            {
                Occluded();
                Debug.DrawLine(transform.position, playerLocation.position, Color.red);
            }


        }
        //NOTE: Today I would trigger snapshots according to the relevent occlusion.
        void Occluded()
        {
            _bgmEvent.setParameterByName("Volume", VolumeValue);
            _bgmEvent.setParameterByName("LPF", LPCutoff);
        }
        void NotOccluded()
        {
            _bgmEvent.setParameterByName("Volume", 1f);
            _bgmEvent.setParameterByName("LPF", 220000);
        }
    }

}

