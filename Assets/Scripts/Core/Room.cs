using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Room : MonoBehaviour
{
    public List<TableInteraction> treatmentTables = new List<TableInteraction>();
    [SerializeField] Transform patientWaitingArea;
    public SO_AreaDetails areaDetails;
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
        SetTreatmentTablesActive(false);
    }
    void Update()
    {

    }
    public void SetTreatmentTablesActive(bool active)
    {

        foreach (TableInteraction table in treatmentTables)
        {
            if (table != null)
                table.gameObject.SetActive(active);
        }
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

        StartCoroutine(CheckWaitingPatients());
        Patient patient = patientObj.GetComponent<Patient>();
        if (patient == null) return;

        if (areaDetails != null && (areaDetails.whichArea == WhichArea.secondArea ||
        areaDetails.whichArea == WhichArea.fourthArea ||
        areaDetails.whichArea == WhichArea.fifthArea) && ShouldVisitPharmacy())
        {
            //Debug.Log("wdawd");
            if (pharmacy != null && pharmacy.CanAcceptPatient())
            {
                patient.AssignToPharmacy(pharmacy);
                return;
            }
        }

        bool tableFound = false;
        foreach (TableInteraction table in treatmentTables)
        {
            if (table != null && !table.isTableLocked && !table.isOccupied)
            {
                patient.AssignToTable(table);
                tableFound = true;
                break;
            }
        }

        if (tableFound) return;

        /*

        if (waitingPatients.Count == 0)
        {
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
        */

        // If no table availbale or there are waiting patients, add to waiting area
        int spotIndex = FindFirstAvailableSpot();
        if (spotIndex == -1) return;

        waitingSpots[spotIndex] = true;
        patientPositions[patient.gameObject] = spotIndex;
        waitingPatients.Enqueue(patient.gameObject);

        if (patient.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            Vector3 waitPos = CalculateWaitingPosition(spotIndex);
            agent.SetDestination(waitPos);
            StartCoroutine(CheckPatientReachWaitingPos(patient, waitPos));
        }

    }
    private IEnumerator CheckPatientReachWaitingPos(Patient patient, Vector3 waitPos)
    {
        NavMeshAgent agent = patient.GetComponent<NavMeshAgent>();
        if (agent == null) yield break;

        while (Vector3.Distance(patient.transform.position, waitPos) > agent.stoppingDistance)
            yield return new WaitForSeconds(1f);

        patient.hasStartedWaiting = true;
        patient.waitingTimer = 0f;
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
        Debug.Log("thists");
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (waitingPatients.Count > 0)
            {
                Debug.Log($"Checking {waitingPatients.Count} waiting patients");
                foreach (TableInteraction table in treatmentTables)
                {
                    if (table != null && !table.isTableLocked && !table.isOccupied)
                    {
                        Debug.Log($"Found available table: {table.name}");
                        GameObject patientObj = waitingPatients.Peek();
                        if (patientObj != null)
                        {
                            Patient patient = patientObj.GetComponent<Patient>();
                            if (patient != null)
                            {
                                Debug.Log($"Processing patient: {patient.name}");
                                patient.isWaiting = false;
                                patient.hasStartedWaiting = false;
                                patient.waitingTimer = 0f;

                                // Remove from waiting area
                                int spotIndex = patientPositions[patientObj];
                                waitingSpots[spotIndex] = false;
                                patientPositions.Remove(patientObj);
                                waitingPatients.Dequeue();

                                // Assign to table
                                patient.AssignToTable(table);
                                Debug.Log($"Moving patient from waiting area to table {table.name}");
                                break;
                            }
                            /*
                            int spotIndex = patientPositions[patient];
                            waitingSpots[spotIndex] = false;
                            patientPositions.Remove(patient);
                            waitingPatients.Dequeue();
                            patient.GetComponent<Patient>()?.AssignToTable(table);
                            */
                        }
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
