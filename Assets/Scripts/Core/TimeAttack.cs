using UnityEngine;

public class TimeAttack : MonoBehaviour
{
    [SerializeField] private PatientSpawn patientSpawnManager;
    [SerializeField] GameObject timeAttackUI;
    [SerializeField] GameObject timeAttackToolTip;
    private float timeElapsed = 0f;
    private float timeThreshold = 10f; // 3 minutes in seconds
    private float fastSpawnRate = 5f; // 5 seconds spawn rate

    bool isPressed = false;

    void Start()
    {
        if (patientSpawnManager == null)
        {
#pragma warning disable
            patientSpawnManager = FindObjectOfType<PatientSpawn>();
        }
        timeAttackUI.SetActive(false);
    }
    public void ShowToolTip()
    {
        isPressed = !isPressed;
        timeAttackToolTip.SetActive(isPressed);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeThreshold)
        {

            timeAttackUI.SetActive(true);
            patientSpawnManager.SetFastSpawnRate(fastSpawnRate);

            Debug.Log($"Time Attack activated! Spawn rate increased to {fastSpawnRate} seconds");
            timeElapsed = 0f;
        }
    }
}
