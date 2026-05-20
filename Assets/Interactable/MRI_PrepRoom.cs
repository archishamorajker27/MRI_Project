

public class MRI_PrepRoom : MonoBehaviour,IInteractable
{
    public WorldInteractUI worldInteractUI;
        private bool Get_Drop = false;
    public void Interact(Transform interactor)
    {
        interactor =null;
    }

    public void OnInteract()
    {

        worldInteractUI.Show(
            "[E] " +
            GetInteractText());
    }
     public string GetInteractText()
    {    
        return Get_Drop
            ? "Get Items"
            : "Drop Items";
    }
}
