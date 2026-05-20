using UnityEngine;
using TMPro;

public class MRINotificationUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject canvasContainer;
    public TextMeshProUGUI instructionText;

    public void DisplayInstruction(string message)
    {
        if (canvasContainer != null && instructionText != null)
        {
            canvasContainer.SetActive(true);
            instructionText.text = message;
        }
    }

    public void HideUI()
    {
        if (canvasContainer != null)
        {
            canvasContainer.SetActive(false);
        }
    }
}