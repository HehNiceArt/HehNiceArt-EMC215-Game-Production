using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Room : MonoBehaviour
{
    [SerializeField] List<TableInteraction> treatmentTables = new List<TableInteraction>();
    [SerializeField] Transform patientWaitingArea;
    [SerializeField] SO_AreaDetails areaDetails;
    [SerializeField] float spaceBetweenPatients = 1.5f;
    [SerializeField] int patientsPerRow = 3;
    [SerializeField] int maxWaitingPatients = 6;
    [SerializeField] Pharmacy pharmacy;

    Queue<GameObject> waitingPatients = new Queue<GameObject>();
    Dictionary<GameObject, int> patientPositions = new Dictionary<GameObject, int>();
    bool[] waitingSpots;
    void OnValidate()
    {
        treatmentTables.Clear();
        treatmentTables.AddRange(GetComponentsInChildren<TableInteraction>());
    }
    void Start()
    {
        if (treatmentTables.Count == 0)
        {
            treatmentTables.AddRange(GetComponentsInChildren<TableInteraction>());
        }
        waitingSpots = new bool[maxWaitingPatients];
        StartCoroutine(CheckWaitingPatients());

    }
    public bool CanAcceptPatient()
    {
        if (areaDetails != null && areaDetails.isLocked)
        {
            return false;
        }
        bool hasAvailableSpot = FindFirstAvailableSpot() != -1;
        bool hasAvailableTable = treatmentTables.Any(table => !table.isTableLocked && !table.isOccupied);

        return hasAvailableSpot || hasAvailableTable;
    }

    public void AddPatientToQueue(GameObject patientObj)
    {
        if (patientWaitingArea == null) return;

        Patient patient = patientObj.GetComponent<Patient>();
        if (patient == null) return;

        if (areaDetails != null && areaDetails.whichArea == WhichArea.secondArea || areaDetails.whichArea == WhichArea.fourthArea || areaDetails.whichArea == WhichArea.fifthArea)
            if (ShouldVisitPharmacy())
            {
                Debug.Log("wdawd");
                if (pharmacy != null && pharmacy.CanAcceptPatient())
                {
                    patient.AssignToPharmacy(pharmacy);
                    return;
                }
            }
        if (waitingPatients.Count == 0)
        {
            bool tableFound = false;
            foreach (TableInteraction table in treatmentTables)
            {
                if (table != null && !table.isTableLocked && !table.isOccupied)
                {
                    patient.GetComponent<Patient>()?.AssignToTable(table);
                    tableFound = true;
                    break;
                }
            }
            if (tableFound) return;
        }

        // If no table availbale or there are waiting patients, add to waiting area
        int spotIndex = FindFirstAvailableSpot();
        if (spotIndex == -1) return;

        waitingSpots[spotIndex] = true;
        patientPositions[patient.gameObject] = spotIndex;
        waitingPatients.Enqueue(patient.gameObject);

        if (patient.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agent.SetDestination(CalculateWaitingPosition(spotIndex));
        }

        if (patient.TryGetComponent<Patient>(out Patient patientComponent))
        {
            patientComponent.hasStartedWaiting = true;
            patientComponent.waitingTimer = 0f;
        }
    }
    bool ShouldVisitPharmacy()
    {
        if (areaDetails == null) return false;
        switch (areaDetails.whichArea)
        {
            case WhichArea.secondArea:
                return Random.value < 0.4f;
            case WhichArea.fourthArea:
                return Random.value < 0.5f;
            case WhichArea.fifthArea:
                return Random.value < 0.6f;
            default:
                return false;
        }
    }
    int FindFirstAvailableSpot()
    {
        for (int i = 0; i < waitingSpots.Length; i++)
            if (!waitingSpots[i]) return i;

        return -1;
    }
    Vector3 CalculateWaitingPosition(int queueIndex)
    {
        int row = queueIndex / patientsPerRow;
        int col = queueIndex % patientsPerRow;

        Vector3 basePosition = patientWaitingArea.position;
        Vector3 offset = new Vector3(-col * spaceBetweenPatients, 0, -row * spaceBetweenPatients);
        offset = patientWaitingArea.rotation * offset;

        return basePosition + offset;
    }
    public void RemovePatientFromQueue(GameObject patient)
    {
        if (patientPositions.TryGetValue(patient, out int spotIndex))
        {
            waitingSpots[spotIndex] = false;
            patientPositions.Remove(patient);
            waitingPatients = new Queue<GameObject>(waitingPatients.Where(p => p != patient));
        }
    }

    IEnumerator CheckWaitingPatients()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (waitingPatients.Count > 0)
            {
                foreach (TableInteraction table in treatmentTables)
                {
                    if (table != null && !table.isTableLocked && !table.isOccupied)
                    {
                        GameObject patient = waitingPatients.Peek();
                        if (patient != null)
                        {
                            int spotIndex = patientPositions[patient];
                            waitingSpots[spotIndex] = false;
                            patientPositions.Remove(patient);
                            waitingPatients.Dequeue();
                            patient.GetComponent<Patient>()?.AssignToTable(table);
                        }
                        break;
                    }
                }
            }
            yield return null;
        }
    }
    void OnDrawGizmos()
    {
        if (patientWaitingArea == null) return;

        int previewPositions = maxWaitingPatients;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < previewPositions; i++)
        {
            Vector3 position = CalculateWaitingPosition(i);

            Gizmos.DrawWireSphere(position, 0.3f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(position + Vector3.up, $"P{i + 1}");
#endif
        }

        int rows = (previewPositions - 1) / patientsPerRow + 1;
        Vector3 boundsSize = new Vector3(patientsPerRow * spaceBetweenPatients, 0.1f, rows * spaceBetweenPatients);

        // Rotate the bounds to match the waiting area orientation
        Vector3 boundsOffset = patientWaitingArea.rotation * new Vector3(-spaceBetweenPatients * (patientsPerRow - 1) / 2f, 0, -spaceBetweenPatients * (rows - 1) / 2f);

        Vector3 boundsCenter = patientWaitingArea.position + boundsOffset;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boundsCenter, patientWaitingArea.rotation, Vector3.one);

        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawCube(Vector3.zero, boundsSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
