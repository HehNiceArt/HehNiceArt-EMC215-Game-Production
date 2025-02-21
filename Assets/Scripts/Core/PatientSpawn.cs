using System.Collections;
using UnityEngine;

public class PatientSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] patientPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] float baseSpawnRate = 30f;
    [SerializeField] float patientSpawnRate = 7f;
    [SerializeField] float[] spawnWeights = new float[] { 50, 25, 15, 7, 3 };

    void Start()
    {
        StartCoroutine(SpawnPatients());
    }
    float GetSpawnRateMultiplier()
    {
        if (playerDetails.reputation <= SO_PlayerDetails.MIN_REPUTATION)
        {
            return 0f;
        }
        else if (playerDetails.reputation <= 0)
        {
            return 0.3f;
        }
        else if (playerDetails.reputation >= SO_PlayerDetails.REPUTATION_TIER_4)
        {
            return 0.8f;
        }
        else if (playerDetails.reputation >= SO_PlayerDetails.REPUTATION_TIER_3)
        {
            return 0.7f;
        }
        else if (playerDetails.reputation >= SO_PlayerDetails.REPUTATION_TIER_2)
        {
            return 0.6f;
        }
        else if (playerDetails.reputation >= SO_PlayerDetails.REPUTATION_TIER_1)
        {
            return 0.5f;
        }

        return 0.4f;
    }
    IEnumerator SpawnPatients()
    {
        while (true)
        {
            float currentSpawnRate = baseSpawnRate;
            Debug.Log(currentSpawnRate);

            if (currentSpawnRate <= 0)
            {
                GameOver();
                yield break;
            }

            yield return new WaitForSeconds(currentSpawnRate);
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
