using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image loadbarFill;

    public void LoadSceneUI(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneId);
        LoadingScreen.SetActive(true);
        while (!op.isDone)
        {
            float progressVal = Mathf.Clamp01(op.progress / 0.9f);
            loadbarFill.fillAmount = progressVal;
            yield return null;
        }
    }
}
