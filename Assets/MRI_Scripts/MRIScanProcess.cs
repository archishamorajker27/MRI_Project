using System.Collections;
using UnityEngine;

public class MRIScanProcess : MonoBehaviour
{
    [Header("Audio & Lighting Assets")]
    public AudioSource mriAudio;
    public AudioClip scanLoopClip;
    public Light tunnelBoreLight;

    [Header("Medical Variable Tuners")]
    public float scanDuration = 15.0f;
    public float movementThreshold = 0.15f;

    [Header("System Dependencies")]
    public MRINotificationUI uiController;

    private MRIBedController operatingBedController;
    private Vector3 baselineCameraLocation;
    private bool isScanningActive = false;

    public void StartScanSequence(MRIBedController connectedBed)
    {
        operatingBedController = connectedBed;
        StartCoroutine(ExecuteScanCycle());
    }

    private IEnumerator ExecuteScanCycle()
    {
        isScanningActive = true;
        baselineCameraLocation = Camera.main.transform.position;

        if (mriAudio != null && scanLoopClip != null)
        {
            mriAudio.clip = scanLoopClip;
            mriAudio.loop = true;
            mriAudio.Play();
        }

        float clockTimer = scanDuration;
        while (clockTimer > 0)
        {
            EvaluateAnxietyLoop();
            clockTimer -= Time.deltaTime;
            yield return null;
        }

        // Close down operation loops
        isScanningActive = false;
        if (mriAudio != null) mriAudio.Stop();
        if (tunnelBoreLight != null) tunnelBoreLight.color = Color.white;

        if (operatingBedController != null)
        {
            operatingBedController.EjectBed();
        }
    }

    private void EvaluateAnxietyLoop()
    {
        if (!isScanningActive) return;

        Vector3 currentCamLocation = Camera.main.transform.position;
        float instantaneousMovement = Vector3.Distance(currentCamLocation, baselineCameraLocation) / Time.deltaTime;
        baselineCameraLocation = currentCamLocation;

        if (instantaneousMovement > movementThreshold)
        {
            if (uiController != null)
                uiController.DisplayInstruction("Motion detected. Please try to remain completely calm and motionless.");

            if (mriAudio != null) mriAudio.pitch = 0.75f;        // Sound variation feedback
            if (tunnelBoreLight != null) tunnelBoreLight.color = Color.cyan; // Calming color tint shift
        }
        else
        {
            if (mriAudio != null) mriAudio.pitch = 1.0f;
            if (tunnelBoreLight != null) tunnelBoreLight.color = Color.white;
        }
    }
}