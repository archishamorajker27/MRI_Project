using UnityEngine;
using TMPro;

public class WorldInteractUI : MonoBehaviour
{
    public TMP_Text textUI;
    public GameObject root;

    public void Show(string message)
    {
        root.SetActive(true);
        textUI.text = message;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}