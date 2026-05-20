using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MRI_MachineUse : MonoBehaviour, IInteractable
{ bool UseMRI = false;
    public GameObject playerCam;
    public GameObject mriCam;
    public GameObject Player;
    public Animator MRIAnimator;
    public string GetInteractText()
    {
        return UseMRI ? "No use" : "Use";
    }

    public void Interact(Transform interactor)
    {

    }

    public void OnInteract()
    {
        UseMRI = !UseMRI;
        playerCam.SetActive(UseMRI);
        mriCam.SetActive(!UseMRI);
        Player.SetActive(UseMRI);
        StartCoroutine(DelayonMovingBed());
    }

    IEnumerator DelayonMovingBed ()
    {
        yield return new WaitForSeconds(2);
        MRIAnimator.SetBool("MRI_INOUT",true);
        yield return new WaitForSeconds(6);
        MRIAnimator.SetBool("MRI_INOUT", false);
    }

}
