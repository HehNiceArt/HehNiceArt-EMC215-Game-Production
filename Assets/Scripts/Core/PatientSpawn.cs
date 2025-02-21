using System.Collections;
using UnityEngine;

public class PatientSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] patientPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] float patientSpawnRate = 30f;

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
        int rand = Random.Range(0, patientPrefab.Length);
        Instantiate(patientPrefab[rand], patientSpawnPoint.position, Quaternion.identity);
    }
}
