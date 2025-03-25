using System.Collections;
using UnityEngine;

public class PatientSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] patientPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] float[] spawnWeights = new float[] { 50, 25, 15, 7, 3 };

    ReputationManager reputationManager;

    void Start()
    {
        reputationManager = GetComponent<ReputationManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnPatients());
        }
    }
    IEnumerator SpawnPatients()
    {
        while (true)
        {
            float patientSpawnRate = reputationManager.GetPatientSpawnRate();
            Debug.Log($"Patinet Spawn Rate {patientSpawnRate}");
            yield return new WaitForSeconds(patientSpawnRate);
            SpawnPatient();
        }
    }
    void GameOver()
    {
        Debug.Log("Game Over - The Hospital has declared bankruptcy!");
    }
    void SpawnPatient()
    {
        float totalWeight = 0;
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            totalWeight += spawnWeights[i];
        }

        float randomPoint = Random.value * totalWeight;
        float currentWeight = 0;

        for (int i = 0; i < spawnWeights.Length; i++)
        {
            currentWeight += spawnWeights[i];
            if (randomPoint <= currentWeight)
            {
                Instantiate(patientPrefab[i], patientSpawnPoint.position, Quaternion.identity);
                return;
            }
        }

        // Fallback in case of floating-point precision issues
        Instantiate(patientPrefab[spawnWeights.Length - 1], patientSpawnPoint.position, Quaternion.identity);
    }
}
