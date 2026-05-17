using UnityEngine;
using UnityEngine.InputSystem;

namespace LGA_SYS
{
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 4f;

    public LayerMask interactLayer;

    [Header("References")]
    public Camera playerCamera;

    private IInteractable currentInteractable;

    private WorldInteractUI currentUI;

    private void Update()
    {
        if (playerCamera == null)
            return;

        DetectInteractable();

        HandleInput();
    }

    // --------------------------------------------------

    void DetectInteractable()
    {
        currentInteractable = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        bool didHit = Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance,
            interactLayer);

        Debug.DrawRay(
            ray.origin,
            ray.direction * interactDistance,
            didHit ? Color.green : Color.red);

        if (!didHit)
        {
            ClearUI();
            return;
        }

        IInteractable interactable =
            hit.collider.GetComponent<IInteractable>();

        if (interactable == null)
        {
            ClearUI();
            return;
        }

        currentInteractable = interactable;

        HandleUI(hit, interactable);
    }

    // --------------------------------------------------

    void HandleInput()
    {
        if (currentInteractable == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact(transform);
        }
    }

    // --------------------------------------------------

    void HandleUI(
        RaycastHit hit,
        IInteractable interactable)
    {
        WorldInteractUI ui =
            hit.collider.GetComponent<WorldInteractUI>();

        if (ui == null)
            return;

        if (currentUI != ui)
        {
            ClearUI();

            currentUI = ui;
        }

        currentUI.Show(
            "[E] " +
            interactable.GetInteractText());
    }

    // --------------------------------------------------

    void ClearUI()
    {
        if (currentUI != null)
        {
            currentUI.Hide();

            currentUI = null;
        }
    }
}
}