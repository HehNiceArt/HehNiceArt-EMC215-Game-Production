using UnityEngine;

public class TimeAttack : MonoBehaviour
{
    [SerializeField] private PatientSpawn patientSpawnManager;
    [SerializeField] GameObject timeAttackUI;
    [SerializeField] GameObject timeAttackToolTip;
    private float timeElapsed = 0f;
    private float timeThreshold = 180f; // 3 minutes in seconds
    private float fastSpawnRate = 5f; // 5 seconds spawn rate
    private float timeAttackDuration = 60f;
    float normalSpawnRate;
    bool isTimeAttackActive = false;

    bool isPressed = false;

    void Start()
    {
        if (patientSpawnManager == null)
        {
#pragma warning disable
            patientSpawnManager = FindObjectOfType<PatientSpawn>();
        }
        timeAttackUI.SetActive(false);
        normalSpawnRate = patientSpawnManager.GetSpawnRate();
    }
    public void ShowToolTip()
    {
        isPressed = !isPressed;
        timeAttackToolTip.SetActive(isPressed);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (!isTimeAttackActive && timeElapsed >= timeThreshold)
        {
            /*
            timeAttackUI.SetActive(true);
            patientSpawnManager.SetFastSpawnRate(fastSpawnRate);

            Debug.Log($"Time Attack activated! Spawn rate increased to {fastSpawnRate} seconds");
            timeElapsed = 0f;
            */
            StartTimeAttack();
        }
        else if (isTimeAttackActive && timeElapsed >= timeAttackDuration)
        {
            EndTimeAttack();
        }
    }
    void StartTimeAttack()
    {
        isTimeAttackActive = true;
        timeElapsed = 0f;
        timeAttackUI.SetActive(true);
        patientSpawnManager.SetFastSpawnRate(fastSpawnRate);
        Debug.Log($"Time attack activated! {fastSpawnRate} seconds");
    }
    void EndTimeAttack()
    {
        isTimeAttackActive = false;
        timeElapsed = 0f;
        timeAttackUI.SetActive(false);
        patientSpawnManager.SetFastSpawnRate(normalSpawnRate);
        Debug.Log($"Time attack ended! Spawn rate reset to {normalSpawnRate} seconds");
    }
}
