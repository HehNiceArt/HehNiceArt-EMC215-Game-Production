using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        InitializeSlider();
    }

    private void InitializeSlider()
    {
        slider.value = PlayerPrefs.GetFloat("musicVolume", 1f);

        VolumeSettings volumeSettings = FindAnyObjectByType<VolumeSettings>();
        if (volumeSettings != null)
        {
            slider.onValueChanged.AddListener(volumeSettings.SetMusicVolume);
        }
        else
        {
            Debug.LogError("VolumeSettings not found!");
        }
    }
}