using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool isWaiting = false;
    float actualTreatmentDuration;
    public bool hasReachedTable = false;
    public float waitingTimer = 0f;
    public bool hasStartedWaiting = false;
    public bool hasReachedPharmacyPosition = false;
    public bool hasDecidedToLeave = false;
    public bool hasBeenTreated = false;
    public Transform[] patientSpawnPoints;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patientSpawnPoints = GameObject.FindGameObjectsWithTag("PatientSpawnPoint").Select(go => go.transform).ToArray();

#pragma warning disable
        levelExperience = FindObjectOfType<LevelExperience>();

        FindAvailableRoom();
    }
    void Update()
    {
        if (isWaiting && !hasReachedTable && currentPharmacy == null && !hasDecidedToLeave)
        {
            // If we're close enough to our waiting position, start the timer
            if (!hasStartedWaiting && agent != null && agent.remainingDistance <= agent.stoppingDistance)
            {
                hasStartedWaiting = true;
                waitingTimer = 0f;
            }

            if (hasStartedWaiting)
            {
                waitingTimer += Time.deltaTime;
                if (waitingTimer >= patientDetails.waitingPeriod)
                {
                    hasDecidedToLeave = true;
                    LeaveHospital();
                }
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

        ReturnToSpawn();
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
            FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail((float)Mathf.Floor(-patientDetails.coinDrops / 2));
            levelExperience?.UpdateReputation(-(patientDetails.reputation / 1.25f));
        }
        ReturnToSpawn();
    }

    void ReturnToSpawn()
    {
        int rand = Random.Range(0, patientSpawnPoints.Length);
        agent.SetDestination(patientSpawnPoints[rand].position);
        StartCoroutine(WaitForReturnToSpawn());
    }

    private IEnumerator WaitForReturnToSpawn()
    {
        while (agent.remainingDistance > agent.stoppingDistance || agent.pathPending)
        {
            yield return new WaitForSeconds(0.1f);
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
            if (room.CanAcceptPatient() && room.areaDetails != null)
            {
                availableRooms.Add(room);
            }
        }

        if (availableRooms.Count > 0)
        {
            int rand = Random.Range(0, availableRooms.Count);
            currentRoom = availableRooms[rand];
            currentRoom.AddPatientToQueue(gameObject);
            Debug.Log($"{this.gameObject.name} is assigned to {currentRoom.name}");
            isWaiting = true;
            hasStartedWaiting = false;
            waitingTimer = 0;
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
            GetComponentInChildren<Patient_animManager>()?.SetHealedState();
            assignedTable.VacateTable();
            if (levelExperience != null)
            {
                float reputationGain = patientDetails.reputation;
                if (patientDetails.patientType == PatientType.highProfile)
                    reputationGain *= 1.5f;
                levelExperience.UpdateReputation(reputationGain);
                ReputationManager reputationManager = FindObjectOfType<ReputationManager>();
                Debug.Log($"Rate Up Applied! {reputationManager.RateupMultiplier()}");
                FindObjectOfType<PlayerStats>()?.UpdatePlayerDetail(patientDetails.coinDrops * reputationManager.RateupMultiplier());
                hasBeenTreated = true;
                levelExperience.AddExperience(patientDetails.xpDrop);
            }
#pragma warning restore
        }
        ReturnToSpawn();
    }
}
