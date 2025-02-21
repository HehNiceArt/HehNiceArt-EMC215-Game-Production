using System.Collections;
using UnityEngine;

public class PatientSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] patientPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] float patientSpawnRate = 30f;
    [SerializeField] float[] spawnWeights = new float[] { 60, 20, 10, 7, 3 };

    void Start()
    {
        StartCoroutine(SpawnPatients());
    }
    IEnumerator SpawnPatients()
    {
        while (true)
        {
            yield return new WaitForSeconds(patientSpawnRate);
            SpawnPatient();
        }
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
