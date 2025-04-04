using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string gameScene = "";
    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}
