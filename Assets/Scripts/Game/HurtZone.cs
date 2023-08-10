using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HurtZone : MonoBehaviour
{
    public GameObject hurtZone;
    public GameObject hurtZoneTwo;
    public GameObject grabZone;

    //private Movement movement;
    private CameraLook cameraLook;

    public GameObject player;

    public GameObject machineGun;
    public GameObject pistol;
    public GameObject hands;

    private GrabZone grabZoneScript;

    public Animator animatorForGrabAnimation;

    public Animator enemyAnimator;


    void Start()
    {
        //movement = player.GetComponent<Movement>();
        cameraLook = GameObject.Find("SK_FP_CH_Default_Root").GetComponent<CameraLook>();
        grabZoneScript = grabZone.GetComponent<GrabZone>();
        enemyAnimator = GameObject.Find("Ch30_nonPBR@T-Pose").GetComponent<Animator>();
    }
    void Update()
    {
        if (grabZoneScript.startCoroutiner)
        {
            StartCoroutine(MovementBack());
        }

    }

    public void SetHurtZoneActive()
    {
        hurtZone.SetActive(true);
    }

    public void SetHurtZoneFalse()
    {
        hurtZone.SetActive(false);
    }
    public void SetHurtZoneTwoActive()
    {
        hurtZoneTwo.SetActive(true);
    }

    public void SetHurtZoneTwoFalse()
    {
        hurtZoneTwo.SetActive(false);
    }
    public void SetGrabZoneActive()
    {
        grabZone.SetActive(true);
    }

    public void SetGrabZoneFalse()
    {
        grabZone.SetActive(false);
    }

    public IEnumerator MovementBack()
    {
        animatorForGrabAnimation.Play("grabbed");
        cameraLook.enabled = false;
        machineGun.SetActive(false);
        pistol.SetActive(false);
        hands.SetActive(false);
        yield return new WaitForSeconds(3.2f);
        //movement.enabled = true;
        grabZoneScript.startCoroutiner = false;
        machineGun.SetActive(true);
        pistol.SetActive(true);
        hands.SetActive(true);
        cameraLook.enabled = true;
    }
}

