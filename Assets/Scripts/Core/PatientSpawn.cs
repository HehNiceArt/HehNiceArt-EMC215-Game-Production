using System.Collections;
using System.Linq;
using UnityEngine;

public class PatientSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] patientPrefab;
    [SerializeField] Transform[] patientSpawnPoints;
    [SerializeField] SO_PlayerDetails playerDetails;
    [SerializeField] float[] spawnWeights = new float[] { 50, 25, 15, 7, 3 };
    [SerializeField] Room firstRoom;

    ReputationManager reputationManager;
    private float currentSpawnRate;
    private bool isTimeAttackActive = false;

    void Start()
    {
        reputationManager = GetComponent<ReputationManager>();
        StartCoroutine(CheckFirstAreaAndTable());
    }
    IEnumerator CheckFirstAreaAndTable()
    {
        while (true)
        {
            if (!firstRoom.areaDetails.isLocked && firstRoom.treatmentTables.Any(table => !table.isTableLocked))
            {
                Debug.Log("Start patient spawn!");
                StartCoroutine(SpawnPatients());
                yield break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawnPatients()
    {
        while (true)
        {
            float patientSpawnRate = isTimeAttackActive ? currentSpawnRate : reputationManager.GetPatientSpawnRate();
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
                int rand = Random.Range(0, patientSpawnPoints.Length);
                Instantiate(patientPrefab[i], patientSpawnPoints[rand].position, Quaternion.identity);
                return;
            }
        }

        // Fallback in case of floating-point precision issues
        Instantiate(patientPrefab[spawnWeights.Length - 1], patientSpawnPoints[0].position, Quaternion.identity);
    }

    public void SetFastSpawnRate(float newSpawnRate)
    {
        isTimeAttackActive = true;
        currentSpawnRate = newSpawnRate;
    }
}
