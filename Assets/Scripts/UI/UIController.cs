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
    [SerializeField] GameObject[] areas;


    void Start()
    {
        StartCoroutine(TutShow());
        doubleTimeButton.image.sprite = single;
    }
    IEnumerator TutShow()
    {
        tutorialUI.SetActive(true);
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(false);
        }
        yield return new WaitForSeconds(10f);
        tutorialUI.SetActive(false);
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].SetActive(true);
        }
    }
    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(1)) && tutorialUI.activeSelf)
        {
            isActive = !isActive;
            tutorialUI.SetActive(isActive);
            StopCoroutine(TutShow());
            for (int i = 0; i < areas.Length; i++)
            {
                areas[i].SetActive(true);
            }
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
