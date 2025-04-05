using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSound();
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
        }
    }

    void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        button.onClick.RemoveListener(PlaySound);
    }
}