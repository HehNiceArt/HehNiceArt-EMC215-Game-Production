using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        // Load and apply saved volume on game start
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 2f);
        SetMusicVolume(savedVolume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
}