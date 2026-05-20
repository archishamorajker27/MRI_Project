using UnityEngine;
using DG.Tweening;


 namespace LGA_SYS
{
     public class TweenDoor : MonoBehaviour, IInteractable
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Header("Rotation Settings")]
    public Axis directionAxis = Axis.Y;

    public bool invertDirection = false;

    public float openAngle = 90f;

    public float duration = 0.4f;

    // --------------------------------------------------

    private bool isOpen = false;

    private Quaternion closedRotation;

    private Tween currentTween;

    // --------------------------------------------------

    void Start()
    {
        closedRotation = transform.localRotation;
    }

    // --------------------------------------------------

    public void Interact(Transform interactor)
    {
        isOpen = !isOpen;

        ToggleDoor(interactor.position, isOpen);
    }

    // --------------------------------------------------
    // CORE LOGIC
    // --------------------------------------------------
    public void OnInteract()
    {
       
    }
    void ToggleDoor(Vector3 playerPos, bool openState)
    {
        if (currentTween != null &&
            currentTween.IsActive())
        {
            currentTween.Kill();
        }

        if (openState)
        {
            float finalAngle =
                CalculateAngle(playerPos);

            Vector3 rotationAxis =
                GetRotationAxis(finalAngle);

            Quaternion targetRot =
                closedRotation *
                Quaternion.Euler(rotationAxis);

            currentTween =
                transform
                .DOLocalRotateQuaternion(
                    targetRot,
                    duration)
                .SetEase(Ease.OutCubic);
        }
        else
        {
            currentTween =
                transform
                .DOLocalRotateQuaternion(
                    closedRotation,
                    duration)
                .SetEase(Ease.InOutCubic);
        }
    }

    // --------------------------------------------------

    Vector3 GetRotationAxis(float angle)
    {
        switch (directionAxis)
        {
            case Axis.X:
                return new Vector3(angle, 0, 0);

            case Axis.Y:
                return new Vector3(0, angle, 0);

            case Axis.Z:
                return new Vector3(0, 0, angle);
        }

        return Vector3.zero;
    }

    // --------------------------------------------------

    float CalculateAngle(Vector3 playerPos)
    {
        Vector3 localPlayerPos =
            transform.InverseTransformPoint(playerPos);

        float value = 0f;

        switch (directionAxis)
        {
            case Axis.X:
                value = localPlayerPos.x;
                break;

            case Axis.Y:
                value = localPlayerPos.y;
                break;

            case Axis.Z:
                value = localPlayerPos.z;
                break;
        }

        float finalAngle =
            value > 0
            ? -openAngle
            : openAngle;

        if (invertDirection)
        {
            finalAngle = -finalAngle;
        }

        return finalAngle;
    }

    // --------------------------------------------------

    public string GetInteractText()
    {
        return isOpen
            ? "Close Door"
            : "Open Door";
    }
}
}