using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.UIElements;

public class Room : MonoBehaviour
{
    [SerializeField] List<TableInteraction> treatmentTables = new List<TableInteraction>();
    [SerializeField] Transform patientWaitingArea;
    [SerializeField] SO_AreaDetails areaDetails;
    [SerializeField] float spaceBetweenPatients = 1.5f;
    [SerializeField] int patientsPerRow = 3;
    [SerializeField] int maxWaitingPatients = 6;
    public Transform waitingArea;

    Queue<GameObject> waitingPatients = new Queue<GameObject>();
    Dictionary<GameObject, Vector3> waitingPositions = new Dictionary<GameObject, Vector3>();
    bool isRoomLocked = false;
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
        StartCoroutine(CheckWaitingPatients());

        Debug.Log($"Room {gameObject.name} has {treatmentTables.Count} tables"); // Debug line
    }
    public bool CanAcceptPatient()
    {
        if (areaDetails != null && areaDetails.isLocked)
        {
            Debug.Log($"Room {gameObject.name} is locked"); // Debug line
            return false;
        }

        return true;
    }

    public void AddPatientToQueue(GameObject patient)
    {
        if (patientWaitingArea == null)
        {
            Debug.LogError($"Room {gameObject.name} has no waiting area assigned!");
            return;
        }

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
        if (!tableFound)
        {
            if (waitingPatients.Count >= maxWaitingPatients)
            {
                Debug.Log($"Waiting area full in {gameObject.name}");
                return;
            }

            waitingPatients.Enqueue(patient);
            Vector3 waitingPosition = CalculateWaitingPosition(waitingPatients.Count - 1);
            waitingPositions[patient] = waitingPosition;
            if (patient.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agent.SetDestination(waitingPosition);
            }
        }
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
                        GameObject patient = waitingPatients.Dequeue();
                        if (patient != null)
                        {
                            waitingPositions.Remove(patient);
                            patient.GetComponent<Patient>()?.AssignToTable(table);

                            // Reorganize remaining waiting patients
                            int index = 0;
                            foreach (GameObject waitingPatient in waitingPatients)
                            {
                                Vector3 newPosition = CalculateWaitingPosition(index);
                                waitingPositions[waitingPatient] = newPosition;
                                if (waitingPatient.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
                                {
                                    agent.SetDestination(newPosition);
                                }
                                index++;
                            }
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

            UnityEditor.Handles.Label(position + Vector3.up, $"P{i + 1}");
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
