using System.Collections;
using UnityEngine;

public class MRIBedController : MonoBehaviour
{
    [Header("Rig & Bed References")]
    public Transform mriBed;
    public Transform vrRig;
    public Transform lyingAnchor;

    [Header("Animation Milestones")]
    public Transform targetInside;
    public Transform targetOutside;

    [Header("System Dependencies")]
    public MRINotificationUI uiController;
    public MRIScanProcess scannerCore;

    private bool canLieDown = false;
    private bool isPatientSeated = false;

    void Update()
    {
        if (canLieDown && !isPatientSeated)
        {
            // Standard Keyboard trigger space fallback / VR Interaction button check
            if (Input.GetKeyDown(KeyCode.Space))// || OVRInput.GetDown(OVRInput.Button.One))
            {
                StartCoroutine(MountPatientToBed());
            }
        }
    }

    public void PrepareForPatient()
    {
        canLieDown = true;
        if (uiController != null)
            uiController.DisplayInstruction("Press your Action Button to safely lie down onto the scanning platform.");
    }

    private IEnumerator MountPatientToBed()
    {
        isPatientSeated = true;
        canLieDown = false;

        if (vrRig == null) vrRig = Camera.main.transform.root;
        vrRig.SetParent(lyingAnchor);

        float elapsed = 0;
        while (elapsed < 2.0f)
        {
            vrRig.localPosition = Vector3.Lerp(vrRig.localPosition, Vector3.zero, elapsed / 2.0f);
            vrRig.localRotation = Quaternion.Slerp(vrRig.localRotation, Quaternion.identity, elapsed / 2.0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (uiController != null) uiController.HideUI();

        // Advance into scanning bay tunnel
        yield return StartCoroutine(LerpPosition(mriBed, targetInside.position, 3.0f));

        // Handover control pattern logic loop to the Scanner Core
        if (scannerCore != null)
        {
            scannerCore.StartScanSequence(this);
        }
    }

    public void EjectBed()
    {
        StartCoroutine(RetractBedSequence());
    }

    private IEnumerator RetractBedSequence()
    {
        if (uiController != null)
            uiController.DisplayInstruction("Scan sequence complete. Deploying safely outward.");

        yield return StartCoroutine(LerpPosition(mriBed, targetOutside.position, 3.0f));

        if (vrRig != null) vrRig.SetParent(null);

        if (uiController != null)
            uiController.DisplayInstruction("Procedure complete. Thank you for staying completely still!");
    }

    private IEnumerator LerpPosition(Transform targets, Vector3 destination, float timeframe)
    {
        float time = 0;
        Vector3 initialPos = targets.position;
        while (time < timeframe)
        {
            targets.position = Vector3.Lerp(initialPos, destination, time / timeframe);
            time += Time.deltaTime;
            yield return null;
        }
        targets.position = destination;
    }
}