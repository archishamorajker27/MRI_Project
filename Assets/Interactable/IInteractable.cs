using UnityEngine;


public interface IInteractable
{
    void Interact(Transform interactor);
    void OnInteract();
    string GetInteractText();
}

