using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    NavMeshAgent agent;
    public Room currentRoom;
    public TableInteraction assignedTable;
    [SerializeField] SO_PatientDetails patientDetails;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindAvailableRoom();
    }
    void Update()
    {
        StartCoroutine(FindRoomTimer());
    }
    IEnumerator FindRoomTimer()
    {
        yield return new WaitForSeconds(5);
        FindAvailableRoom();
    }
    void FindAvailableRoom()
    {
        Room[] rooms = FindObjectsOfType<Room>();
        Debug.Log($"Found {rooms.Length} rooms"); // Debug line


        foreach (Room room in rooms)
        {
            Debug.Log($"Checking room: {room.gameObject.name}, CanAcceptPatient: {room.CanAcceptPatient()}"); // Debug line

            if (room.CanAcceptPatient())
            {
                currentRoom = room;
                currentRoom.AddPatientToQueue(gameObject);
                break;
            }
        }
        if (currentRoom == null)
        {
            Debug.Log("Cannot find a room!");
            //Destroy(gameObject);
        }
    }
    public void AssignToTable(TableInteraction table)
    {
        assignedTable = table;
        agent.SetDestination(table.transform.position);
        StartCoroutine(StartTreatment());
    }
    IEnumerator StartTreatment()
    {
        while (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.remainingDistance > 0.1f)
            yield return null;

        yield return new WaitForSeconds(patientDetails.treatmentTime);

        if (assignedTable != null && assignedTable.so_TableBehavior != null)
        {
            FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(patientDetails.coinDrops);
        }
        ;
        Destroy(gameObject);
    }
}
