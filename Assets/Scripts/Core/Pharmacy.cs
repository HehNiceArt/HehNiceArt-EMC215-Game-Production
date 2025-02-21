using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pharmacy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform patientWaitingArea;
    public Transform tellerArea;
    [SerializeField] SO_AreaDetails areaDetails;
    [SerializeField] float tellerSpacing = 2f; // Space between tellers
    [SerializeField] GameObject tellerPrefab; // Add this field

    [Header("Settings")]
    [SerializeField] int numberOfTellers = 3;
    [SerializeField] float incomePerPatient = 10f;
    [SerializeField] float spaceBetweenPatients = 1.5f;
    [SerializeField] int patientsPerRow = 3;
    [SerializeField] int maxWaitingPatients = 9;

    private Queue<Patient> patientQueue = new Queue<Patient>();
    private Dictionary<Patient, int> patientPositions = new Dictionary<Patient, int>();
    private bool[] waitingSpots;
    private Teller[] tellers;
    private float totalIncome = 0f;

    void Start()
    {
        if (areaDetails != null && areaDetails.hasPharmacy)
        {
            InitializeTellers();
            waitingSpots = new bool[maxWaitingPatients];
            StartCoroutine(CheckWaitingPatients());
        }
        else
        {
            enabled = false;
        }
    }

    void InitializeTellers()
    {
        tellers = new Teller[numberOfTellers];
        for (int i = 0; i < numberOfTellers; i++)
        {
            Vector3 tellerPosition = tellerArea.position + tellerArea.right * (i - (numberOfTellers - 1) / 2f) * tellerSpacing;

            // Instantiate teller prefab
            GameObject tellerObject = Instantiate(tellerPrefab, tellerPosition, tellerArea.rotation, tellerArea);
            tellerObject.name = $"Teller_{i + 1}";

            tellers[i] = new Teller(tellerPosition, tellerObject);
        }
    }

    public bool CanAcceptPatient()
    {
        return patientQueue.Count < maxWaitingPatients;
    }

    public void AddPatientToQueue(Patient patient)
    {
        if (!CanAcceptPatient()) return;

        int spotIndex = FindFirstAvailableSpot();
        if (spotIndex != -1)
        {
            patientQueue.Enqueue(patient);
            patientPositions[patient] = spotIndex;
            waitingSpots[spotIndex] = true;

            Vector3 waitPos = CalculateWaitingPosition(spotIndex);
            var agent = patient.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(waitPos);
                StartCoroutine(CheckPatientReachedPosition(patient, agent));
            }
        }
    }

    private IEnumerator CheckPatientReachedPosition(Patient patient, NavMeshAgent agent)
    {
        while (true)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                patient.OnReachedPharmacyPosition();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private int FindFirstAvailableSpot()
    {
        for (int i = 0; i < waitingSpots.Length; i++)
        {
            if (!waitingSpots[i]) return i;
        }
        return -1;
    }

    private Vector3 CalculateWaitingPosition(int spotIndex)
    {
        int row = spotIndex / patientsPerRow;
        int col = spotIndex % patientsPerRow;

        Vector3 basePosition = patientWaitingArea.position;
        Vector3 offset = new Vector3(
            (col - (patientsPerRow - 1) / 2f) * spaceBetweenPatients,
            0,
            -row * spaceBetweenPatients
        );

        // Apply waiting area rotation to the offset
        offset = patientWaitingArea.rotation * offset;

        return basePosition + offset;
    }

    IEnumerator CheckWaitingPatients()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ProcessPatientQueue();
        }
    }

    void Update()
    {
        UpdateTellers();
    }

    void UpdateTellers()
    {
        foreach (var teller in tellers)
        {
            teller.Update();
        }
    }

    void ProcessPatientQueue()
    {
        foreach (var teller in tellers)
        {
            if (!teller.IsBusy && patientQueue.Count > 0)
            {
                Patient nextPatient = patientQueue.Peek();
                if (nextPatient != null && nextPatient.HasReachedPharmacyPosition())
                {
                    patientQueue.Dequeue();
                    if (patientPositions.ContainsKey(nextPatient))
                    {
                        int spotIndex = patientPositions[nextPatient];
                        waitingSpots[spotIndex] = false;
                        patientPositions.Remove(nextPatient);
                    }
                    teller.ServePatient(nextPatient);
                    totalIncome += incomePerPatient;
                }
            }
        }
    }

    public float GetTotalIncome()
    {
        return totalIncome;
    }

    void OnDrawGizmos()
    {
        if (patientWaitingArea == null) return;

        // Draw waiting positions
        Gizmos.color = Color.yellow;
        for (int i = 0; i < maxWaitingPatients; i++)
        {
            Vector3 position = CalculateWaitingPosition(i);
            Gizmos.DrawWireSphere(position, 0.3f);
            UnityEditor.Handles.Label(position + Vector3.up, $"P{i + 1}");
        }

        // Draw pharmacy area bounds
        int rows = (maxWaitingPatients - 1) / patientsPerRow + 1;
        Vector3 boundsSize = new Vector3(
            patientsPerRow * spaceBetweenPatients + 4f,
            0.1f,
            numberOfTellers * 2f + 4f
        );

        Vector3 boundsOffset = patientWaitingArea.rotation * new Vector3(
            0,
            0,
            -(numberOfTellers * 2f) / 2f
        );

        Vector3 boundsCenter = patientWaitingArea.position + boundsOffset;

        Matrix4x4 boundsMatrix = Matrix4x4.TRS(boundsCenter, patientWaitingArea.rotation, Vector3.one);
        Gizmos.matrix = boundsMatrix;
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(Vector3.zero, boundsSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
[System.Serializable]
public class Teller
{
    public bool IsBusy { get; private set; }
    private float serviceTime = 3f;
    private float currentServiceTimer = 0f;
    private Patient currentPatient;
    private bool patientReachedCounter = false;
    private Vector3 tellerPosition;
    private GameObject tellerObject; // Add this field

    public Teller(Vector3 position, GameObject tellerObj)
    {
        tellerPosition = position;
        tellerObject = tellerObj;
    }

    public void ServePatient(Patient patient)
    {
        IsBusy = true;
        currentServiceTimer = serviceTime;
        currentPatient = patient;
        patientReachedCounter = false;

        // Move patient to this specific teller's position
        if (currentPatient != null)
        {
            var agent = currentPatient.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(tellerPosition);
                GameObject.FindObjectOfType<Pharmacy>().StartCoroutine(CheckPatientReachedTeller());
            }
        }
    }

    private IEnumerator CheckPatientReachedTeller()
    {
        if (currentPatient == null) yield break;

        var agent = currentPatient.GetComponent<NavMeshAgent>();
        while (agent != null && !patientReachedCounter)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                patientReachedCounter = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Update()
    {
        if (IsBusy && patientReachedCounter)
        {
            currentServiceTimer -= Time.deltaTime;
            if (currentServiceTimer <= 0)
            {
                if (currentPatient != null)
                {
                    currentPatient.OnPharmacyServiceComplete();
                }
                IsBusy = false;
                currentPatient = null;
                patientReachedCounter = false;
            }
        }
    }
}
