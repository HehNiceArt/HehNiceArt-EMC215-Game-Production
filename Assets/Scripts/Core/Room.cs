using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<TableInteraction> treatmentTables = new List<TableInteraction>();
    [SerializeField] Transform patientWaitingArea;
    [SerializeField] SO_AreaDetails areaDetails;

    Queue<GameObject> waitingPatients = new Queue<GameObject>();
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

        Debug.Log($"Room {gameObject.name} has {treatmentTables.Count} tables"); // Debug line
    }
    public bool CanAcceptPatient()
    {
        if (areaDetails != null && areaDetails.isLocked)
        {
            Debug.Log($"Room {gameObject.name} is locked"); // Debug line
            return false;
        }
        ;

        foreach (TableInteraction table in treatmentTables)
        {
            if (table != null && !table.isTableLocked)
            {
                Debug.Log($"Room {gameObject.name} has available table"); // Debug line
                return true;
            }
        }
        Debug.Log($"Room {gameObject.name} has no available tables"); // Debug line

        return false;
    }

    public void AddPatientToQueue(GameObject patient)
    {
        foreach (TableInteraction table in treatmentTables)
        {
            if (table != null && !table.isTableLocked)
            {
                patient.GetComponent<Patient>()?.AssignToTable(table);
                break;
            }
        }
    }
    void TryAssignPatientToTable()
    {
        if (waitingPatients.Count == 0) return;
        foreach (TableInteraction table in treatmentTables)
        {
            if (!table.isTableLocked)
            {
                GameObject patient = waitingPatients.Dequeue();
                break;
            }
        }
    }
}
