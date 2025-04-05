using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("----------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("-----------Audio Clip------------")]
    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip buttonSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlaySceneMusic();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic();
    }

    private void PlaySceneMusic()
    {
        AudioClip targetClip = SceneManager.GetActiveScene().name == "Title_Scene" ? 
            mainMenuBGM : gameBGM;

        if (musicSource.clip != targetClip)
        {
            musicSource.clip = targetClip;
            musicSource.Play();
        }
    }

    public void PlayButtonSound()
    {
        sfxSource.PlayOneShot(buttonSound);
    }
}