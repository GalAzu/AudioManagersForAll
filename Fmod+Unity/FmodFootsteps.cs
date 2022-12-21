using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.InputSystem;

public class FmodFootsteps : MonoBehaviour
{
    [SerializeField]private Transform footstepsTransform;
    [SerializeField] private InputActionReference action;
    [SerializeField] private ParamRef materialParam;
    [SerializeField]private LayerMask Ground;
    public enum FootstepsMaterial {Wood , Gravel , Stone, Metal }
    public enum MoveState {Walk , Run , Crouch }
    private FootstepsMaterial footstepsMaterial;
    private int footstepsMaterialID;
    private MoveState moveState;

    public float curTime;
    public float timeBetweenSteps;

    private void Update()
    {
        CheckMovement();
    }
    private void CheckGroundMaterial()
    {
        RaycastHit ray;
        if (Physics.Raycast(transform.position,Vector3.down,out ray, 1 , Ground))
        {

        }
    }

    private void CheckMovement()
    {
        if (action.action.ReadValue<Vector2>().magnitude > 0f)
        {
            PlayStep();
        }
    }

    private void PlayStep()
    {
        curTime += Time.deltaTime;
        if(curTime >= timeBetweenSteps)
        {
            curTime = 0;
            FmodAudioManager.instance.PlayAndAttachOneShot(FmodSfxClass.sfxEnums.footsteps,materialParam, footstepsMaterial.ToString(), footstepsTransform.position);
        }
    }
}
