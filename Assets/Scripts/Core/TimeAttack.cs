using UnityEngine;

public class TimeAttack : MonoBehaviour
{
    [SerializeField] private PatientSpawn patientSpawnManager;
    private float timeElapsed = 0f;
    private float timeThreshold = 180f; // 3 minutes in seconds
    private float fastSpawnRate = 5f; // 5 seconds spawn rate

    void Start()
    {
        if (patientSpawnManager == null)
        {
#pragma warning disable
            patientSpawnManager = FindObjectOfType<PatientSpawn>();
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Check if 3 minutes have passed
        if (timeElapsed >= timeThreshold)
        {
            // Trigger faster spawn rate
            patientSpawnManager.SetFastSpawnRate(fastSpawnRate);

            Debug.Log($"Time Attack activated! Spawn rate increased to {fastSpawnRate} seconds");
            // Reset timer for next 3-minute interval
            timeElapsed = 0f;
        }
    }
}
