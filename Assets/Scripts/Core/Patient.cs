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
    [SerializeField] SO_PlayerDetails sO_PlayerDetails;
    LevelExperience levelExperience;
    Pharmacy currentPharmacy;

    bool isWaiting = false;
    float actualTreatmentDuration;
    bool hasReachedTable = false;
    public float waitingTimer = 0f;
    public bool hasStartedWaiting = false;
    private bool hasReachedPharmacyPosition = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

#pragma warning disable
        levelExperience = FindObjectOfType<LevelExperience>();

        FindAvailableRoom();
    }
    void Update()
    {
        if (isWaiting && hasStartedWaiting && !hasReachedTable && currentPharmacy == null)
        {
            waitingTimer += Time.deltaTime;
            if (waitingTimer >= patientDetails.waitingPeriod)
            {
                LeaveHospital();
            }
        }
    }
    public void AssignToPharmacy(Pharmacy pharmacy)
    {
        currentPharmacy = pharmacy;
        isWaiting = false;
        pharmacy.AddPatientToQueue(this);
    }
    public void OnPharmacyServiceComplete()
    {
        if (levelExperience != null)
        {
            levelExperience.UpdateReputation(patientDetails.reputation);
            FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(patientDetails.coinDrops);
            levelExperience.AddExperience(patientDetails.xpDrop);
        }

        // Handle patient leaving
        Destroy(gameObject);
    }
    public void OnReachedPharmacyPosition()
    {
        hasReachedPharmacyPosition = true;
    }

    public bool HasReachedPharmacyPosition()
    {
        return hasReachedPharmacyPosition;
    }
    void LeaveHospital()
    {
        if (currentRoom != null)
        {
            currentRoom.RemovePatientFromQueue(gameObject);
            levelExperience?.UpdateReputation(-(patientDetails.reputation / 1.25f));
        }
        Destroy(gameObject);
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
        yield return new WaitForSeconds(actualTreatmentDuration);

        if (assignedTable != null && assignedTable.so_TableBehavior != null)
        {
            assignedTable.VacateTable();
            if (levelExperience != null)
            {
                levelExperience.UpdateReputation(patientDetails.reputation);
                FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(patientDetails.coinDrops);
                levelExperience.AddExperience(patientDetails.xpDrop);
            }
#pragma warning restore
        }
        Destroy(gameObject);
    }
}
