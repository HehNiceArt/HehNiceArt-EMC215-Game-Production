using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject creditsUI;

    bool isActive = false;
    public void ShowCredits()
    {
        isActive = !isActive;
        creditsUI.SetActive(isActive);
    }

}
