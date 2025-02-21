using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    NavMeshAgent agent;
    public Room currentRoom;
    public TableInteraction assignedTable;
    [SerializeField] SO_PatientDetails patientDetails;
    bool isWaiting = false;
    float actualTreatmentDuration;
    bool hasReachedTable = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindAvailableRoom();
    }
    void FindAvailableRoom()
    {
        if (isWaiting) return;
        Room[] rooms = FindObjectsOfType<Room>();
        List<Room> availableRooms = new List<Room>();
        foreach (Room room in rooms)
        {
            if (room.CanAcceptPatient())
            {
                availableRooms.Add(room);
            }
        }

        if (availableRooms.Count > 0)
        {
            int rand = Random.Range(0, availableRooms.Count);
            currentRoom = availableRooms[rand];
            currentRoom.AddPatientToQueue(gameObject);
            isWaiting = true;
        }
        else
        {
            Debug.Log("Cannot find a room!");
        }
    }
    public void AssignToTable(TableInteraction table)
    {
        assignedTable = table;
        table.OccupyTable();
        hasReachedTable = false;

        CalculateTreatmentDuration();
        agent.SetDestination(table.transform.position);
        StartCoroutine(CheckReachedTable());
    }
    void CalculateTreatmentDuration()
    {
        actualTreatmentDuration = patientDetails.treatmentTime;
        if (assignedTable != null && assignedTable.so_TableBehavior != null)
            actualTreatmentDuration /= assignedTable.so_TableBehavior.treatmentCost;
        Debug.Log($"Calculated treatment duration: {actualTreatmentDuration} seconds");
    }
    IEnumerator CheckReachedTable()
    {
        yield return new WaitForSeconds(0.1f);

        while (!hasReachedTable)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                hasReachedTable = true;
                StartCoroutine(StartTreatment());
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StartTreatment()
    {
        Debug.Log("Treatment: " + actualTreatmentDuration);
        yield return new WaitForSeconds(actualTreatmentDuration);

        if (assignedTable != null && assignedTable.so_TableBehavior != null)
        {
            assignedTable.VacateTable();
            FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(patientDetails.coinDrops);
        }
        Destroy(gameObject);
    }
}
