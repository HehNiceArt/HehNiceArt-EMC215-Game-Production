using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;
    [SerializeField] Sprite single, doubleTime;
    [SerializeField] Button doubleTimeButton;
    [SerializeField] GameObject[] locks;
    [SerializeField] GameObject[] gears;
    bool isActive = false;
    bool isDoubleTime = false;

    void Start()
    {
        StartCoroutine(TutShow());
        doubleTimeButton.image.sprite = single;
    }
    IEnumerator TutShow()
    {
        tutorialUI.SetActive(true);
        yield return new WaitForSeconds(10f);
        tutorialUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && tutorialUI.activeSelf)
        {
            isActive = !isActive;
            StopCoroutine(TutShow());
            tutorialUI.SetActive(isActive);
        }
    }
    public void ShowTutorial()
    {
        isActive = !isActive;
        tutorialUI.SetActive(isActive);
    }

    public void DoubleTme()
    {
        isDoubleTime = !isDoubleTime;

        if (isDoubleTime)
        {
            doubleTimeButton.image.sprite = doubleTime;
            Time.timeScale = 2;
        }
        else
        {
            doubleTimeButton.image.sprite = single;
            Time.timeScale = 1;
        }
    }
}
